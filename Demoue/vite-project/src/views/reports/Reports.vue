<template>
  <div class="page">
    <el-tabs v-model="tab" @tab-change="onTabChange">
      <el-tab-pane label="库存周转" name="turnover">
        <el-form :inline="true" class="filter">
          <el-form-item label="统计周期(天)"><el-input-number v-model="days" :min="7" :max="365" controls-position="right" @change="loadTurnover" /></el-form-item>
          <el-button type="primary" @click="loadTurnover">查询</el-button>
        </el-form>
        <el-row :gutter="16" v-if="turnover.summary">
          <el-col :span="6"><div class="stat"><div class="l">物料数</div><div class="v">{{ turnover.summary.materialCount }}</div></div></el-col>
          <el-col :span="6"><div class="stat"><div class="l">库存金额</div><div class="v">{{ money(turnover.summary.stockValue) }}</div></div></el-col>
          <el-col :span="6"><div class="stat"><div class="l">出库金额</div><div class="v">{{ money(turnover.summary.outValue) }}</div></div></el-col>
          <el-col :span="6"><div class="stat"><div class="l">呆滞料数</div><div class="v danger">{{ turnover.summary.slowMovingCount }}</div></div></el-col>
        </el-row>
        <div ref="chartTurnoverRef" style="height:320px;margin-top:12px"></div>
        <el-table :data="turnover.rows" border size="small" style="margin-top:12px" max-height="360">
          <el-table-column prop="materialCode" label="编码" width="130" />
          <el-table-column prop="materialName" label="名称" min-width="160" />
          <el-table-column prop="currentQty" label="结存" width="100" align="right" />
          <el-table-column prop="outQty" label="期间出库" width="100" align="right" />
          <el-table-column prop="turnoverRate" label="周转率" width="100" align="right" />
          <el-table-column prop="turnoverDays" label="周转天数" width="100" align="right" />
          <el-table-column prop="isSlowMoving" label="呆滞" width="80" align="center"><template #default="{ row }"><el-tag size="small" :type="row.isSlowMoving ? 'danger' : 'success'">{{ row.isSlowMoving ? '是' : '否' }}</el-tag></template></el-table-column>
        </el-table>
      </el-tab-pane>

      <el-tab-pane label="生产进度" name="production">
        <el-row :gutter="16" v-if="production.summary">
          <el-col :span="4"><div class="stat"><div class="l">工单数</div><div class="v">{{ production.summary.totalOrders }}</div></div></el-col>
          <el-col :span="4"><div class="stat"><div class="l">生产中</div><div class="v warning">{{ production.summary.producing }}</div></div></el-col>
          <el-col :span="4"><div class="stat"><div class="l">已完工</div><div class="v success">{{ production.summary.done }}</div></div></el-col>
          <el-col :span="4"><div class="stat"><div class="l">平均完成率</div><div class="v">{{ production.summary.avgProgress }}%</div></div></el-col>
          <el-col :span="4"><div class="stat"><div class="l">平均不良率</div><div class="v danger">{{ production.summary.avgDefectRate }}%</div></div></el-col>
          <el-col :span="4"><div class="stat"><div class="l">累计工时</div><div class="v">{{ production.summary.totalWorkHours }}</div></div></el-col>
        </el-row>
        <div ref="chartProductionRef" style="height:320px;margin-top:12px"></div>
        <el-table :data="production.rows" border size="small" style="margin-top:12px" max-height="360">
          <el-table-column prop="orderNo" label="单号" width="150" />
          <el-table-column prop="materialName" label="产品" min-width="150" />
          <el-table-column prop="planQty" label="计划量" width="90" align="right" />
          <el-table-column prop="goodQty" label="良品" width="90" align="right" />
          <el-table-column prop="badQty" label="不良" width="90" align="right" />
          <el-table-column prop="progressRate" label="完成率%" width="100" align="right" />
          <el-table-column prop="defectRate" label="不良率%" width="100" align="right" />
        </el-table>
      </el-tab-pane>

      <el-tab-pane label="毛利分析" name="margin">
        <el-form :inline="true" class="filter">
          <el-form-item label="起始"><el-date-picker v-model="marginFrom" type="date" value-format="YYYY-MM-DD" /></el-form-item>
          <el-form-item label="截止"><el-date-picker v-model="marginTo" type="date" value-format="YYYY-MM-DD" /></el-form-item>
          <el-button type="primary" @click="loadMargin">查询</el-button>
        </el-form>
        <el-row :gutter="16" v-if="margin.summary">
          <el-col :span="6"><div class="stat"><div class="l">营收</div><div class="v">{{ money(margin.summary.totalRevenue) }}</div></div></el-col>
          <el-col :span="6"><div class="stat"><div class="l">成本</div><div class="v warning">{{ money(margin.summary.totalCost) }}</div></div></el-col>
          <el-col :span="6"><div class="stat"><div class="l">毛利</div><div class="v success">{{ money(margin.summary.totalGross) }}</div></div></el-col>
          <el-col :span="6"><div class="stat"><div class="l">毛利率</div><div class="v">{{ margin.summary.grossRate }}%</div></div></el-col>
        </el-row>
        <div ref="chartMarginRef" style="height:320px;margin-top:12px"></div>
        <el-table :data="margin.rows" border size="small" style="margin-top:12px" max-height="360">
          <el-table-column prop="materialCode" label="编码" width="130" />
          <el-table-column prop="materialName" label="名称" min-width="160" />
          <el-table-column prop="revenue" label="收入" width="120" align="right" />
          <el-table-column prop="cost" label="成本" width="120" align="right" />
          <el-table-column prop="gross" label="毛利" width="120" align="right" />
          <el-table-column prop="grossRate" label="毛利率%" width="100" align="right" />
        </el-table>
      </el-tab-pane>

      <el-tab-pane label="销售趋势" name="trend">
        <el-form :inline="true" class="filter">
          <el-form-item label="月数"><el-input-number v-model="months" :min="3" :max="24" controls-position="right" @change="loadTrend" /></el-form-item>
          <el-button type="primary" @click="loadTrend">查询</el-button>
        </el-form>
        <div ref="chartTrendRef" style="height:360px;margin-top:12px"></div>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script setup lang="ts">
