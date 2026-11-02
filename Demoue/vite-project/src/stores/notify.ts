import { defineStore } from 'pinia'

/**
 * 全局消息中心（右上角铃铛）
 * - 消息持久化到 localStorage，刷新不丢失
 * - 库存不足、发货提醒等业务消息统一从这里进
 */
export interface NoticeItem {
  /** 去重键：同类业务消息只保留最新一条（如 stock:{material}:{warehouse}） */
  key: string
  type: 'stock' | 'so' | 'sys'
  title: string
  content: string
  time: string
  read: boolean
}

const STORAGE_KEY = 'erp_notices'

export const useNotifyStore = defineStore('notify', {
  state: () => ({
    items: [] as NoticeItem[]
  }),
  getters: {
    unread: (s) => s.items.filter((i) => !i.read).length
  },
  actions: {
    restore() {
      try {
        this.items = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]')
      } catch {
        this.items = []
      }
    },
    persist() {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(this.items.slice(0, 50)))
    },
    /** 推送消息：相同 key 置顶更新；内容未变化时保持已读状态（刷新/重登不重新亮红点） */
    push(n: Omit<NoticeItem, 'time' | 'read'>) {
      const time = new Date().toLocaleString('zh-CN', { hour12: false })
      const exist = this.items.find((i) => i.key === n.key)
      if (exist) {
        const changed = exist.title !== n.title || exist.content !== n.content
        exist.title = n.title
        exist.content = n.content
        exist.time = time
        if (changed) exist.read = false
        this.items = [exist, ...this.items.filter((i) => i.key !== n.key)]
      } else {
        this.items.unshift({ ...n, time, read: false })
      }
      this.persist()
    },
    markAllRead() {
      this.items.forEach((i) => (i.read = true))
      this.persist()
    },
    remove(key: string) {
      this.items = this.items.filter((i) => i.key !== key)
      this.persist()
    },
    clear() {
      this.items = []
      this.persist()
    }
  }
})

/** 消息去重键 */
export const stockNoticeKey = (materialCode: string, warehouseId: number | string) => `stock:${materialCode}:${warehouseId}`
