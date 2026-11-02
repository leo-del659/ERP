<template>
  <div class="page">
    <el-tabs v-model="tab">
      <el-tab-pane label="生产订单" name="pr">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="pr.keyword" placeholder="生产单号" clearable style="width: 200px" @keyup.enter="prSearch" />
            <el-button type="primary" @click="prSearch">查询</el-button>
            <el-button @click="prReset">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openPrCreate">新建生产订单</el-button>
            <el-button @click="prLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="pr.rows" v-loading="pr.loading" border stripe size="small">
          <el-table-column prop="orderNo" label="单号" width="160" />
          <el-table-column prop="materialName" label="产品" min-width="160" show-overflow-tooltip />
          <el-table-column prop="planQty" label="计划量" width="100" align="right" />
          <el-table-column prop="completedQty" label="完工量" width="100" align="right" />
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }"><el-tag size="small" :type="prStatus(row.status).t">{{ prStatus(row.status).l }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="320" fixed="right">
            <template #default="{ row }">
              <el-button link type="primary" :disabled="row.status === 0" @click="openIssue(row)">领料</el-button>
              <el-button link type="warning" @click="openReport(row)">报工</el-button>
              <el-button link type="success" :disabled="row.status === 0" @click="releasePr(row)">下达</el-button>
              <el-button link type="danger" :disabled="row.status !== 3" @click="openComplete(row)">完工入库</el-button>
            </template>
          </el-table-column>
        </el-table>
        <ErpPagination :total="pr.total" v-model:page="pr.page" v-model:size="pr.size" @change="prLoad" @size-change="prSearch" />
      </el-tab-pane>

      <el-tab-pane label="主生产计划(MPS)" name="mps">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="mps.keyword" placeholder="计划单号/物料" clearable style="width: 220px" @keyup.enter="mpsSearch" />
            <el-button type="primary" @click="mpsSearch">查询</el-button>
            <el-button @click="mpsReset">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openMpsCreate">新建MPS</el-button>
            <el-button @click="mpsLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="mps.rows" v-loading="mps.loading" border stripe size="small">
          <el-table-column prop="mpsNo" label="单号" width="160" />
          <el-table-column prop="materialName" label="产品" min-width="160" show-overflow-tooltip />
          <el-table-column prop="planQty" label="计划量" width="100" align="right" />
          <el-table-column prop="planStartDate" label="起始" width="120" />
          <el-table-column prop="planEndDate" label="截止" width="120" />
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.status === 0 ? 'info' : row.status === 1 ? '' : 'success'">{{ row.status===0?'草稿':row.status===1?'已确认':'已转单' }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="220" fixed="right">
            <template #default="{ row }">
              <el-button link type="success" :disabled="row.status !== 0" @click="confirmMps(row)">确认</el-button>
              <el-button link type="primary" :disabled="row.status !== 1" @click="mpsToOrder(row)">转工单</el-button>
            </template>
          </el-table-column>
        </el-table>
        <ErpPagination :total="mps.total" v-model:page="mps.page" v-model:size="mps.size" @change="mpsLoad" @size-change="mpsSearch" />
      </el-tab-pane>

      <el-tab-pane label="MRP 运算" name="mrp">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="mrp.keyword" placeholder="MRP单号/物料" clearable style="width: 220px" @keyup.enter="mrpLoad" />
            <el-button type="primary" @click="mrpLoad">查询</el-button>
            <el-button @click="mrp.keyword=''; mrpLoad()">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" :loading="running" @click="runMrp">运行MRP</el-button>
            <el-button @click="mrpLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="mrp.rows" v-loading="mrp.loading" border stripe size="small">
          <el-table-column prop="mrpNo" label="MRP号" width="160" />
          <el-table-column prop="materialName" label="物料" min-width="160" />
          <el-table-column prop="grossRequirement" label="毛需求" width="100" align="right" />
          <el-table-column prop="onHandQty" label="现存量" width="100" align="right" />
          <el-table-column prop="netRequirement" label="净需求" width="100" align="right" />
          <el-table-column prop="suggestType" label="建议" width="90" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.suggestType==='BUY'?'warning':'success'">{{ row.suggestType==='BUY'?'采购':'生产' }}</el-tag></template>
          </el-table-column>
          <el-table-column prop="status" label="状态" width="90" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.status===1?'success':'info'">{{ row.status===1?'已转单':'待转' }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="160" fixed="right">
            <template #default="{ row }">
              <el-button link type="primary" :disabled="row.suggestType!=='BUY' || row.status===1" @click="openMrpToPo(row)">转采购订单</el-button>
            </template>
          </el-table-column>
        </el-table>
        <ErpPagination :total="mrp.total" v-model:page="mrp.page" v-model:size="mrp.size" @change="mrpLoad" @size-change="mrpLoad" />
      </el-tab-pane>
    </el-tabs>

    <el-dialog v-model="prCreateVisible" title="新建生产订单" width="560px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="产品"><el-select v-model="prForm.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></el-form-item>
        <el-form-item label="计划数量"><el-input-number v-model="prForm.planQty" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></el-form-item>
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="开始"><el-date-picker v-model="prForm.startDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="结束"><el-date-picker v-model="prForm.endDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" :disabled-date="disablePrEnd" style="width:100%" /></el-form-item></el-col>
        </el-row>
      </el-form>
      <template #footer><el-button @click="prCreateVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitPr">保存</el-button></template>
    </el-dialog>

    <el-dialog v-model="issueVisible" title="生产领料" width="560px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="物料"><el-select v-model="issueForm.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></el-form-item>
        <el-form-item label="领料数量"><el-input-number v-model="issueForm.issueQty" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></el-form-item>
        <el-form-item label="仓库"><el-select v-model="issueForm.warehouseId" style="width:100%"><el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" /></el-select></el-form-item>
      </el-form>
      <template #footer><el-button @click="issueVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitIssue">确认领料</el-button></template>
    </el-dialog>

    <el-dialog v-model="reportVisible" title="生产报工" width="560px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="工序"><el-input v-model="reportForm.processName" placeholder="如 注塑/装配" /></el-form-item>
        <el-form-item label="报工数量"><el-input-number v-model="reportForm.reportQty" :min="1" controls-position="right" style="width:100%" /></el-form-item>
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="良品数"><el-input-number v-model="reportForm.goodQty" :min="0" controls-position="right" style="width:100%" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="不良数"><el-input-number v-model="reportForm.badQty" :min="0" controls-position="right" style="width:100%" /></el-form-item></el-col>
        </el-row>
        <el-form-item label="工时(h)"><el-input-number v-model="reportForm.workHours" :min="0" :precision="2" controls-position="right" style="width:100%" /></el-form-item>
      </el-form>
      <template #footer><el-button @click="reportVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitReport">提交报工</el-button></template>
    </el-dialog>

    <el-dialog v-model="completeVisible" title="完工入库" width="480px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="入库仓库"><el-select v-model="completeForm.warehouseId" style="width:100%"><el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" /></el-select></el-form-item>
      </el-form>
      <template #footer><el-button @click="completeVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitComplete">确认入库</el-button></template>
    </el-dialog>

    <el-dialog v-model="mpsVisible" title="新建主生产计划" width="560px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="产品"><el-select v-model="mpsForm.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></el-form-item>
        <el-form-item label="计划数量"><el-input-number v-model="mpsForm.planQty" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></el-form-item>
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="起始"><el-date-picker v-model="mpsForm.planStartDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="截止"><el-date-picker v-model="mpsForm.planEndDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" :disabled-date="disableMpsEnd" style="width:100%" /></el-form-item></el-col>
        </el-row>
        <el-form-item label="来源类型"><el-input v-model="mpsForm.sourceType" placeholder="如 销售订单/预测" /></el-form-item>
      </el-form>
      <template #footer><el-button @click="mpsVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitMps">保存</el-button></template>
    </el-dialog>

    <el-dialog v-model="mrpToPoVisible" title="MRP建议转采购订单" width="480px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="供应商"><el-select v-model="mrpToPoForm.supplierId" filterable style="width:100%"><el-option v-for="s in suppliers" :key="s.id" :label="s.supplierName" :value="s.id" /></el-select></el-form-item>
      </el-form>
      <template #footer><el-button @click="mrpToPoVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitMrpToPo">确认转单</el-button></template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { ElMessage } from 'element-plus'
