import { ref } from 'vue'
import { get } from '@/utils/request'

export interface Page_Result<T = any> {
  total: number
  rows: T[]
}

export function usePageList<T = any>(url: string, extra?: () => Record<string, any>) {
  const page = ref(1)
  const size = ref(20)
  const keyword = ref('')
  const total = ref(0)
  const rows = ref<T[]>([]) as any
  const loading = ref(false)

  async function load() {
    loading.value = true
    try {
      const params = {
        page: page.value,
        size: size.value,
        keyword: keyword.value,
        ...(extra ? extra() : {})
      }
      const res = await get<Page_Result<T>>(url, params)
      total.value = res?.total ?? 0
      rows.value = (res?.rows ?? []) as any
    } finally {
      loading.value = false
    }
  }

  function search() {
    page.value = 1
    load()
  }

  function reset() {
    keyword.value = ''
    page.value = 1
    load()
  }

  return { page, size, keyword, total, rows, loading, load, search, reset }
}

export async function fetchOptions<T = any>(entity: string): Promise<T[]> {
  const res = await get<Page_Result<T>>(`/api/md/${entity}`, { page: 1, size: 500 })
  return (res?.rows ?? []) as T[]
}