import { nextTick, onMounted, onBeforeUnmount, reactive, ref, watch } from 'vue'
import * as echarts from 'echarts/core'
import { BarChart, LineChart, PieChart } from 'echarts/charts'
import { TooltipComponent, LegendComponent, GridComponent, TitleComponent } from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import { get } from '@/utils/request'

echarts.use([BarChart, LineChart, PieChart, TooltipComponent, LegendComponent, GridComponent, TitleComponent, CanvasRenderer])

const tab = ref('turnover')
const days = ref(90)
const months = ref(12)
const marginFrom = ref('')
const marginTo = ref('')

const turnover = reactive<any>({ rows: [], summary: null })
const production = reactive<any>({ rows: [], summary: null })
const margin = reactive<any>({ rows: [], summary: null })
const trend = ref<any[]>([])

const chartTurnoverRef = ref<HTMLElement>()
const chartProductionRef = ref<HTMLElement>()
const chartMarginRef = ref<HTMLElement>()
const chartTrendRef = ref<HTMLElement>()
const chartRefs: Record<string, any> = {
  turnover: chartTurnoverRef,
  production: chartProductionRef,
  margin: chartMarginRef,
  trend: chartTrendRef
}
const charts: Record<string, echarts.ECharts> = {}

function getChart(name: string): echarts.ECharts | null {
  const el = chartRefs[name]?.value
  if (!el) return null
  if (!charts[name]) charts[name] = echarts.init(el)
  return charts[name]
}

const money = (v: number) => `¥ ${Number(v || 0).toLocaleString('zh-CN', { minimumFractionDigits: 2 })}`

async function loadTurnover() { try { const res: any = await get('/api/rpt/turnover', { days: days.value }); if (res) { turnover.rows = res.rows || []; turnover.summary = res.summary } } catch {} renderChart() }
async function loadProduction() { try { const res: any = await get('/api/rpt/production'); if (res) { production.rows = res.rows || []; production.summary = res.summary } } catch {} renderChart() }
async function loadMargin() { try { const res: any = await get('/api/rpt/margin', { from: marginFrom.value, to: marginTo.value }); if (res) { margin.rows = res.rows || []; margin.summary = res.summary } } catch {} renderChart() }
async function loadTrend() { try { trend.value = (await get('/api/rpt/trend', { months: months.value })) || [] } catch {} renderChart() }

