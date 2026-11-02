
export interface MesRowAction {
  label: string
  url: string
  method?: 'post' | 'put'
  confirm?: boolean
}

export interface MesToolbarAction {
  label: string
  url: string
  method?: 'post'
  body?: Record<string, any>
}

export const mesBizActions: Record<
  string,
  {
    rowActions?: MesRowAction[]
    toolbarActions?: MesToolbarAction[]
    disableCreate?: boolean
    createHint?: string
  }
> = {
  'work-order': {
    disableCreate: true,
    createHint: 'MES 工单仅允许由生产订单派生，请使用「拉取生产订单」生成',
    rowActions: [
      { label: '下发生产', url: '/api/mes/biz/work-order/{id}/dispatch', method: 'post', confirm: true },
      { label: '回写生产订单', url: '/api/mes/biz/work-order/{id}/sync-pr', method: 'post', confirm: true }
    ],
    toolbarActions: [
      { label: '拉取生产订单', url: '/api/mes/biz/work-order/sync-from-pr', method: 'post' }
    ]
  },
  'production-order': {
    rowActions: [
      { label: '领料', url: '/api/mes/biz/production-order/{id}/issue', method: 'post', confirm: true },
      { label: '报工', url: '/api/mes/biz/production-order/{id}/report', method: 'post', confirm: true }
    ]
  },
  'work-report': {
    rowActions: [
      { label: '质检', url: '/api/mes/biz/work-report/{id}/qc', method: 'post', confirm: true }
    ]
  }
}
