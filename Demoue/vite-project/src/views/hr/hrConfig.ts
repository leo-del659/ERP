export interface HrField {
  prop: string
  label: string
  type?: 'text' | 'number'
  precision?: number
  required?: boolean
  options?: { label: string; value: number }[]
  ref?: { resource: string; field: string; prefix?: string }
}

export interface HrResourceMeta {
  title: string
  columns: { prop: string; label: string; width?: number; minWidth?: number; enums?: Record<number, string>; ref?: { resource: string; field: string; prefix?: string } }[]
  fields: HrField[]
}

export const hrResources: Record<string, HrResourceMeta> = {
  'department': {
    title: '部门（组织层级）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'deptNo', label: '部门编码', minWidth: 160  },
      { prop: 'deptName', label: '部门名称', minWidth: 160  },
      { prop: 'managerName', label: '负责人', minWidth: 110  },
      { prop: 'phone', label: '联系电话', minWidth: 160  },
      { prop: 'address', label: '地址', minWidth: 160  },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'停用',1:'启用' } }
    ],
    fields: [
      { prop: 'deptNo', label: '部门编码', required: true  },
      { prop: 'deptName', label: '部门名称', required: true  },
      { prop: 'parentId', label: '上级部门ID', type: 'number' , options: [ { label: '顶级', value: 0 } ] },
      { prop: 'managerName', label: '负责人'  },
      { prop: 'phone', label: '联系电话'  },
      { prop: 'address', label: '地址'  },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '停用', value: 0 }, { label: '启用', value: 1 } ] }
    ]
  },
  'position': {
    title: '岗位体系',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'posNo', label: '岗位编码', minWidth: 160  },
      { prop: 'posName', label: '岗位名称', minWidth: 160  },
      { prop: 'deptId', label: '归属部门', minWidth: 110  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'parentId', label: '上级岗位', minWidth: 110 , enums: { 0:'顶级' } , ref: { resource: 'position', field: 'posName' } },
      { prop: 'jobFamily', label: '职族/序列', minWidth: 160  },
      { prop: 'responsibility', label: '岗位职责', minWidth: 160  },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'停用',1:'启用' } }
    ],
    fields: [
      { prop: 'posNo', label: '岗位编码', required: true  },
      { prop: 'posName', label: '岗位名称', required: true  },
      { prop: 'deptId', label: '归属部门', type: 'number'  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'parentId', label: '上级岗位', type: 'number' , options: [ { label: '顶级', value: 0 } ] , ref: { resource: 'position', field: 'posName' } },
      { prop: 'jobFamily', label: '职族/序列'  },
      { prop: 'responsibility', label: '岗位职责'  },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '停用', value: 0 }, { label: '启用', value: 1 } ] }
    ]
  },
  'position-quota': {
    title: '岗位编制控制',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'deptId', label: '部门', minWidth: 110  , ref: { resource: 'department', field: 'deptName' }  },
      { prop: 'posId', label: '岗位', minWidth: 110  , ref: { resource: 'position', field: 'posName' }  },
      { prop: 'planCount', label: '编制数', minWidth: 110  },
      { prop: 'actualCount', label: '在岗数', minWidth: 110  },
      { prop: 'frozenCount', label: '冻结/预留数', minWidth: 110  },
      { prop: 'remark', label: '备注', minWidth: 160  }
    ],
    fields: [
      { prop: 'deptId', label: '部门', type: 'number'  , ref: { resource: 'department', field: 'deptName' }  },
      { prop: 'posId', label: '岗位', type: 'number'  , ref: { resource: 'position', field: 'posName' }  },
      { prop: 'planCount', label: '编制数', type: 'number', precision: 0  },
      { prop: 'actualCount', label: '在岗数', type: 'number', precision: 0  },
      { prop: 'frozenCount', label: '冻结/预留数', type: 'number', precision: 0  },
      { prop: 'remark', label: '备注'  }
    ]
  },
  'employee': {
    title: '员工主档',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empNo', label: '工号', minWidth: 160  },
      { prop: 'name', label: '姓名', minWidth: 160  },
      { prop: 'gender', label: '性别', minWidth: 110 , enums: { 0:'女',1:'男' } },
      { prop: 'birthday', label: '出生日期', minWidth: 110  },
      { prop: 'idCard', label: '身份证', minWidth: 160  },
      { prop: 'nation', label: '民族', minWidth: 160  },
      { prop: 'marital', label: '婚姻状况', minWidth: 110 , enums: { 0:'未婚',1:'已婚' } },
      { prop: 'education', label: '学历', minWidth: 160  },
      { prop: 'phone', label: '联系电话', minWidth: 160  },
      { prop: 'email', label: '邮箱', minWidth: 160  },
      { prop: 'address', label: '地址', minWidth: 160  },
      { prop: 'deptId', label: '所属部门', minWidth: 110  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'posId', label: '所属岗位', minWidth: 110  , ref: { resource: 'position', field: 'posName' } },
      { prop: 'empStatus', label: '员工状态', minWidth: 110 , enums: { 0:'试用',1:'正式',2:'休假',3:'离职',4:'借调' } },
      { prop: 'hireDate', label: '入职日期', minWidth: 110  },
      { prop: 'regularDate', label: '转正日期', minWidth: 110  },
      { prop: 'leaveDate', label: '离职日期', minWidth: 110  },
      { prop: 'contractCount', label: '合同份数', minWidth: 110  }
    ],
    fields: [
      { prop: 'empNo', label: '工号', required: true  },
      { prop: 'name', label: '姓名', required: true  },
      { prop: 'gender', label: '性别', type: 'number', precision: 0 , options: [ { label: '女', value: 0 }, { label: '男', value: 1 } ] },
      { prop: 'birthday', label: '出生日期'  },
      { prop: 'idCard', label: '身份证'  },
      { prop: 'nation', label: '民族'  },
      { prop: 'marital', label: '婚姻状况', type: 'number', precision: 0 , options: [ { label: '未婚', value: 0 }, { label: '已婚', value: 1 } ] },
      { prop: 'education', label: '学历'  },
      { prop: 'phone', label: '联系电话'  },
      { prop: 'email', label: '邮箱'  },
      { prop: 'address', label: '地址'  },
      { prop: 'deptId', label: '所属部门', type: 'number'  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'posId', label: '所属岗位', type: 'number'  , ref: { resource: 'position', field: 'posName' } },
      { prop: 'empStatus', label: '员工状态', type: 'number', precision: 0 , options: [ { label: '试用', value: 0 }, { label: '正式', value: 1 }, { label: '休假', value: 2 }, { label: '离职', value: 3 }, { label: '借调', value: 4 } ] },
      { prop: 'hireDate', label: '入职日期'  },
      { prop: 'regularDate', label: '转正日期'  },
      { prop: 'leaveDate', label: '离职日期'  },
      { prop: 'contractCount', label: '合同份数', type: 'number', precision: 0  }
    ]
  },
  'employee-change': {
    title: '人事异动轨迹（全周期）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'changeType', label: '异动类型', minWidth: 110 , enums: { 1:'入职',2:'转正',3:'调动',4:'晋升',5:'降职',6:'离职',7:'复职' } },
      { prop: 'oldDeptId', label: '原部门', minWidth: 110 , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'newDeptId', label: '新部门', minWidth: 110 , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'oldPosId', label: '原岗位', minWidth: 110 , ref: { resource: 'position', field: 'posName' } },
      { prop: 'newPosId', label: '新岗位', minWidth: 110 , ref: { resource: 'position', field: 'posName' } },
      { prop: 'oldStatus', label: '原员工状态', minWidth: 110 , enums: { 0:'试用',1:'正式',2:'休假',3:'离职',4:'借调' } },
      { prop: 'newStatus', label: '新员工状态', minWidth: 110 , enums: { 0:'试用',1:'正式',2:'休假',3:'离职',4:'借调' } },
      { prop: 'effectDate', label: '生效日期', minWidth: 110  },
      { prop: 'reason', label: '原因', minWidth: 160  },
      { prop: 'operator', label: '操作人', minWidth: 160  }
    ],
    fields: [
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'changeType', label: '异动类型', type: 'number', precision: 0 , options: [ { label: '入职', value: 1 }, { label: '转正', value: 2 }, { label: '调动', value: 3 }, { label: '晋升', value: 4 }, { label: '降职', value: 5 }, { label: '离职', value: 6 }, { label: '复职', value: 7 } ] },
      { prop: 'oldDeptId', label: '原部门', type: 'number' , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'newDeptId', label: '新部门', type: 'number' , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'oldPosId', label: '原岗位', type: 'number' , ref: { resource: 'position', field: 'posName' } },
      { prop: 'newPosId', label: '新岗位', type: 'number' , ref: { resource: 'position', field: 'posName' } },
      { prop: 'oldStatus', label: '原员工状态', type: 'number', precision: 0 , options: [ { label: '试用', value: 0 }, { label: '正式', value: 1 }, { label: '休假', value: 2 }, { label: '离职', value: 3 }, { label: '借调', value: 4 } ] },
      { prop: 'newStatus', label: '新员工状态', type: 'number', precision: 0 , options: [ { label: '试用', value: 0 }, { label: '正式', value: 1 }, { label: '休假', value: 2 }, { label: '离职', value: 3 }, { label: '借调', value: 4 } ] },
      { prop: 'effectDate', label: '生效日期'  },
      { prop: 'reason', label: '原因'  },
      { prop: 'operator', label: '操作人'  }
    ]
  },
  'contract': {
    title: '劳动合同（含到期预警：按 end_date 查询）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'contractNo', label: '合同编号', minWidth: 160  },
      { prop: 'contractType', label: '合同类型', minWidth: 110 , enums: { 1:'固定期限',2:'无固定期限',3:'实习',4:'劳务' } },
      { prop: 'startDate', label: '开始日期', minWidth: 110   },
      { prop: 'endDate', label: '结束日期', minWidth: 110   },
      { prop: 'signDate', label: '签订日期', minWidth: 110  },
      { prop: 'probationMonths', label: '试用期(月)', minWidth: 110  },
      { prop: 'salary', label: '合同薪资', minWidth: 110  },
      { prop: 'status', label: '合同状态', minWidth: 110 , enums: { 0:'草稿',1:'生效',2:'续签',3:'变更',4:'解除',5:'到期' } },
      { prop: 'remark', label: '备注', minWidth: 160  }
    ],
    fields: [
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'contractNo', label: '合同编号', required: true  },
      { prop: 'contractType', label: '合同类型', type: 'number', precision: 0 , options: [ { label: '固定期限', value: 1 }, { label: '无固定期限', value: 2 }, { label: '实习', value: 3 }, { label: '劳务', value: 4 } ] },
      { prop: 'startDate', label: '开始日期'   },
      { prop: 'endDate', label: '结束日期'   },
      { prop: 'signDate', label: '签订日期'  },
      { prop: 'probationMonths', label: '试用期(月)', type: 'number', precision: 0  },
      { prop: 'salary', label: '合同薪资', type: 'number', precision: 2  },
      { prop: 'status', label: '合同状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '生效', value: 1 }, { label: '续签', value: 2 }, { label: '变更', value: 3 }, { label: '解除', value: 4 }, { label: '到期', value: 5 } ] },
      { prop: 'remark', label: '备注'  }
    ]
  },
  'shift': {
    title: '班次定义',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'shiftNo', label: '班次编码', minWidth: 160  },
      { prop: 'shiftName', label: '班次名称', minWidth: 160  },
      { prop: 'startTime', label: '开始时间', minWidth: 110   },
      { prop: 'endTime', label: '结束时间', minWidth: 110   },
      { prop: 'crossDay', label: '是否跨天', minWidth: 110 , enums: { 0:'否',1:'跨天' } },
      { prop: 'restMinutes', label: '休息分钟', minWidth: 110  },
      { prop: 'status', label: '状态', minWidth: 110 , enums: { 0:'停用',1:'启用' } }
    ],
    fields: [
      { prop: 'shiftNo', label: '班次编码', required: true  },
      { prop: 'shiftName', label: '班次名称', required: true  },
      { prop: 'startTime', label: '开始时间'   },
      { prop: 'endTime', label: '结束时间'   },
      { prop: 'crossDay', label: '是否跨天', type: 'number', precision: 0 , options: [ { label: '否', value: 0 }, { label: '跨天', value: 1 } ] },
      { prop: 'restMinutes', label: '休息分钟', type: 'number', precision: 0  },
      { prop: 'status', label: '状态', type: 'number', precision: 0 , options: [ { label: '停用', value: 0 }, { label: '启用', value: 1 } ] }
    ]
  },
  'schedule': {
    title: '排班表',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'shiftId', label: '班次', minWidth: 110  , ref: { resource: 'shift', field: 'shiftName' }  },
      { prop: 'workDate', label: '排班日期', minWidth: 110  },
      { prop: 'remark', label: '备注', minWidth: 160  }
    ],
    fields: [
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'shiftId', label: '班次', type: 'number'  , ref: { resource: 'shift', field: 'shiftName' }  },
      { prop: 'workDate', label: '排班日期'  },
      { prop: 'remark', label: '备注'  }
    ]
  },
  'attendance': {
    title: '打卡/考勤明细（结果传递至薪资核算）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'scheduleName', label: '排班', minWidth: 140  },
      { prop: 'workDate', label: '考勤日期', minWidth: 110   },
      { prop: 'punchIn', label: '上班打卡', minWidth: 110  },
      { prop: 'punchOut', label: '下班打卡', minWidth: 110  },
      { prop: 'lateMin', label: '迟到分钟', minWidth: 110  },
      { prop: 'earlyMin', label: '早退分钟', minWidth: 110  },
      { prop: 'absent', label: '是否缺勤', minWidth: 110 , enums: { 0:'否',1:'是' } },
      { prop: 'overtimeMin', label: '加班分钟', minWidth: 110  },
      { prop: 'status', label: '打卡状态', minWidth: 110 , enums: { 0:'正常',1:'迟到',2:'早退',3:'缺勤',4:'加班' } }
    ],
    fields: [
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'scheduleId', label: '排班', type: 'number'  , ref: { resource: 'schedule', field: 'scheduleName' } },
      { prop: 'workDate', label: '考勤日期'   },
      { prop: 'punchIn', label: '上班打卡'  },
      { prop: 'punchOut', label: '下班打卡'  },
      { prop: 'lateMin', label: '迟到分钟', type: 'number', precision: 0  },
      { prop: 'earlyMin', label: '早退分钟', type: 'number', precision: 0  },
      { prop: 'absent', label: '是否缺勤', type: 'number', precision: 0 , options: [ { label: '否', value: 0 }, { label: '是', value: 1 } ] },
      { prop: 'overtimeMin', label: '加班分钟', type: 'number', precision: 0  },
      { prop: 'status', label: '打卡状态', type: 'number', precision: 0 , options: [ { label: '正常', value: 0 }, { label: '迟到', value: 1 }, { label: '早退', value: 2 }, { label: '缺勤', value: 3 }, { label: '加班', value: 4 } ] }
    ]
  },
  'leave': {
    title: '请假单',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'leaveNo', label: '请假单号', minWidth: 160  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'leaveType', label: '请假类型', minWidth: 110 , enums: { 1:'年假',2:'事假',3:'病假',4:'调休',5:'婚假',6:'产假' } },
      { prop: 'startDate', label: '开始日期', minWidth: 110   },
      { prop: 'endDate', label: '结束日期', minWidth: 110   },
      { prop: 'days', label: '请假天数', minWidth: 110  },
      { prop: 'reason', label: '原因', minWidth: 160  },
      { prop: 'status', label: '审批状态', minWidth: 110 , enums: { 0:'草稿',1:'审批中',2:'通过',3:'驳回' } },
    ],
    fields: [
      { prop: 'leaveNo', label: '请假单号', required: true  },
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'leaveType', label: '请假类型', type: 'number', precision: 0 , options: [ { label: '年假', value: 1 }, { label: '事假', value: 2 }, { label: '病假', value: 3 }, { label: '调休', value: 4 }, { label: '婚假', value: 5 }, { label: '产假', value: 6 } ] },
      { prop: 'startDate', label: '开始日期'   },
      { prop: 'endDate', label: '结束日期'   },
      { prop: 'days', label: '请假天数', type: 'number', precision: 2  },
      { prop: 'reason', label: '原因'  },
      { prop: 'status', label: '审批状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '审批中', value: 1 }, { label: '通过', value: 2 }, { label: '驳回', value: 3 } ] },
    ]
  },
  'overtime': {
    title: '加班单',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'otNo', label: '加班单号', minWidth: 160  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'otDate', label: '加班日期', minWidth: 110  },
      { prop: 'hours', label: '加班小时', minWidth: 110  },
      { prop: 'reason', label: '原因', minWidth: 160  },
      { prop: 'status', label: '审批状态', minWidth: 110 , enums: { 0:'草稿',1:'审批中',2:'通过',3:'驳回' } },
    ],
    fields: [
      { prop: 'otNo', label: '加班单号', required: true  },
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'otDate', label: '加班日期'  },
      { prop: 'hours', label: '加班小时', type: 'number', precision: 2  },
      { prop: 'reason', label: '原因'  },
      { prop: 'status', label: '审批状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '审批中', value: 1 }, { label: '通过', value: 2 }, { label: '驳回', value: 3 } ] },
    ]
  },
  'salary-item': {
    title: '薪资项目（自定义）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'itemNo', label: '薪资项目编码', minWidth: 160  },
      { prop: 'itemName', label: '项目名称', minWidth: 160  },
      { prop: 'itemType', label: '项目类型', minWidth: 110 , enums: { 1:'应发',2:'扣减' } },
      { prop: 'calcType', label: '计算方式', minWidth: 110 , enums: { 1:'固定',2:'公式',3:'考勤',4:'绩效' } },
      { prop: 'taxFlag', label: '是否计税', minWidth: 110 , enums: { 0:'不计税',1:'计税' } },
    ],
    fields: [
      { prop: 'itemNo', label: '薪资项目编码', required: true  },
      { prop: 'itemName', label: '项目名称', required: true  },
      { prop: 'itemType', label: '项目类型', type: 'number', precision: 0 , options: [ { label: '应发', value: 1 }, { label: '扣减', value: 2 } ] },
      { prop: 'calcType', label: '计算方式', type: 'number', precision: 0 , options: [ { label: '固定', value: 1 }, { label: '公式', value: 2 }, { label: '考勤', value: 3 }, { label: '绩效', value: 4 } ] },
      { prop: 'taxFlag', label: '是否计税', type: 'number', precision: 0 , options: [ { label: '不计税', value: 0 }, { label: '计税', value: 1 } ] },
    ]
  },
  'employee-salary': {
    title: '员工薪资档案(定薪)',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'baseSalary', label: '核定基本工资', minWidth: 110  },
      { prop: 'effDate', label: '生效日期', minWidth: 110  },
      { prop: 'status', label: '档案状态', minWidth: 110 , enums: { 0:'历史',1:'生效' } }
    ],
    fields: [
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'baseSalary', label: '核定基本工资', type: 'number', precision: 2  },
      { prop: 'effDate', label: '生效日期'  },
      { prop: 'status', label: '档案状态', type: 'number', precision: 0 , options: [ { label: '历史', value: 0 }, { label: '生效', value: 1 } ] }
    ]
  },
  'salary': {
    title: '薪资核算主表',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'salaryNo', label: '工资单号', minWidth: 160  },
      { prop: 'period', label: '薪资期间', minWidth: 160  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'baseSalary', label: '基本工资', minWidth: 110   },
      { prop: 'attendDays', label: '出勤天数', minWidth: 110  },
      { prop: 'otPay', label: '加班费', minWidth: 110  },
      { prop: 'leaveDeduct', label: '请假扣款', minWidth: 110  },
      { prop: 'perfBonus', label: '绩效奖金', minWidth: 110  },
      { prop: 'grossPay', label: '应发合计', minWidth: 110  },
      { prop: 'socialPersonal', label: '个人社保', minWidth: 110  },
      { prop: 'fundPersonal', label: '个人公积金', minWidth: 110  },
      { prop: 'tax', label: '个税', minWidth: 110  },
      { prop: 'netPay', label: '实发合计', minWidth: 110  },
      { prop: 'status', label: '核算状态', minWidth: 110 , enums: { 0:'草稿',1:'已核算',2:'已发放',3:'已生成凭证' } }
    ],
    fields: [
      { prop: 'salaryNo', label: '工资单号', required: true  },
      { prop: 'period', label: '薪资期间', required: true  },
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'baseSalary', label: '基本工资', type: 'number', precision: 2   },
      { prop: 'attendDays', label: '出勤天数', type: 'number', precision: 2  },
      { prop: 'otPay', label: '加班费', type: 'number', precision: 2  },
      { prop: 'leaveDeduct', label: '请假扣款', type: 'number', precision: 2  },
      { prop: 'perfBonus', label: '绩效奖金', type: 'number', precision: 2  },
      { prop: 'grossPay', label: '应发合计', type: 'number', precision: 2  },
      { prop: 'socialPersonal', label: '个人社保', type: 'number', precision: 2  },
      { prop: 'fundPersonal', label: '个人公积金', type: 'number', precision: 2  },
      { prop: 'tax', label: '个税', type: 'number', precision: 2  },
      { prop: 'netPay', label: '实发合计', type: 'number', precision: 2  },
      { prop: 'status', label: '核算状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '已核算', value: 1 }, { label: '已发放', value: 2 }, { label: '已生成凭证', value: 3 } ] }
    ]
  },
  'salary-detail': {
    title: '薪资明细',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'salaryId', label: '工资单', minWidth: 110  , ref: { resource: 'salary', field: 'salaryNo' } },
      { prop: 'itemId', label: '薪资项目', minWidth: 110  , ref: { resource: 'salary-item', field: 'itemName' } },
      { prop: 'itemName', label: '项目名称', minWidth: 160   },
      { prop: 'amount', label: '金额', minWidth: 110  },
      { prop: 'direction', label: '收支方向', minWidth: 110 , enums: { 1:'收入','-1':'扣减' } }
    ],
    fields: [
      { prop: 'salaryId', label: '工资单', type: 'number'  , ref: { resource: 'salary', field: 'salaryNo' }  },
      { prop: 'itemId', label: '薪资项目', type: 'number'  , ref: { resource: 'salary-item', field: 'itemName' }  },
      { prop: 'itemName', label: '项目名称'   },
      { prop: 'amount', label: '金额', type: 'number', precision: 2  },
      { prop: 'direction', label: '收支方向', type: 'number', precision: 0 , options: [ { label: '收入', value: 1 }, { label: '扣减', value: -1 } ] }
    ]
  },
  'social-rule': {
    title: '社保/公积金缴费规则',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'city', label: '参保城市', minWidth: 160  },
      { prop: 'baseMin', label: '缴费基数下限', minWidth: 110  },
      { prop: 'baseMax', label: '缴费基数上限', minWidth: 110  },
      { prop: 'pensionEmpRate', label: '养老-个人比例', minWidth: 110  },
      { prop: 'pensionComRate', label: '养老-企业比例', minWidth: 110  },
      { prop: 'medicalEmpRate', label: '医疗-个人比例', minWidth: 110  },
      { prop: 'medicalComRate', label: '医疗-企业比例', minWidth: 110  },
      { prop: 'unemploymentEmpRate', label: '失业-个人比例', minWidth: 110  },
      { prop: 'unemploymentComRate', label: '失业-企业比例', minWidth: 110  },
      { prop: 'injuryComRate', label: '工伤-企业比例', minWidth: 110  },
      { prop: 'birthComRate', label: '生育-企业比例', minWidth: 110  },
      { prop: 'fundEmpRate', label: '公积金-个人比例', minWidth: 110  },
      { prop: 'fundComRate', label: '公积金-企业比例', minWidth: 110  }
    ],
    fields: [
      { prop: 'city', label: '参保城市', required: true  },
      { prop: 'baseMin', label: '缴费基数下限', type: 'number', precision: 2  },
      { prop: 'baseMax', label: '缴费基数上限', type: 'number', precision: 2  },
      { prop: 'pensionEmpRate', label: '养老-个人比例', type: 'number', precision: 2  },
      { prop: 'pensionComRate', label: '养老-企业比例', type: 'number', precision: 2  },
      { prop: 'medicalEmpRate', label: '医疗-个人比例', type: 'number', precision: 2  },
      { prop: 'medicalComRate', label: '医疗-企业比例', type: 'number', precision: 2  },
      { prop: 'unemploymentEmpRate', label: '失业-个人比例', type: 'number', precision: 2  },
      { prop: 'unemploymentComRate', label: '失业-企业比例', type: 'number', precision: 2  },
      { prop: 'injuryComRate', label: '工伤-企业比例', type: 'number', precision: 2  },
      { prop: 'birthComRate', label: '生育-企业比例', type: 'number', precision: 2  },
      { prop: 'fundEmpRate', label: '公积金-个人比例', type: 'number', precision: 2  },
      { prop: 'fundComRate', label: '公积金-企业比例', type: 'number', precision: 2  }
    ]
  },
  'tax-bracket': {
    title: '个税累进税率表(月度)',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'level', label: '级次', minWidth: 110  },
      { prop: 'thresholdLow', label: '应纳税所得额下限', minWidth: 110  },
      { prop: 'thresholdHigh', label: '所得额上限', minWidth: 110 , enums: { 0:'无上限' } },
      { prop: 'rate', label: '税率', minWidth: 110  },
      { prop: 'quickDeduction', label: '速算扣除数', minWidth: 110  }
    ],
    fields: [
      { prop: 'level', label: '级次', type: 'number', precision: 0  },
      { prop: 'thresholdLow', label: '应纳税所得额下限', type: 'number', precision: 2  },
      { prop: 'thresholdHigh', label: '所得额上限', type: 'number', precision: 2 , options: [ { label: '无上限', value: 0 } ] },
      { prop: 'rate', label: '税率', type: 'number', precision: 2  },
      { prop: 'quickDeduction', label: '速算扣除数', type: 'number', precision: 2  }
    ]
  },
  'welfare': {
    title: '员工福利',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'welfareType', label: '福利类型', minWidth: 160  },
      { prop: 'amount', label: '金额', minWidth: 110  },
      { prop: 'month', label: '所属月份', minWidth: 160  },
      { prop: 'remark', label: '备注', minWidth: 160  }
    ],
    fields: [
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'welfareType', label: '福利类型'  },
      { prop: 'amount', label: '金额', type: 'number', precision: 2  },
      { prop: 'month', label: '所属月份'  },
      { prop: 'remark', label: '备注'  }
    ]
  },
  'recruit-plan': {
    title: '人力需求/招聘计划',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'planNo', label: '招聘计划单号', minWidth: 160  },
      { prop: 'deptId', label: '需求部门', minWidth: 110  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'posId', label: '需求岗位', minWidth: 110  , ref: { resource: 'position', field: 'posName' } },
      { prop: 'needCount', label: '需求人数', minWidth: 110  },
      { prop: 'reason', label: '原因', minWidth: 160  },
      { prop: 'status', label: '审批状态', minWidth: 110 , enums: { 0:'草稿',1:'审批中',2:'通过',3:'关闭' } },
    ],
    fields: [
      { prop: 'planNo', label: '招聘计划单号', required: true  },
      { prop: 'deptId', label: '需求部门', type: 'number'  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'posId', label: '需求岗位', type: 'number'  , ref: { resource: 'position', field: 'posName' } },
      { prop: 'needCount', label: '需求人数', type: 'number', precision: 0  },
      { prop: 'reason', label: '原因'  },
      { prop: 'status', label: '审批状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '审批中', value: 1 }, { label: '通过', value: 2 }, { label: '关闭', value: 3 } ] },
    ]
  },
  'candidate': {
    title: '候选人/人才库',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'name', label: '姓名', minWidth: 160  },
      { prop: 'gender', label: '性别', minWidth: 110 , enums: { 0:'女',1:'男' } },
      { prop: 'phone', label: '联系电话', minWidth: 160  },
      { prop: 'email', label: '邮箱', minWidth: 160  },
      { prop: 'education', label: '学历', minWidth: 160  },
      { prop: 'source', label: '招聘来源', minWidth: 110 , enums: { 1:'内推',2:'招聘网站',3:'校招',4:'猎头' } },
      { prop: "resumeUrl", label: "简历链接", minWidth: 160   },
      { prop: 'posId', label: '应聘岗位', minWidth: 110  , ref: { resource: 'position', field: 'posName' } },
      { prop: 'status', label: '候选人状态', minWidth: 110 , enums: { 0:'初筛',1:'面试中',2:'已录用',3:'已入职',4:'淘汰',5:'人才库' } },
      { prop: 'rating', label: '评分', minWidth: 110  },
      { prop: 'talentPool', label: '是否入人才库', minWidth: 110 , enums: { 0:'否',1:'是' } }
    ],
    fields: [
      { prop: 'name', label: '姓名', required: true  },
      { prop: 'gender', label: '性别', type: 'number', precision: 0 , options: [ { label: '女', value: 0 }, { label: '男', value: 1 } ] },
      { prop: 'phone', label: '联系电话'  },
      { prop: 'email', label: '邮箱'  },
      { prop: 'education', label: '学历'  },
      { prop: 'source', label: '招聘来源', type: 'number', precision: 0 , options: [ { label: '内推', value: 1 }, { label: '招聘网站', value: 2 }, { label: '校招', value: 3 }, { label: '猎头', value: 4 } ] },
      { prop: "resumeUrl", label: "简历链接"   },
      { prop: 'posId', label: '应聘岗位', type: 'number'  , ref: { resource: 'position', field: 'posName' } },
      { prop: 'status', label: '候选人状态', type: 'number', precision: 0 , options: [ { label: '初筛', value: 0 }, { label: '面试中', value: 1 }, { label: '已录用', value: 2 }, { label: '已入职', value: 3 }, { label: '淘汰', value: 4 }, { label: '人才库', value: 5 } ] },
      { prop: 'rating', label: '评分', type: 'number', precision: 0  },
      { prop: 'talentPool', label: '是否入人才库', type: 'number', precision: 0 , options: [ { label: '否', value: 0 }, { label: '是', value: 1 } ] }
    ]
  },
  'interview': {
    title: '面试记录',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'candidateId', label: '候选人', minWidth: 110  , ref: { resource: 'candidate', field: 'name' }  },
      { prop: 'round', label: '面试轮次', minWidth: 110  },
      { prop: 'interviewDate', label: '面试日期', minWidth: 110  },
      { prop: 'interviewers', label: '面试官', minWidth: 160  },
      { prop: 'score', label: '综合评分', minWidth: 110  },
      { prop: 'result', label: '面试结果', minWidth: 110 , enums: { 1:'通过',2:'未通过' } }
    ],
    fields: [
      { prop: 'candidateId', label: '候选人', type: 'number'  , ref: { resource: 'candidate', field: 'name' }  },
      { prop: 'round', label: '面试轮次', type: 'number', precision: 0  },
      { prop: 'interviewDate', label: '面试日期'  },
      { prop: 'interviewers', label: '面试官'  },
      { prop: 'score', label: '综合评分', type: 'number', precision: 2  },
      { prop: 'result', label: '面试结果', type: 'number', precision: 0 , options: [ { label: '通过', value: 1 }, { label: '未通过', value: 2 } ] }
    ]
  },
  'offer': {
    title: '录用offer',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'candidateId', label: '候选人', minWidth: 110  , ref: { resource: 'candidate', field: 'name' } },
      { prop: 'offerNo', label: '录用单号', minWidth: 160  },
      { prop: 'salary', label: '录用薪资', minWidth: 110  },
      { prop: 'effDate', label: '期望入职日期', minWidth: 110  },
      { prop: 'status', label: '录用状态', minWidth: 110 , enums: { 0:'待确认',1:'已接受',2:'已拒绝' } }
    ],
    fields: [
      { prop: 'candidateId', label: '候选人', type: 'number'  , ref: { resource: 'candidate', field: 'name' }  },
      { prop: 'offerNo', label: '录用单号', required: true  },
      { prop: 'salary', label: '录用薪资', type: 'number', precision: 2  },
      { prop: 'effDate', label: '期望入职日期'  },
      { prop: 'status', label: '录用状态', type: 'number', precision: 0 , options: [ { label: '待确认', value: 0 }, { label: '已接受', value: 1 }, { label: '已拒绝', value: 2 } ] }
    ]
  },
  'perf-indicator': {
    title: '考核指标库',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'indNo', label: '指标编码', minWidth: 160  },
      { prop: 'indName', label: '指标名称', minWidth: 160  },
      { prop: 'category', label: '指标类别', minWidth: 110 , enums: { 1:'业绩',2:'能力',3:'态度' } },
      { prop: 'weight', label: '权重%', minWidth: 110  },
      { prop: 'target', label: '目标值', minWidth: 160  }
    ],
    fields: [
      { prop: 'indNo', label: '指标编码', required: true  },
      { prop: 'indName', label: '指标名称', required: true  },
      { prop: 'category', label: '指标类别', type: 'number', precision: 0 , options: [ { label: '业绩', value: 1 }, { label: '能力', value: 2 }, { label: '态度', value: 3 } ] },
      { prop: 'weight', label: '权重%', type: 'number', precision: 2  },
      { prop: 'target', label: '目标值'  }
    ]
  },
  'perf-plan': {
    title: '考核计划',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'planNo', label: '考核计划单号', minWidth: 160  },
      { prop: 'planName', label: '计划名称', minWidth: 160  },
      { prop: 'period', label: '考核期间', minWidth: 160  },
      { prop: 'startDate', label: '开始日期', minWidth: 110   },
      { prop: 'endDate', label: '结束日期', minWidth: 110   },
      { prop: 'status', label: '计划状态', minWidth: 110 , enums: { 0:'草稿',1:'进行中',2:'已完成' } }
    ],
    fields: [
      { prop: 'planNo', label: '考核计划单号', required: true  },
      { prop: 'planName', label: '计划名称', required: true  },
      { prop: 'period', label: '考核期间'  },
      { prop: 'startDate', label: '开始日期'   },
      { prop: 'endDate', label: '结束日期'   },
      { prop: 'status', label: '计划状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '进行中', value: 1 }, { label: '已完成', value: 2 } ] }
    ]
  },
  'perf-result': {
    title: '考核结果（应用至薪资/晋升）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'planId', label: '考核计划', minWidth: 110  , ref: { resource: 'perf-plan', field: 'planName' } },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'score', label: '综合得分', minWidth: 110  },
      { prop: 'level', label: '考核等级', minWidth: 110 , enums: { 1:'优秀',2:'良好',3:'合格',4:'待改进' } },
      { prop: 'conclusion', label: '考核结论', minWidth: 160  },
      { prop: 'applySalary', label: '应用于薪资', minWidth: 110 , enums: { 0:'否',1:'是' } },
      { prop: 'applyPromote', label: '应用于晋升', minWidth: 110 , enums: { 0:'否',1:'是' } }
    ],
    fields: [
      { prop: 'planId', label: '考核计划', type: 'number'  , ref: { resource: 'perf-plan', field: 'planName' }  },
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'score', label: '综合得分', type: 'number', precision: 2  },
      { prop: 'level', label: '考核等级', type: 'number', precision: 0 , options: [ { label: '优秀', value: 1 }, { label: '良好', value: 2 }, { label: '合格', value: 3 }, { label: '待改进', value: 4 } ] },
      { prop: 'conclusion', label: '考核结论'  },
      { prop: 'applySalary', label: '应用于薪资', type: 'number', precision: 0 , options: [ { label: '否', value: 0 }, { label: '是', value: 1 } ] },
      { prop: 'applyPromote', label: '应用于晋升', type: 'number', precision: 0 , options: [ { label: '否', value: 0 }, { label: '是', value: 1 } ] }
    ]
  },
  'perf-detail': {
    title: '考核明细（按指标）',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'indId', label: '指标', minWidth: 110  , ref: { resource: 'perf-indicator', field: 'indName' } },
      { prop: 'indName', label: '指标名称', minWidth: 160   },
      { prop: 'score', label: '指标得分', minWidth: 110  },
      { prop: 'weight', label: '权重%', minWidth: 110  }
    ],
    fields: [
      { prop: 'indId', label: '指标', type: 'number'  , ref: { resource: 'perf-indicator', field: 'indName' }  },
      { prop: 'indName', label: '指标名称'   },
      { prop: 'score', label: '指标得分', type: 'number', precision: 2  },
      { prop: 'weight', label: '权重%', type: 'number', precision: 2  }
    ]
  },
  'train-need': {
    title: '培训需求',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'needNo', label: '培训需求单号', minWidth: 160  },
      { prop: 'deptId', label: '提出部门', minWidth: 110  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'needDesc', label: '需求描述', minWidth: 160  },
      { prop: 'priority', label: '优先级', minWidth: 110 , enums: { 1:'高',2:'中',3:'低' } },
      { prop: 'status', label: '审批状态', minWidth: 110 , enums: { 0:'待审批',1:'采纳',2:'驳回' } }
    ],
    fields: [
      { prop: 'needNo', label: '培训需求单号', required: true  },
      { prop: 'deptId', label: '提出部门', type: 'number'  , ref: { resource: 'department', field: 'deptName' } },
      { prop: 'needDesc', label: '需求描述'  },
      { prop: 'priority', label: '优先级', type: 'number', precision: 0 , options: [ { label: '高', value: 1 }, { label: '中', value: 2 }, { label: '低', value: 3 } ] },
      { prop: 'status', label: '审批状态', type: 'number', precision: 0 , options: [ { label: '待审批', value: 0 }, { label: '采纳', value: 1 }, { label: '驳回', value: 2 } ] }
    ]
  },
  'train-plan': {
    title: '培训计划',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'planNo', label: '培训计划单号', minWidth: 160  },
      { prop: 'planName', label: '培训名称', minWidth: 160  },
      { prop: 'trainType', label: '培训类型', minWidth: 160  },
      { prop: 'startDate', label: '开始日期', minWidth: 110   },
      { prop: 'endDate', label: '结束日期', minWidth: 110   },
      { prop: 'trainer', label: '讲师', minWidth: 160  },
      { prop: 'budget', label: '培训预算', minWidth: 110  },
      { prop: 'status', label: '计划状态', minWidth: 110 , enums: { 0:'草稿',1:'执行中',2:'完成' } }
    ],
    fields: [
      { prop: 'planNo', label: '培训计划单号', required: true  },
      { prop: 'planName', label: '培训名称', required: true  },
      { prop: 'trainType', label: '培训类型'  },
      { prop: 'startDate', label: '开始日期'   },
      { prop: 'endDate', label: '结束日期'   },
      { prop: 'trainer', label: '讲师'  },
      { prop: 'budget', label: '培训预算', type: 'number', precision: 2  },
      { prop: 'status', label: '计划状态', type: 'number', precision: 0 , options: [ { label: '草稿', value: 0 }, { label: '执行中', value: 1 }, { label: '完成', value: 2 } ] }
    ]
  },
  'train-record': {
    title: '培训记录',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'planId', label: '培训计划', minWidth: 110  , ref: { resource: 'train-plan', field: 'planName' } },
      { prop: 'empId', label: '员工', minWidth: 110  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'hours', label: '培训课时', minWidth: 110  },
      { prop: 'score', label: '考核成绩', minWidth: 110  },
      { prop: 'certificate', label: '获得证书', minWidth: 160  }
    ],
    fields: [
      { prop: 'planId', label: '培训计划', type: 'number'  , ref: { resource: 'train-plan', field: 'planName' }  },
      { prop: 'empId', label: '员工', type: 'number'  , ref: { resource: 'employee', field: 'name' }  },
      { prop: 'hours', label: '培训课时', type: 'number', precision: 2  },
      { prop: 'score', label: '考核成绩', type: 'number', precision: 2  },
      { prop: 'certificate', label: '获得证书'  }
    ]
  },
  'train-expense': {
    title: '培训费用',
    columns: [
      { prop: 'id', label: '主键', minWidth: 110  },
      { prop: 'planId', label: '培训计划', minWidth: 110  , ref: { resource: 'train-plan', field: 'planName' } },
      { prop: 'expenseType', label: '费用类型', minWidth: 160  },
      { prop: 'amount', label: '金额', minWidth: 110  },
      { prop: 'occurDate', label: '发生日期', minWidth: 110  },
      { prop: 'remark', label: '备注', minWidth: 160  }
    ],
    fields: [
      { prop: 'planId', label: '培训计划', type: 'number'  , ref: { resource: 'train-plan', field: 'planName' }  },
      { prop: 'expenseType', label: '费用类型'  },
      { prop: 'amount', label: '金额', type: 'number', precision: 2  },
      { prop: 'occurDate', label: '发生日期'  },
      { prop: 'remark', label: '备注'  }
    ]
  },
}
