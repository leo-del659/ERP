// 后台菜单配置：与 .NET 后端的 Permission_ key、/api 路由保持一致
// perm 字段对应后端 Permission_Filter 校验的 key；为空表示登录即可访问
// 排序原则：总览(看板/报表) → 主数据 → 业务主线(销/采/存/产/制造/财/人) → 系统支撑(日志/审批)
export interface MenuItem {
  title: string
  icon?: string
  path?: string
  perm?: string
  children?: MenuItem[]
}

export const menus: MenuItem[] = [
  {
    title: '决策看板',
    icon: 'DataLine',
    path: '/dashboard'
  },
  {
    title: '决策报表',
    icon: 'TrendCharts',
    path: '/rpt',
    perm: '/rpt'
  },
  {
    title: '主数据',
    icon: 'Files',
    children: [
      { title: '物料', path: '/md/material', perm: '/md' },
      { title: '客户', path: '/md/customer', perm: '/md' },
      { title: '供应商', path: '/md/supplier', perm: '/md' },
      { title: '仓库', path: '/md/warehouse', perm: '/md' }
    ]
  },
  {
    title: '销售管理',
    icon: 'Sell',
    perm: '/so',
    children: [
      { title: '销售订单', path: '/so' }
    ]
  },
  {
    title: '采购管理',
    icon: 'ShoppingCart',
    perm: '/po',
    children: [
      { title: '采购订单', path: '/po' }
    ]
  },
  {
    title: '库存管理',
    icon: 'Box',
    perm: '/inv',
    children: [
      { title: '库存台账', path: '/inv/stock' },
      { title: '库存流水', path: '/inv/trans' },
      { title: '库存预警', path: '/inv/alerts' },
      { title: '出入库/调拨', path: '/inv/inout' },
      { title: '库存盘点', path: '/inv/check' }
    ]
  },
  {
    title: '生产管理',
    icon: 'Cpu',
    perm: '/pr',
    children: [
      { title: '生产订单', path: '/pr' }
    ]
  },
  {
    title: '生产制造',
    icon: 'Box',
    children: [
      { title: '工单管理', path: '/mes/work-order', perm: '/mes/work-order' },
      { title: '工位管理', path: '/mes/workstation', perm: '/mes/workstation' },
      { title: '生产任务', path: '/mes/production-order', perm: '/mes/production-order' },
      { title: '领料管理', path: '/mes/material-issue', perm: '/mes/material-issue' },
      { title: '报工管理', path: '/mes/work-report', perm: '/mes/work-report' },
      { title: '工艺路线', path: '/mes/routing', perm: '/mes/routing' },
      { title: '工艺工序', path: '/mes/routing-step', perm: '/mes/routing-step' },
      { title: '质量检验', path: '/mes/quality-check', perm: '/mes/quality-check' }
    ]
  },
  {
    title: '财务管理',
    icon: 'Money',
    perm: '/fin',
    children: [
      { title: '总账/应收应付/成本', path: '/fin' }
    ]
  },
  {
    title: '人事资源',
    icon: 'User',
    children: [
      {
        title: '组织与岗位',
        icon: 'OfficeBuilding',
        children: [
          { title: '部门管理', path: '/hr/department', perm: '/hr/department' },
          { title: '岗位体系', path: '/hr/position', perm: '/hr/position' },
          { title: '岗位编制', path: '/hr/position-quota', perm: '/hr/position-quota' }
        ]
      },
      {
        title: '员工管理',
        icon: 'User',
        children: [
          { title: '员工档案', path: '/hr/employee', perm: '/hr/employee' },
          { title: '异动轨迹', path: '/hr/employee-change', perm: '/hr/employee-change' }
        ]
      },
      {
        title: '合同管理',
        icon: 'Files',
        children: [{ title: '劳动合同', path: '/hr/contract', perm: '/hr/contract' }]
      },
      {
        title: '考勤与休假',
        icon: 'Calendar',
        children: [
          { title: '排班班次', path: '/hr/shift', perm: '/hr/shift' },
          { title: '排班计划', path: '/hr/schedule', perm: '/hr/schedule' },
          { title: '考勤打卡', path: '/hr/attendance', perm: '/hr/attendance' },
          { title: '请假管理', path: '/hr/leave', perm: '/hr/leave' },
          { title: '加班管理', path: '/hr/overtime', perm: '/hr/overtime' }
        ]
      },
      {
        title: '薪资与福利',
        icon: 'Money',
        children: [
          { title: '薪资项目', path: '/hr/salary-item', perm: '/hr/salary-item' },
          { title: '薪资档案', path: '/hr/employee-salary', perm: '/hr/employee-salary' },
          { title: '薪资核算', path: '/hr/salary', perm: '/hr/salary' },
          { title: '薪资明细', path: '/hr/salary-detail', perm: '/hr/salary-detail' },
          { title: '社保规则', path: '/hr/social-rule', perm: '/hr/social-rule' },
          { title: '个税税率', path: '/hr/tax-bracket', perm: '/hr/tax-bracket' },
          { title: '员工福利', path: '/hr/welfare', perm: '/hr/welfare' }
        ]
      },
      {
        title: '招聘与人才',
        icon: 'Avatar',
        children: [
          { title: '招聘需求', path: '/hr/recruit-plan', perm: '/hr/recruit-plan' },
          { title: '候选人库', path: '/hr/candidate', perm: '/hr/candidate' },
          { title: '面试记录', path: '/hr/interview', perm: '/hr/interview' },
          { title: '录用 Offer', path: '/hr/offer', perm: '/hr/offer' }
        ]
      },
      {
        title: '绩效与培训',
        icon: 'TrendCharts',
        children: [
          { title: '考核指标', path: '/hr/perf-indicator', perm: '/hr/perf-indicator' },
          { title: '考核计划', path: '/hr/perf-plan', perm: '/hr/perf-plan' },
          { title: '考核结果', path: '/hr/perf-result', perm: '/hr/perf-result' },
          { title: '考核明细', path: '/hr/perf-detail', perm: '/hr/perf-detail' },
          { title: '培训需求', path: '/hr/train-need', perm: '/hr/train-need' },
          { title: '培训计划', path: '/hr/train-plan', perm: '/hr/train-plan' },
          { title: '培训记录', path: '/hr/train-record', perm: '/hr/train-record' },
          { title: '培训费用', path: '/hr/train-expense', perm: '/hr/train-expense' }
        ]
      }
    ]
  },
  {
    title: '系统日志',
    icon: 'Document',
    children: [
      { title: '操作日志', path: '/log/oper' },
      { title: '异常日志', path: '/log/ex' }
    ]
  },
  {
    title: '审批流',
    icon: 'Stamp',
    children: [
      { title: '审批中心', path: '/approval', perm: '/sys/approval' }
    ]
  }
]
