import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';
import UserStore from '@/store/userStore';
import { ErrorCode, StaticRoutes } from '@/utils/globalValue.ts';
import { message } from 'antd';
import dayjs from 'dayjs';

class HttpClient {
  private readonly instance: AxiosInstance;
  allowAnonymousApis: string[] = ['/admin-api/account/login']; //允许匿名访问接口
  
  // 错误提示防抖相关
  private static lastErrorTime = 0;
  private static errorTimeout = 3000; // 3秒内只显示一次相同错误
  private static errorQueue: Array<{msg: string, jumpLogin: boolean}> = [];

  constructor(config?: AxiosRequestConfig) {
    this.instance = axios.create(config);

    // 请求拦截器
    this.instance.interceptors.request.use(
      async (config) => {
        if (config.url && this.allowAnonymousApis.includes(config.url)) {
          return config;
        }
        //添加token
        const token = UserStore.token?.accessToken;
        const expired = UserStore.token?.expiredTime;
        const now = new Date();
        if (token && expired && dayjs(expired).isAfter(now)) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      },
    );

    // 响应拦截器
    this.instance.interceptors.response.use(
      (response) => {
        /** 统一返回结果响应码不等于成功，中断请求；这样做是确保.then()中是响应成功 */
        if (response.data.code && response.data.code !== ErrorCode.Success) {
          const errMsg = response.data.message ?? '请求失败';
          message.error(errMsg);
          return Promise.reject(errMsg);
        }
        return response.data;
      },
      (error) => {
        let msg = '异常错误，请联系管理员';
        let jumpLogin = false;
        if (error.code === 'ERR_NETWORK') {
          msg = '网络错误，请联系管理员';
        } else if (error.response) {
          switch (error.response.status) {
            case 401:
              msg = '身份信息过期，请重新登录';
              UserStore.logout();
              jumpLogin = true;
              break;
            case 404:
              msg = '请求接口不存在';
              break;
            case 405:
              msg = '请求方法错误';
              break;
          }
        }
        
        // 处理错误提示的防抖逻辑
        const currentTime = Date.now();
        
        // 登录过期错误总是立即显示
        if (jumpLogin) {
          message.error(msg, 1, () => {
            window.location.href = StaticRoutes.Login;
          });
        } else {
          // 对于其他错误，实现防抖处理
          if (currentTime - HttpClient.lastErrorTime > HttpClient.errorTimeout) {
            // 只有当时间间隔超过设定值时才显示新的错误提示
            HttpClient.lastErrorTime = currentTime;
            
            message.error(msg, 3, () => {
              // 检查是否有队列中的错误需要显示
              if (HttpClient.errorQueue.length > 0) {
                const nextError = HttpClient.errorQueue.shift();
                if (nextError) {
                  HttpClient.lastErrorTime = Date.now(); // 更新最后显示时间
                  message.error(nextError.msg, 3, () => {
                    if (nextError.jumpLogin) {
                      window.location.href = StaticRoutes.Login;
                    }
                  });
                }
              }
            });
          } else if (HttpClient.errorQueue.length === 0) {
            // 只保留第一个错误在队列中，避免队列过长
            HttpClient.errorQueue.push({ msg, jumpLogin });
          }
        }
        
        return Promise.reject(error);
      },
    );
  }

  // GET请求
  public get<TRequest = any, TResponse = any>(url: string, config?: AxiosRequestConfig): Promise<TResponse> {
    return this.instance.get<TRequest, TResponse>(url, config);
  }

  // POST请求
  public post<TRequest = any, TResponse = any>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig,
  ): Promise<TResponse> {
    return this.instance.post<TRequest, TResponse>(url, data, config);
  }

  // PUT请求
  public put<TRequest = any, TResponse = any>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig,
  ): Promise<TResponse> {
    return this.instance.put<TRequest, TResponse>(url, data, config);
  }

  // DELETE请求
  public delete<TRequest = any, TResponse = any>(url: string, config?: AxiosRequestConfig): Promise<TResponse> {
    return this.instance.delete<TRequest, TResponse>(url, config);
  }

  // PATCH请求
  public patch<TRequest = any, TResponse = any>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig,
  ): Promise<TResponse> {
    return this.instance.patch<TRequest, TResponse>(url, data, config);
  }

  // 获取原始Axios实例
  public getInstance(): AxiosInstance {
    return this.instance;
  }
}

// 默认配置
const defaultConfig: AxiosRequestConfig = {
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 10000,
};

// 创建默认实例
const httpClient = new HttpClient(defaultConfig);

export default httpClient;