import { get, post, del } from '@/utils/request'
import { fetchOptions } from '@/composables/usePage'

const tab = ref('pr')
const saving = ref(false)
const running = ref(false)
const today = new Date().toISOString().slice(0, 10)
const materials = ref<any[]>([])
const warehouses = ref<any[]>([])
const suppliers = ref<any[]>([])

const pr = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function prLoad() { pr.loading = true; try { const res: any = await get('/api/pr', { page: pr.page, size: pr.size, keyword: pr.keyword }); pr.total = res?.total ?? 0; pr.rows = res?.rows ?? [] } finally { pr.loading = false } }
const prSearch = () => { pr.page = 1; prLoad() }
const prReset = () => { pr.keyword = ''; prSearch() }
function prStatus(s: number) { return s === 0 ? { l: '计划', t: 'info' } : s === 1 ? { l: '已下达', t: '' } : s === 2 ? { l: '生产中', t: 'warning' } : s === 3 ? { l: '已完工', t: 'success' } : { l: '已关闭', t: 'info' } }
async function releasePr(r: any) { await post(`/api/pr/${r.id}/release`); ElMessage.success('已下达'); prLoad() }

const mps = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function mpsLoad() { mps.loading = true; try { const res: any = await get('/api/pr/mps', { page: mps.page, size: mps.size, keyword: mps.keyword }); mps.total = res?.total ?? 0; mps.rows = res?.rows ?? [] } finally { mps.loading = false } }
const mpsSearch = () => { mps.page = 1; mpsLoad() }
const mpsReset = () => { mps.keyword = ''; mpsSearch() }
async function confirmMps(r: any) { await post(`/api/pr/mps/${r.id}/confirm`); ElMessage.success('已确认'); mpsLoad() }
async function mpsToOrder(r: any) { await post('/api/pr/mps/to-order', { mpsId: r.id }); ElMessage.success('已转生产订单'); mpsLoad(); prLoad() }

