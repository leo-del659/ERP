<template>
  <div>
    <el-row :gutter="16">
      <el-col v-for="c in cards" :key="c.key" :xs="12" :sm="12" :md="6" class="card-col">
        <div class="stat-card" :style="{ '--accent': c.color }">
          <div class="stat-icon">
            <el-icon><component :is="c.icon" /></el-icon>
          </div>
          <div class="stat-body">
            <div class="label">{{ c.label }}</div>
            <div class="value">{{ c.value }}</div>
          </div>
        </div>
      </el-col>
    </el-row>

    <el-row :gutter="16" class="mt">
      <el-col :xs="24" :md="12">
        <div class="page-card">
          <div class="toolbar"><div class="left"><b>销售订单状态分布</b></div></div>
          <div ref="chartRef" style="height: 320px"></div>
        </div>
      </el-col>
      <el-col :xs="24" :md="12">
        <div class="page-card">
          <div class="toolbar"><div class="left"><b>关键指标</b></div></div>
          <el-table :data="rows" size="small" border>
            <el-table-column prop="name" label="指标" />
            <el-table-column prop="value" label="数值" align="right" />
          </el-table>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onBeforeUnmount, ref } from 'vue'
import * as echarts from 'echarts/core'
import { PieChart } from 'echarts/charts'
import { TooltipComponent, LegendComponent } from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import { get } from '@/utils/request'

echarts.use([PieChart, TooltipComponent, LegendComponent, CanvasRenderer])

interface Overview {
  soAmount: number
  poAmount: number
  arOpen: number
  apOpen: number
  stockSku: number
  alerts: number
  prOpen: number
  gross: number
  soByStatus: { status: number; count: number }[]
}

const data = ref<Overview>({
  soAmount: 0,
  poAmount: 0,
  arOpen: 0,
  apOpen: 0,
  stockSku: 0,
  alerts: 0,
  prOpen: 0,
  gross: 0,
  soByStatus: []
})

const chartRef = ref<HTMLElement>()
let chart: echarts.ECharts | null = null

const money = (v: number) => `¥ ${Number(v || 0).toLocaleString('zh-CN', { minimumFractionDigits: 2 })}`

const cards = computed(() => [
  { key: 'so', label: '销售订单总额', value: money(data.value.soAmount), color: '#e6a23c', icon: 'Sell' },
  { key: 'po', label: '采购订单总额', value: money(data.value.poAmount), color: '#409eff', icon: 'ShoppingCart' },
  { key: 'ar', label: '应收未收余额', value: money(data.value.arOpen), color: '#f56c6c', icon: 'Wallet' },
  { key: 'ap', label: '应付未付余额', value: money(data.value.apOpen), color: '#909399', icon: 'CreditCard' },
  { key: 'stock', label: '库存 SKU 数', value: data.value.stockSku, color: '#303133', icon: 'Box' },
  { key: 'alert', label: '库存预警数', value: data.value.alerts, color: '#f56c6c', icon: 'Warning' },
  { key: 'pr', label: '在制生产工单', value: data.value.prOpen, color: '#67c23a', icon: 'Cpu' },
  { key: 'gross', label: '毛利(销-购)', value: money(data.value.gross), color: '#67c23a', icon: 'TrendCharts' }
])

const rows = computed(() => [
  { name: '销售订单总额', value: money(data.value.soAmount) },
  { name: '采购订单总额', value: money(data.value.poAmount) },
  { name: '应收未收余额', value: money(data.value.arOpen) },
  { name: '应付未付余额', value: money(data.value.apOpen) },
  { name: '毛利(销-购)', value: money(data.value.gross) },
  { name: '在制生产工单', value: data.value.prOpen },
  { name: '库存预警物料数', value: data.value.alerts }
])

const STATUS_TEXT: Record<number, string> = { 0: '新建', 1: '已确认', 2: '已发货', 3: '已完成' }

function renderChart() {
  if (!chartRef.value) return
  chart = echarts.init(chartRef.value)
  const items = data.value.soByStatus || []
  chart.setOption({
    tooltip: { trigger: 'item' },
    legend: { bottom: 0 },
    series: [
      {
        type: 'pie',
        radius: ['40%', '65%'],
        label: { formatter: '{b}: {c}' },
        data: items.map((i) => ({ name: STATUS_TEXT[i.status] ?? `状态${i.status}`, value: i.count }))
      }
    ]
  })
}

function onResize() {
  chart?.resize()
}

onMounted(async () => {
  try {
    const res = await get<Overview>('/api/dashboard')
    if (res) data.value = { ...data.value, ...res }
  } catch {
  }
  renderChart()
  window.addEventListener('resize', onResize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', onResize)
  chart?.dispose()
})
</script>

<style scoped>
.card-col {
  margin-bottom: 16px;
}
.mt {
  margin-top: 4px;
}

.stat-card {
  display: flex;
  align-items: center;
  gap: 14px;
  background: #fff;
  border-radius: 10px;
  padding: 16px 18px;
  border-left: 4px solid var(--accent, #2f54eb);
  box-shadow: var(--erp-shadow-sm);
  border-top: 1px solid #eef0f4;
  border-right: 1px solid #eef0f4;
  border-bottom: 1px solid #eef0f4;
  transition: transform 0.18s ease, box-shadow 0.18s ease;
}
.stat-card:hover {
  transform: translateY(-3px);
  box-shadow: var(--erp-shadow-md);
}
.stat-icon {
  flex: 0 0 46px;
  width: 46px;
  height: 46px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 22px;
  color: var(--accent, #2f54eb);
  background: color-mix(in srgb, var(--accent, #2f54eb) 14%, #fff);
}
.stat-body {
  min-width: 0;
}
.stat-card .label {
  color: #909399;
  font-size: 13px;
  margin-bottom: 4px;
}
.stat-card .value {
  font-size: 22px;
  font-weight: 700;
  color: #303133;
  line-height: 1.2;
}
</style>
