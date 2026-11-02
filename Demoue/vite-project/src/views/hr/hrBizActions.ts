
export interface HrRowAction {
  label: string
  url: string
  method?: 'post' | 'put'
  confirm?: boolean
}

export interface HrToolbarAction {
  label: string
  url: string
  method?: 'post'
  body?: Record<string, any>
}

export const hrBizActions: Record<
  string,
  { rowActions?: HrRowAction[]; toolbarActions?: HrToolbarAction[] }
> = {
  leave: {
    rowActions: [
      { label: '审批通过', url: '/api/hr/biz/leave/{id}/approve', method: 'post', confirm: true }
    ]
  },
  overtime: {
    rowActions: [
      { label: '审批通过', url: '/api/hr/biz/overtime/{id}/approve', method: 'post', confirm: true }
    ]
  },
  salary: {
    toolbarActions: [
      {
        label: '核算本月薪资',
        url: '/api/hr/biz/salary/calculate',
        method: 'post',
        body: { year: new Date().getFullYear(), month: new Date().getMonth() + 1 }
      }
    ]
  }
}
