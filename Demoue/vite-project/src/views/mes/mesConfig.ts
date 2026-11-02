export interface MesField {
  prop: string
  label: string
  type?: 'text' | 'number'
  precision?: number
  required?: boolean
  options?: { label: string; value: number }[]
  ref?: { resource: string; field: string; prefix?: string }
}

export interface MesResourceMeta {
  title: string
  columns: { prop: string; label: string; width?: number; minWidth?: number; enums?: Record<number, string>; ref?: { resource: string; field: string; prefix?: string } }[]
  fields: MesField[]
}

export const mesResources: Record<string, MesResourceMeta> = {
  'work-report': {
    title: '报工单',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'reportNo', label: '报工单号', minWidth: 160 },
      { prop: 'productionOrderId', label: '生产任务', minWidth: 110 , ref: { resource: 'production-order', field: 'prodNo' } },
      { prop: 'goodQty', label: '合格数', minWidth: 110 },
      { prop: 'badQty', label: '不良数', minWidth: 110 },
      { prop: 'workHours', label: '工时', minWidth: 110 },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'待确认',1:'已确认' } },
      { prop: 'reportTime', label: '报工时间', minWidth: 160 }
    ],
    fields: [
      { prop: 'reportNo', label: '报工单号', required: true },
      { prop: 'productionOrderId', label: '生产任务ID', type: 'number' , ref: { resource: 'production-order', field: 'prodNo' } },
      { prop: 'goodQty', label: '合格数', type: 'number', precision: 0 },
      { prop: 'badQty', label: '不良数', type: 'number', precision: 0 },
      { prop: 'workHours', label: '工时', type: 'number', precision: 2 },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '待确认', value: 0 }, { label: '已确认', value: 1 } ] },
      { prop: 'reportTime', label: '报工时间' }
    ]
  },
  'material-issue': {
    title: '领料单',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'issueNo', label: '领料单号', minWidth: 160 },
      { prop: 'workOrderId', label: '工单', minWidth: 110 , ref: { resource: 'work-order', field: 'orderNo' } },
      { prop: 'materialId', label: '物料', minWidth: 110 , ref: { resource: 'material', field: 'materialName', prefix: 'md' } },
      { prop: 'qty', label: '领用数量', minWidth: 110 },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 1:'已领' } },
      { prop: 'issueTime', label: '领料时间', minWidth: 160 },
      { prop: 'remark', label: '备注', minWidth: 160 }
    ],
    fields: [
      { prop: 'issueNo', label: '领料单号', required: true },
      { prop: 'workOrderId', label: '工单ID', type: 'number' , ref: { resource: 'work-order', field: 'orderNo' } },
      { prop: 'materialId', label: '物料ID', type: 'number' , ref: { resource: 'material', field: 'materialName', prefix: 'md' } },
      { prop: 'qty', label: '领用数量', type: 'number', precision: 2 },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '已领', value: 1 } ] },
      { prop: 'issueTime', label: '领料时间' },
      { prop: 'remark', label: '备注' }
    ]
  },
  'production-order': {
    title: '生产任务(派工)',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'prodNo', label: '生产任务号', minWidth: 160 },
      { prop: 'workOrderId', label: '所属工单', minWidth: 110 , ref: { resource: 'work-order', field: 'orderNo' } },
      { prop: 'workstationId', label: '工位', minWidth: 110 , ref: { resource: 'workstation', field: 'name' } },
      { prop: 'planQty', label: '计划数量', minWidth: 110 },
      { prop: 'doneQty', label: '已产数量', minWidth: 110 },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'待开工',1:'生产中',2:'完工' } },
      { prop: 'startTime', label: '开工时间', minWidth: 160 },
      { prop: 'endTime', label: '完工时间', minWidth: 160 },
      { prop: 'remark', label: '备注', minWidth: 160 }
    ],
    fields: [
      { prop: 'prodNo', label: '生产任务号', required: true },
      { prop: 'workOrderId', label: '所属工单ID', type: 'number' , ref: { resource: 'work-order', field: 'orderNo' } },
      { prop: 'workstationId', label: '工位ID', type: 'number' , ref: { resource: 'workstation', field: 'name' } },
      { prop: 'planQty', label: '计划数量', type: 'number', precision: 0 },
      { prop: 'doneQty', label: '已产数量', type: 'number', precision: 0 },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '待开工', value: 0 }, { label: '生产中', value: 1 }, { label: '完工', value: 2 } ] },
      { prop: 'startTime', label: '开工时间' },
      { prop: 'endTime', label: '完工时间' },
      { prop: 'remark', label: '备注' }
    ]
  },
  'workstation': {
    title: '工位/产线',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'code', label: '工位编码', minWidth: 160 },
      { prop: 'name', label: '工位名称', minWidth: 160 },
      { prop: 'line', label: '产线', minWidth: 160 },
      { prop: 'remark', label: '备注', minWidth: 160 }
    ],
    fields: [
      { prop: 'code', label: '工位编码', required: true },
      { prop: 'name', label: '工位名称', required: true },
      { prop: 'line', label: '产线' },
      { prop: 'remark', label: '备注' }
    ]
  },
  'work-order': {
    title: '生产工单',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'orderNo', label: '工单号', minWidth: 160 },
      { prop: 'productId', label: '产品/成品', minWidth: 110 , ref: { resource: 'material', field: 'materialName', prefix: 'md' } },
      { prop: 'planQty', label: '计划数量', minWidth: 110 },
      { prop: 'doneQty', label: '已产数量', minWidth: 110 },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'待下发',1:'生产中',2:'完工' } },
      { prop: 'planStart', label: '计划开工', minWidth: 160 },
      { prop: 'planEnd', label: '计划完工', minWidth: 160 },
      { prop: 'remark', label: '备注', minWidth: 160 },
      { prop: 'prOrderId', label: '关联生产订单', minWidth: 130 , ref: { resource: 'pr', field: 'orderNo', prefix: '' } },
      { prop: 'source', label: '来源', minWidth: 110 , enums: { 1:'手工',2:'生产订单' } }
    ],
    fields: [
      { prop: 'orderNo', label: '工单号', required: true },
      { prop: 'productId', label: '产品/成品ID', type: 'number' , ref: { resource: 'material', field: 'materialName', prefix: 'md' } },
      { prop: 'planQty', label: '计划数量', type: 'number', precision: 0 },
      { prop: 'doneQty', label: '已产数量', type: 'number', precision: 0 },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '待下发', value: 0 }, { label: '生产中', value: 1 }, { label: '完工', value: 2 } ] },
      { prop: 'planStart', label: '计划开工' },
      { prop: 'planEnd', label: '计划完工' },
      { prop: 'remark', label: '备注' },
      { prop: 'prOrderId', label: '关联生产订单', type: 'number' , ref: { resource: 'pr', field: 'orderNo', prefix: '' } },
      { prop: 'source', label: '来源', type: 'number', precision: 0 , options: [ { label: '手工', value: 1 }, { label: '生产订单', value: 2 } ] }
    ]
  },
  'routing': {
    title: '工艺路线',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'routingCode', label: '工艺路线编码', minWidth: 160 },
      { prop: 'productId', label: '产品', minWidth: 110 , ref: { resource: 'material', field: 'materialName', prefix: 'md' } },
      { prop: 'version', label: '版本', minWidth: 160 },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'停用',1:'启用' } },
      { prop: 'remark', label: '备注', minWidth: 160 }
    ],
    fields: [
      { prop: 'routingCode', label: '工艺路线编码', required: true },
      { prop: 'productId', label: '产品ID', type: 'number' , ref: { resource: 'material', field: 'materialName', prefix: 'md' } },
      { prop: 'version', label: '版本' },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '停用', value: 0 }, { label: '启用', value: 1 } ] },
      { prop: 'remark', label: '备注' }
    ]
  },
  'routing-step': {
    title: '工艺工序',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'routingId', label: '工艺路线', minWidth: 110 , ref: { resource: 'routing', field: 'routingCode' } },
      { prop: 'stepNo', label: '工序号', minWidth: 110 },
      { prop: 'stepName', label: '工序名称', minWidth: 160 },
      { prop: 'workstationId', label: '工位', minWidth: 110 , ref: { resource: 'workstation', field: 'name' } },
      { prop: 'stdHours', label: '标准工时', minWidth: 110 },
      { prop: 'remark', label: '备注', minWidth: 160 }
    ],
    fields: [
      { prop: 'routingId', label: '工艺路线ID', type: 'number' , ref: { resource: 'routing', field: 'routingCode' } },
      { prop: 'stepNo', label: '工序号', type: 'number', precision: 0 },
      { prop: 'stepName', label: '工序名称', required: true },
      { prop: 'workstationId', label: '工位ID', type: 'number' , ref: { resource: 'workstation', field: 'name' } },
      { prop: 'stdHours', label: '标准工时', type: 'number', precision: 2 },
      { prop: 'remark', label: '备注' }
    ]
  },
  'quality-check': {
    title: '质量检验',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110 },
      { prop: 'checkNo', label: '质检单号', minWidth: 160 },
      { prop: 'productionOrderId', label: '生产任务', minWidth: 110 , ref: { resource: 'production-order', field: 'prodNo' } },
      { prop: 'workReportId', label: '报工单', minWidth: 110 , ref: { resource: 'work-report', field: 'reportNo' } },
      { prop: 'checkType', label: '检验类型', minWidth: 110 , enums: { 1:'首检',2:'巡检',3:'终检' } },
      { prop: 'qty', label: '检验数量', minWidth: 110 },
      { prop: 'passQty', label: '合格数', minWidth: 110 },
      { prop: 'failQty', label: '不良数', minWidth: 110 },
      { prop: 'result', label: '检验结果', minWidth: 110, enums: { 2:'合格' } },
      { prop: 'checkTime', label: '检验时间', minWidth: 160 },
      { prop: 'remark', label: '备注', minWidth: 160 }
    ],
    fields: [
      { prop: 'checkNo', label: '质检单号', required: true },
      { prop: 'productionOrderId', label: '生产任务ID', type: 'number' , ref: { resource: 'production-order', field: 'prodNo' } },
      { prop: 'workReportId', label: '报工单ID', type: 'number' , ref: { resource: 'work-report', field: 'reportNo' } },
      { prop: 'checkType', label: '检验类型', type: 'number', precision: 0 , options: [ { label: '首检', value: 1 }, { label: '巡检', value: 2 }, { label: '终检', value: 3 } ] },
      { prop: 'qty', label: '检验数量', type: 'number', precision: 0 },
      { prop: 'passQty', label: '合格数', type: 'number', precision: 0 },
      { prop: 'failQty', label: '不良数', type: 'number', precision: 0 },
      { prop: 'result', label: '检验结果', type: 'number', precision: 0, options: [ { label: '合格', value: 2 } ] },
      { prop: 'checkTime', label: '检验时间' },
      { prop: 'remark', label: '备注' }
    ]
  },
}
