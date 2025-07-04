import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';
import UserStore from '@/store/userStore';
import { ErrorCode } from '@/utils/globalValue.ts';
import { message } from 'antd';

class HttpClient {
  private readonly instance: AxiosInstance;

  constructor(config?: AxiosRequestConfig) {
    this.instance = axios.create(config);

    // 请求拦截器
    this.instance.interceptors.request.use(
      (config) => {
        // 可以在这里添加token等
        const token = UserStore.token?.accessToken;
        if (token) {
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
        if (error.code === 'ERR_NETWORK') {
          msg = '网络错误，请联系管理员';
        } else if (error.response) {
          switch (error.response.status) {
            case 401:
              msg = '身份信息过期，请重新登录';
              break;
            case 404:
              msg = '请求接口不存在';
              break;
            case 405:
              msg = '请求方法错误';
              break;
          }
        }
        message.error(msg);
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
