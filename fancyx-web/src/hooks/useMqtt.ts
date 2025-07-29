import { useEffect, useMemo, useCallback } from 'react';
import mqtt, { type MqttClient, type IClientOptions } from 'mqtt';
import { observable, action, runInAction } from 'mobx';
import { getMqttToken } from '@/api/mqtt.ts';
import BrowserUtils from '@/utils/browserUtils.ts';

type ConnectionStatus = 'disconnected' | 'connecting' | 'connected' | 'reconnecting' | 'error';

interface MqttMessage {
  topic: string;
  payload: string;
  timestamp: string;
}

interface TokenInfo {
  token: string;
  expiresAt: number; // 过期时间戳(毫秒)
}

// 创建响应式状态
const createMqttState = () => {
  const state = observable({
    client: null as MqttClient | null,
    status: 'disconnected' as ConnectionStatus,
    messages: {} as Record<string, MqttMessage[]>,
    subscriptions: [] as string[],
    tokenInfo: null as TokenInfo | null,
    tokenRefreshPromise: null as Promise<string> | null,

    // Actions
    setStatus: action(function (this: typeof state, status: ConnectionStatus) {
      this.status = status;
    }),

    setTokenInfo: action(function (this: typeof state, tokenInfo: TokenInfo | null) {
      this.tokenInfo = tokenInfo;
    }),

    setTokenRefreshPromise: action(function (this: typeof state, promise: Promise<string> | null) {
      this.tokenRefreshPromise = promise;
    }),

    addMessage: action(function (this: typeof state, topic: string, payload: string) {
      if (!this.messages[topic]) {
        this.messages[topic] = [];
      }
      this.messages[topic].push({
        topic,
        payload,
        timestamp: new Date().toISOString(),
      });
    }),

    addSubscription: action(function (this: typeof state, topic: string) {
      if (!this.subscriptions.includes(topic)) {
        this.subscriptions.push(topic);
      }
    }),

    removeSubscription: action(function (this: typeof state, topic: string) {
      this.subscriptions = this.subscriptions.filter((t) => t !== topic);
    }),

    clearMessages: action(function (this: typeof state, topic?: string) {
      if (topic) {
        delete this.messages[topic];
      } else {
        this.messages = {};
      }
    }),
  });

  return state;
};

