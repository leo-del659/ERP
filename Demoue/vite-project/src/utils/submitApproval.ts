import { post } from '@/utils/request'

export interface ApprovalSubmit {
  bizType: string
  bizId?: number | null
  billNo?: string | null
  title: string
  amount?: number | null
  totalNodes?: number
  remark?: string
}

export async function submitApproval(p: ApprovalSubmit): Promise<boolean> {
  try {
    await post('/api/approval/submit', {
      bizType: p.bizType,
      bizId: p.bizId ?? null,
      billNo: p.billNo ?? '',
      title: p.title,
      amount: p.amount ?? null,
      totalNodes: p.totalNodes ?? 1,
      remark: p.remark ?? '新建自动送审'
    })
    return true
  } catch {
    return false
  }
}
