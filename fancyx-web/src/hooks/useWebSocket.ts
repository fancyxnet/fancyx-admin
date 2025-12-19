import userStore from '@/store/userStore';
import { useEffect, useRef } from 'react';

interface UseWebSocketOptions {
  reconnectInterval?: number; // 重连间隔，默认 3000ms
  onOpen?: () => void;
  onClose?: () => void;
}

export const useWebSocket = (
  onMessage: (data: any) => void,
  options: UseWebSocketOptions = {}
) => {
  const url = `${import.meta.env.VITE_MQTT_SERVER}?token=${userStore.token?.accessToken}`;
  const { reconnectInterval = 3000, onOpen, onClose } = options;

  const wsRef = useRef<WebSocket | null>(null);
  const reconnectTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  const connect = () => {
    if (!url) return;

    try {
      const ws = new WebSocket(url);
      wsRef.current = ws;

      ws.onopen = () => {
        console.log('[WebSocket] 连接成功');
        onOpen?.();
      };

      ws.onmessage = (event) => {
        try {
          const data = JSON.parse(event.data);

          // 如果是后端发来的 ping，自动回复 pong
          if (data.type === 'ping') {
            ws.send(JSON.stringify({ type: 'pong' }));
            return; // 不触发业务回调
          }

          // 其他消息交给业务层 TODO: hook增加type参数，调用者只处理特定消息
          onMessage(data);
        } catch (e) {
          console.error('[WebSocket] 消息解析失败:', event.data, e);
        }
      };

      ws.onclose = () => {
        console.log('[WebSocket] 连接关闭，准备重连...');
        onClose?.();

        if (reconnectTimeoutRef.current) {
          clearTimeout(reconnectTimeoutRef.current);
        }
        reconnectTimeoutRef.current = setTimeout(() => {
          connect();
        }, reconnectInterval);
      };

      ws.onerror = (error) => {
        console.error('[WebSocket] 连接出错:', error);
        // 注意：onerror 后通常会触发 onclose，所以不在这里重复重连
      };
    } catch (e) {
      console.error('[WebSocket] 创建连接失败:', e);
      reconnectTimeoutRef.current = setTimeout(() => {
        connect();
      }, reconnectInterval);
    }
  };

  useEffect(() => {
    if (!url) return;

    connect();

    return () => {
      if (reconnectTimeoutRef.current) {
        clearTimeout(reconnectTimeoutRef.current);
      }
      if (wsRef.current) {
        wsRef.current.close();
        wsRef.current = null;
      }
    };
  }, [url]);

  // 可选：提供手动发送方法
  const send = (data: any) => {
    if (wsRef.current?.readyState === WebSocket.OPEN) {
      wsRef.current.send(typeof data === 'string' ? data : JSON.stringify(data));
    } else {
      console.warn('[WebSocket] 未连接，无法发送');
    }
  };

  return { send };
};