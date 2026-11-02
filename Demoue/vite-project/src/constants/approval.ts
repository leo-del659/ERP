export const approvalBizTypes = [
  { label: '采购订单', value: 'PO' },
  { label: '采购请购单', value: 'REQUISITION' },
  { label: '采购收货单', value: 'RECEIPT' },
  { label: '采购订单变更', value: 'PO_CHANGE' },
  { label: '销售订单', value: 'SO' },
  { label: '销售报价', value: 'QUOTE' },
  { label: '客户信用', value: 'CREDIT' },
  { label: '记账凭证', value: 'VOUCHER' },
  { label: '收款单', value: 'FIN_RECEIPT' },
  { label: '付款单', value: 'FIN_PAYMENT' },
  { label: '入库单', value: 'IN_ORDER' },
  { label: '出库单', value: 'OUT_ORDER' },
  { label: '库存调拨单', value: 'TRANSFER' },
  { label: '库存盘点单', value: 'CHECK' },
  { label: '主数据变更', value: 'MD' },
  { label: '其他', value: 'OTHER' }
]

export function approvalBizLabel(v?: string) {
  return approvalBizTypes.find(t => t.value === v)?.label ?? v ?? '-'
}