export const useMqtt = (
  topic: string,
  options?: {
    onMessage?: (payload: string) => void;
    autoConnect?: boolean;
    autoSubscribe?: boolean;
    tokenRefreshThreshold?: number; // 提前刷新token的阈值(毫秒)，默认5分钟
  },
) => {
  const {
    onMessage,
    autoConnect = true,
    autoSubscribe = true,
    tokenRefreshThreshold = 5 * 60 * 1000, // 默认提前5分钟刷新token
  } = options || {};

  // 使用 useMemo 保持 state 单例
  const mqttState = useMemo(() => createMqttState(), []);

  // 获取或刷新token
  const getOrRefreshToken = useCallback(
    async (forceRefresh = false): Promise<string> => {
      // 如果有正在进行的刷新请求，直接返回这个Promise
      if (mqttState.tokenRefreshPromise && !forceRefresh) {
        return mqttState.tokenRefreshPromise;
      }

      // 检查现有token是否有效
      const currentTokenValid =
        mqttState.tokenInfo && mqttState.tokenInfo.expiresAt > Date.now() + tokenRefreshThreshold;

      if (currentTokenValid && !forceRefresh) {
        return mqttState.tokenInfo!.token;
      }

      // 开始新的token获取
      const browserCode = await BrowserUtils.getBrowserCode();
      const tokenPromise = getMqttToken(browserCode)
        .then((res) => {
          const { expired, token } = res.data;
          const tokenInfo = {
            token,
            expiresAt: Date.now() + expired * 1000, // 转换为毫秒
          };
          runInAction(() => {
            mqttState.setTokenInfo(tokenInfo);
            mqttState.setTokenRefreshPromise(null);
          });
          return token;
        })
        .catch((err) => {
          runInAction(() => {
            mqttState.setTokenRefreshPromise(null);
          });
          throw err;
        });

      runInAction(() => {
        mqttState.setTokenRefreshPromise(tokenPromise);
      });

      return tokenPromise;
    },
    [mqttState, tokenRefreshThreshold],
  );

  // 连接MQTT
  const connect = useCallback(async () => {
    if (mqttState.status === 'connected' || mqttState.status === 'connecting') return;

    try {
      mqttState.setStatus('connecting');

      // 获取token
      const token = await getOrRefreshToken();

      const wsUrl = import.meta.env.VITE_MQTT_SERVER;
      const envMode = import.meta.env.MODE;
      const clientId = `mqtt_web_${Math.random().toString(16).substring(2, 10)}`;

      const client = mqtt.connect(wsUrl, {
        clientId,
        protocol: envMode === 'production' ? 'wss' : 'ws',
        reconnectPeriod: 5000,
        connectTimeout: 3000,
        username: token,
        keepalive: 60,
        clean: true,
      });

      // 设置token过期检查定时器
      const checkTokenExpiration = () => {
        if (mqttState.tokenInfo && mqttState.tokenInfo.expiresAt <= Date.now() + tokenRefreshThreshold) {
          // Token即将过期，主动刷新
          getOrRefreshToken(true).then((newToken) => {
            if (client.connected) {
              // 如果使用token作为密码，也需要更新
              (client.options as any).username = newToken;
              // 对于已连接的客户端，可能需要重新连接以应用新token
              client.reconnect();
            }
          });
        }
      };

      // 每30秒检查一次token过期
      const tokenCheckInterval = setInterval(checkTokenExpiration, 30 * 1000);

      client.on('connect', () => {
        runInAction(() => {
          mqttState.setStatus('connected');
          // 重新订阅已有主题
          mqttState.subscriptions.forEach((t) => client.subscribe(t));
        });
      });

      client.on('reconnect', () => {
        mqttState.setStatus('reconnecting');
        // 在重连时检查token是否需要刷新
        getOrRefreshToken().catch(() => {
          // 如果token刷新失败，停止重连
          client.end();
        });
      });

      client.on('close', () => {
        clearInterval(tokenCheckInterval);
        mqttState.setStatus('disconnected');
      });

      client.on('error', (err) => {
        console.error('MQTT error:', err);
        mqttState.setStatus('error');
      });

      client.on('message', (receivedTopic, payload) => {
        runInAction(() => {
          mqttState.addMessage(receivedTopic, payload.toString());
        });
        if (receivedTopic === topic && onMessage) {
          onMessage(payload.toString());
        }
      });

      runInAction(() => {
        mqttState.client = client;
      });

      return () => {
        clearInterval(tokenCheckInterval);
      };
    } catch (err) {
      console.error('MQTT connection failed:', err);
      mqttState.setStatus('error');
      throw err;
    }
  }, [mqttState, getOrRefreshToken, topic, onMessage, tokenRefreshThreshold]);

  // 断开连接
  const disconnect = useCallback(() => {
    if (mqttState.client) {
      mqttState.client.end();
      runInAction(() => {
        mqttState.client = null;
        mqttState.setStatus('disconnected');
      });
    }
  }, [mqttState]);

  // 连接管理
  useEffect(() => {
    if (!autoConnect) return;

    connect();

    return () => {
      disconnect();
    };
  }, [autoConnect, disconnect]);

  // 订阅管理
  useEffect(() => {
    if (!autoSubscribe || !mqttState.client || mqttState.status !== 'connected') return;

    mqttState.client.subscribe(topic, (err) => {
      if (!err) {
        runInAction(() => {
          mqttState.addSubscription(topic);
        });
      }
    });

    return () => {
      if (mqttState.client && mqttState.status === 'connected') {
        mqttState.client.unsubscribe(topic, (err) => {
          if (!err) {
            runInAction(() => {
              mqttState.removeSubscription(topic);
            });
          }
        });
      }
    };
  }, [autoSubscribe, mqttState.status, topic]);

  // 返回 API
  return {
    status: mqttState.status,
    messages: mqttState.messages[topic] || [],
    publish: (payload: string, publishOptions?: IClientOptions) => {
      if (mqttState.client && mqttState.status === 'connected') {
        mqttState.client.publish(topic, payload, publishOptions);
      }
    },
    subscribe: () => {
      if (mqttState.client && mqttState.status === 'connected') {
        mqttState.client.subscribe(topic, (err) => {
          if (!err) {
            runInAction(() => {
              mqttState.addSubscription(topic);
            });
          }
        });
      }
    },
    unsubscribe: () => {
      if (mqttState.client && mqttState.status === 'connected') {
        mqttState.client.unsubscribe(topic, (err) => {
          if (!err) {
            runInAction(() => {
              mqttState.removeSubscription(topic);
            });
          }
        });
      }
    },
    clearMessages: () => mqttState.clearMessages(topic),
    connect, // 暴露connect方法以便手动连接
    disconnect, // 暴露disconnect方法
    refreshToken: () => getOrRefreshToken(true), // 暴露手动刷新token的方法
  };
};
