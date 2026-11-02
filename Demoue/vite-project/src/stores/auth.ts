import { defineStore } from 'pinia'
import { post, getToken, setToken, clearToken } from '@/utils/request'

interface UserInfo {
  id: number
  username: string
  realName?: string
  roleCode?: string
}

interface LoginResult {
  token: string
  user: UserInfo
  paths: string[]
}

interface AuthState {
  token: string
  user: UserInfo | null
  paths: string[]
}

const USER_KEY = 'erp_user'
const PATHS_KEY = 'erp_paths'

// 容错读取 localStorage，避免脏数据导致解析失败
function safeParse<T>(key: string, fallback: T): T {
  try {
    const raw = localStorage.getItem(key)
    return raw ? (JSON.parse(raw) as T) : fallback
  } catch {
    localStorage.removeItem(key)
    return fallback
  }
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    token: getToken(),
    user: safeParse<UserInfo | null>(USER_KEY, null),
    paths: safeParse<string[]>(PATHS_KEY, [])
  }),

  getters: {
    isLoggedIn: (s) => !!s.token,
    // 是否拥有某菜单/按钮权限（后端 Permission_Filter 的同名 key）
    can: (s) => (key: string) => s.paths.includes(key)
  },

  actions: {
    async login(username: string, password: string) {
      const res = await post<LoginResult>('/api/auth/login', { username, password })
      this.token = res.token
      this.user = res.user
      this.paths = res.paths || []
      setToken(res.token)
      localStorage.setItem(USER_KEY, JSON.stringify(res.user))
      localStorage.setItem(PATHS_KEY, JSON.stringify(res.paths || []))
      return res
    },

    logout() {
      clearToken()
      this.token = ''
      this.user = null
      this.paths = []
      localStorage.removeItem(USER_KEY)
      localStorage.removeItem(PATHS_KEY)
    }
  }
})
