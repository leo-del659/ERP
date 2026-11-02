import axios, {
  type AxiosInstance,
  type AxiosResponse,
  type InternalAxiosRequestConfig
} from 'axios'
import { ElMessage } from 'element-plus'
import router from '@/router'


const TOKEN_KEY = 'erp_token'

export function getToken(): string {
  return localStorage.getItem(TOKEN_KEY) || ''
}

export function setToken(token: string) {
  localStorage.setItem(TOKEN_KEY, token)
}

export function clearToken() {
  localStorage.removeItem(TOKEN_KEY)
}

const http: AxiosInstance = axios.create({
  baseURL: '',
  timeout: 30000,
  headers: { 'Content-Type': 'application/json' }
})

http.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = getToken()
    if (token) {
      config.headers.set('Authorization', `Bearer ${token}`)
    }
    const url = config.url
    if (url && url.startsWith('/api/')) {
      config.url = url.replace(/^\/api\//, '/api/v1/')
    }
    return config
  },
  (error) => Promise.reject(error)
)

http.interceptors.response.use(
  (response: AxiosResponse) => {
    const u = response.config.url ?? ''
    if (u.endsWith('/api/auth/login') || u.endsWith('/api/v1/auth/login')) {
      return response.data
    }

    const body = response.data
    if (body == null || typeof body.code === 'undefined') {
      return body
    }

    if (body.code === 1) {
      return body.data // 成功：直接返回业务数据
    }

    ElMessage.error(body.message || '操作失败')
    return Promise.reject(new Error(body.message || '操作失败'))
  },
  (error) => {
    const status = error?.response?.status
    if (status === 401) {
      clearToken()
      ElMessage.error('登录已失效，请重新登录')
      if (router.currentRoute.value.path !== '/login') {
        router.replace('/login')
      }
    } else if (status === 403) {
      ElMessage.error('无权限访问该资源')
    } else {
      ElMessage.error(error?.response?.data?.message || error.message || '网络异常，请稍后重试')
    }
    return Promise.reject(error)
  }
)

export function get<T = any>(url: string, params?: object): Promise<T> {
  return http.get(url, { params }) as unknown as Promise<T>
}

export function post<T = any>(url: string, data?: object): Promise<T> {
  return http.post(url, data) as unknown as Promise<T>
}

export function put<T = any>(url: string, data?: object): Promise<T> {
  return http.put(url, data) as unknown as Promise<T>
}

export function del<T = any>(url: string): Promise<T> {
  return http.delete(url) as unknown as Promise<T>
}

export default http
