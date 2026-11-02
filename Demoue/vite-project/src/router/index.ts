import { createRouter, createWebHistory } from 'vue-router'
import { getToken } from '@/utils/request'
import AdminLayout from '@/layout/AdminLayout.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/login/index.vue'),
      meta: { public: true }
    },
    {
      path: '/',
      component: AdminLayout,
      redirect: '/dashboard',
      children: [
        { path: 'dashboard', name: 'dashboard', component: () => import('@/views/dashboard/index.vue'), meta: { title: '决策看板' } },
        // 库存
        { path: 'inv/stock', name: 'inv-stock', component: () => import('@/views/inventory/StockList.vue'), meta: { title: '库存台账' } },
        { path: 'inv/trans', name: 'inv-trans', component: () => import('@/views/inventory/TransList.vue'), meta: { title: '库存流水' } },
        { path: 'inv/alerts', name: 'inv-alerts', component: () => import('@/views/inventory/Alerts.vue'), meta: { title: '库存预警' } },
        { path: 'inv/inout', name: 'inv-inout', component: () => import('@/views/inventory/InOut.vue'), meta: { title: '出入库/调拨' } },
        { path: 'inv/check', name: 'inv-check', component: () => import('@/views/inventory/Check.vue'), meta: { title: '库存盘点' } },
        // 主数据
        { path: 'md/material', name: 'md-material', component: () => import('@/views/masterdata/Material.vue'), meta: { title: '物料主数据' } },
        { path: 'md/customer', name: 'md-customer', component: () => import('@/views/masterdata/Customer.vue'), meta: { title: '客户主数据' } },
        { path: 'md/supplier', name: 'md-supplier', component: () => import('@/views/masterdata/Supplier.vue'), meta: { title: '供应商主数据' } },
        { path: 'md/warehouse', name: 'md-warehouse', component: () => import('@/views/masterdata/Warehouse.vue'), meta: { title: '仓库主数据' } },
        // 日志
        { path: 'log/oper', name: 'log-oper', component: () => import('@/views/logs/OperLog.vue'), meta: { title: '操作日志' } },
        { path: 'log/ex', name: 'log-ex', component: () => import('@/views/logs/ExLog.vue'), meta: { title: '异常日志' } },
        // 业务单据
        { path: 'so', name: 'so', component: () => import('@/views/sales/OrderList.vue'), meta: { title: '销售订单' } },
        { path: 'po', name: 'po', component: () => import('@/views/purchase/OrderList.vue'), meta: { title: '采购订单' } },
        { path: 'pr', name: 'pr', component: () => import('@/views/production/OrderList.vue'), meta: { title: '生产订单' } },
        { path: 'fin', name: 'fin', component: () => import('@/views/finance/Finance.vue'), meta: { title: '财务中心' } },
        // 审批与决策
        { path: 'approval', name: 'approval', component: () => import('@/views/approval/Approval.vue'), meta: { title: '审批流' } },
        { path: 'rpt', name: 'rpt', component: () => import('@/views/reports/Reports.vue'), meta: { title: '决策报表' } },
        // 人事模块（动态资源页：/hr/<resource>）
        { path: 'hr/:resource', name: 'hr-resource', component: () => import('@/views/hr/HrPage.vue'), meta: { title: '人事管理' } },
        // 生产制造模块（动态资源页：/mes/<resource>）
        { path: 'mes/:resource', name: 'mes-resource', component: () => import('@/views/mes/MesPage.vue'), meta: { title: '生产制造' } }
      ]
    }
  ]
})

router.beforeEach((to) => {
  const token = getToken()
  if (to.meta.public) return true
  if (!token) return '/login'
  return true
})

export default router