const mrp = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function mrpLoad() { mrp.loading = true; try { const res: any = await get('/api/pr/mrp', { page: mrp.page, size: mrp.size, keyword: mrp.keyword }); mrp.total = res?.total ?? 0; mrp.rows = res?.rows ?? [] } finally { mrp.loading = false } }
async function runMrp() { running.value = true; try { await post('/api/pr/mrp'); ElMessage.success('MRP 运算完成'); mrpLoad() } finally { running.value = false } }

const prCreateVisible = ref(false)
const issueVisible = ref(false)
const reportVisible = ref(false)
const completeVisible = ref(false)
const mpsVisible = ref(false)
const mrpToPoVisible = ref(false)
const currentPrId = ref<number>()
const currentMrpId = ref<number>()

const prForm = reactive({ materialId: undefined as number | undefined, planQty: 1, startDate: today, endDate: today })
const disablePrEnd = (d: Date) => !!prForm.startDate && d.getTime() < new Date(`${prForm.startDate}T00:00:00`).getTime()
const issueForm = reactive({ materialId: undefined as number | undefined, issueQty: 1, warehouseId: undefined as number | undefined })
const reportForm = reactive({ processName: '', reportQty: 1, goodQty: 1, badQty: 0, workHours: 0 })
const completeForm = reactive({ warehouseId: undefined as number | undefined })
const mpsForm = reactive({ materialId: undefined as number | undefined, planQty: 1, planStartDate: today, planEndDate: today, sourceType: '' })
const disableMpsEnd = (d: Date) => !!mpsForm.planStartDate && d.getTime() < new Date(`${mpsForm.planStartDate}T00:00:00`).getTime()
const mrpToPoForm = reactive({ supplierId: undefined as number | undefined })

function openPrCreate() { prCreateVisible.value = true }
function openIssue(r: any) { currentPrId.value = r.id; issueForm.materialId = r.materialId; issueForm.issueQty = 1; issueVisible.value = true }
function openReport(r: any) { currentPrId.value = r.id; reportForm.processName=''; reportForm.reportQty=1; reportForm.goodQty=1; reportForm.badQty=0; reportForm.workHours=0; reportVisible.value = true }
function openComplete(r: any) { currentPrId.value = r.id; completeVisible.value = true }
function openMpsCreate() { mpsVisible.value = true }
function openMrpToPo(r: any) { currentMrpId.value = r.id; mrpToPoForm.supplierId = undefined; mrpToPoVisible.value = true }

async function submitPr() { if (!prForm.materialId) return ElMessage.warning('请选择产品'); saving.value = true; try { await post('/api/pr', { ...prForm }); ElMessage.success('创建成功'); prCreateVisible.value = false; prLoad() } finally { saving.value = false } }
async function submitIssue() { if (!issueForm.materialId || !issueForm.warehouseId) return ElMessage.warning('请选择物料与仓库'); saving.value = true; try { await post('/api/pr/issue', { prOrderId: currentPrId.value, ...issueForm }); ElMessage.success('领料成功'); issueVisible.value = false; prLoad() } finally { saving.value = false } }
async function submitReport() { saving.value = true; try { await post('/api/pr/report', { prOrderId: currentPrId.value, ...reportForm }); ElMessage.success('报工成功'); reportVisible.value = false; prLoad() } finally { saving.value = false } }
async function submitComplete() { if (!completeForm.warehouseId) return ElMessage.warning('请选择仓库'); saving.value = true; try { await post(`/api/pr/${currentPrId.value}/complete?warehouseId=${completeForm.warehouseId}`); ElMessage.success('完工入库成功'); completeVisible.value = false; prLoad() } finally { saving.value = false } }
async function submitMps() { if (!mpsForm.materialId || mpsForm.planQty <= 0) return ElMessage.warning('请填写产品与计划量'); saving.value = true; try { await post('/api/pr/mps', { ...mpsForm }); ElMessage.success('MPS 已创建'); mpsVisible.value = false; mpsLoad() } finally { saving.value = false } }
async function submitMrpToPo() { if (!mrpToPoForm.supplierId) return ElMessage.warning('请选择供应商'); saving.value = true; try { await post('/api/pr/mrp/to-po', { mrpId: currentMrpId.value, supplierId: mrpToPoForm.supplierId }); ElMessage.success('已转采购订单'); mrpToPoVisible.value = false; mrpLoad() } finally { saving.value = false } }

onMounted(async () => {
  materials.value = await fetchOptions('material')
  warehouses.value = await fetchOptions('warehouse')
  suppliers.value = await fetchOptions('supplier')
  prLoad(); mpsLoad(); mrpLoad()
})
</script>

<style scoped>
.toolbar { display: flex; justify-content: space-between; margin-bottom: 12px; }
.left, .right { display: flex; gap: 8px; align-items: center; }
.pager { margin-top: 12px; justify-content: flex-end; }
</style>