const loadedTabs: Record<string, boolean> = { turnover: true }
async function onTabChange(name: any) {
  const key = String(name)
  if (loadedTabs[key]) { renderChart(); return }
  loadedTabs[key] = true
  if (key === 'production') await loadProduction()
  else if (key === 'margin') await loadMargin()
  else if (key === 'trend') await loadTrend()
}

const titleOption = (len: number) => ({
  title: { text: len ? '' : '暂无数据', left: 'center', top: 'middle', textStyle: { color: '#909399', fontSize: 14, fontWeight: 400 } }
})

function renderChart() {
  nextTick(() => {
    const chart = getChart(tab.value)
    if (!chart) return
    if (tab.value === 'turnover') {
      const top = (turnover.rows || []).slice(0, 10)
      chart.setOption({
        ...titleOption(top.length),
        tooltip: { trigger: 'axis' },
        grid: { left: 50, right: 20, bottom: 60 },
        xAxis: { type: 'category', data: top.map((r: any) => r.materialName), axisLabel: { rotate: 30 } },
        yAxis: { type: 'value' },
        series: [{ type: 'bar', name: '出库金额', data: top.map((r: any) => r.outValue), itemStyle: { color: '#409eff' } }]
      })
    } else if (tab.value === 'production') {
      const top = (production.rows || []).slice(0, 10)
      chart.setOption({
        ...titleOption(top.length),
        tooltip: { trigger: 'axis' },
        legend: { data: ['计划量', '良品量'], bottom: 0 },
        grid: { left: 50, right: 20, bottom: 60 },
        xAxis: { type: 'category', data: top.map((r: any) => r.orderNo), axisLabel: { rotate: 30 } },
        yAxis: { type: 'value' },
        series: [
          { type: 'bar', name: '计划量', data: top.map((r: any) => r.planQty), itemStyle: { color: '#909399' } },
          { type: 'bar', name: '良品量', data: top.map((r: any) => r.goodQty), itemStyle: { color: '#67c23a' } }
        ]
      })
    } else if (tab.value === 'margin') {
      const top = (margin.rows || []).slice(0, 10)
      chart.setOption({
        ...titleOption(top.length),
        tooltip: { trigger: 'axis' },
        legend: { data: ['收入', '成本', '毛利'], bottom: 0 },
        grid: { left: 60, right: 20, bottom: 60 },
        xAxis: { type: 'category', data: top.map((r: any) => r.materialName), axisLabel: { rotate: 30 } },
        yAxis: { type: 'value' },
        series: [
          { type: 'bar', name: '收入', data: top.map((r: any) => r.revenue), itemStyle: { color: '#409eff' } },
          { type: 'bar', name: '成本', data: top.map((r: any) => r.cost), itemStyle: { color: '#e6a23c' } },
          { type: 'bar', name: '毛利', data: top.map((r: any) => r.gross), itemStyle: { color: '#67c23a' } }
        ]
      })
    } else if (tab.value === 'trend') {
      const d = trend.value || []
      chart.setOption({
        ...titleOption(d.length),
        tooltip: { trigger: 'axis' },
        legend: { data: ['营收', '成本', '毛利'], bottom: 0 },
        grid: { left: 60, right: 20, bottom: 40 },
        xAxis: { type: 'category', data: d.map((r: any) => r.period) },
        yAxis: { type: 'value' },
        series: [
          { type: 'line', name: '营收', data: d.map((r: any) => r.revenue), smooth: true },
          { type: 'line', name: '成本', data: d.map((r: any) => r.cost), smooth: true },
          { type: 'line', name: '毛利', data: d.map((r: any) => r.gross), smooth: true }
        ]
      })
    }
    chart.resize()
  })
}

function onResize() { Object.values(charts).forEach(c => c.resize()) }
watch(tab, renderChart)

onMounted(async () => {
  window.addEventListener('resize', onResize)
  await loadTurnover()
})
onBeforeUnmount(() => {
  window.removeEventListener('resize', onResize)
  Object.values(charts).forEach(c => c.dispose())
})
</script>

<style scoped>
.filter { margin-bottom: 8px; }
.stat { background: #fff; border: 1px solid #ebeef5; border-radius: 8px; padding: 14px 16px; }
.stat .l { color: #909399; font-size: 13px; }
.stat .v { font-size: 22px; font-weight: 600; margin-top: 6px; color: #303133; }
.stat .v.danger { color: #f56c6c; }
.stat .v.warning { color: #e6a23c; }
.stat .v.success { color: #67c23a; }
</style>
