<template>
  <div class="page">
    <el-tabs v-model="tab">
      <el-tab-pane label="采购订单" name="po">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="po.keyword" placeholder="采购单号" clearable style="width: 200px" @keyup.enter="poSearch" />
            <el-button type="primary" @click="poSearch">查询</el-button>
            <el-button @click="poReset">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openPoCreate">新建采购订单</el-button>
            <el-button @click="poLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="po.rows" v-loading="po.loading" border stripe size="small">
          <el-table-column prop="orderNo" label="单号" width="160" />
          <el-table-column prop="supplierName" label="供应商" min-width="160" show-overflow-tooltip />
          <el-table-column prop="materials" label="物料" min-width="220" show-overflow-tooltip />
          <el-table-column prop="orderDate" label="下单日期" width="120" />
          <el-table-column prop="totalAmount" label="金额" width="120" align="right" />
          <el-table-column prop="status" label="状态" width="110" align="center">
            <template #default="{ row }"><el-tag size="small" :type="poStatus(row.status).t">{{ poStatus(row.status).l }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="280" fixed="right">
            <template #default="{ row }">
              <el-button link type="primary" @click="openPoChange(row)">变更</el-button>
              <el-button link type="warning" :disabled="row.status !== 1 && row.status !== 2" @click="openReceive(row)">收货</el-button>
              <el-button link type="danger" :disabled="row.status !== 1 && row.status !== 2" @click="inspectPo(row)">质检入库</el-button>
            </template>
          </el-table-column>
        </el-table>
        <ErpPagination :total="po.total" v-model:page="po.page" v-model:size="po.size" @change="poLoad" @size-change="poSearch" />
      </el-tab-pane>

      <el-tab-pane label="请购单" name="req">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="req.keyword" placeholder="请购单号" clearable style="width: 200px" @keyup.enter="reqSearch" />
            <el-button type="primary" @click="reqSearch">查询</el-button>
            <el-button @click="reqReset">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openReqCreate">新建请购单</el-button>
            <el-button @click="reqLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="req.rows" v-loading="req.loading" border stripe size="small">
          <el-table-column prop="reqNo" label="单号" width="160" />
          <el-table-column prop="deptName" label="申请部门" width="140" />
          <el-table-column prop="reqDate" label="申请日期" width="120" />
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.status === 1 ? 'success' : 'info'">{{ row.status === 1 ? '已转单' : '待处理' }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="140" fixed="right">
            <template #default="{ row }">
              <el-button link type="primary" :disabled="row.status === 1" @click="openReqToPo(row)">转采购订单</el-button>
            </template>
          </el-table-column>
        </el-table>
        <ErpPagination :total="req.total" v-model:page="req.page" v-model:size="req.size" @change="reqLoad" @size-change="reqSearch" />
      </el-tab-pane>

      <el-tab-pane label="供应商报价" name="quote">
        <div class="toolbar">
          <div class="left">
            <el-select v-model="quote.materialId" placeholder="物料" clearable filterable style="width: 200px">
              <el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" />
            </el-select>
            <el-button type="primary" @click="quoteLoad">查询</el-button>
            <el-button @click="quote.materialId = undefined; quoteLoad()">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openQuote">登记报价</el-button>
            <el-button @click="quoteLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="quote.rows" v-loading="quote.loading" border stripe size="small">
          <el-table-column prop="supplierName" label="供应商" min-width="160" show-overflow-tooltip />
          <el-table-column prop="materialCode" label="物料编码" width="130" />
          <el-table-column prop="materialName" label="物料名称" min-width="160" />
          <el-table-column prop="quotePrice" label="报价单价" width="120" align="right" />
          <el-table-column prop="validFrom" label="生效起" width="120" />
          <el-table-column prop="validTo" label="生效止" width="120" />
        </el-table>
        <ErpPagination :total="quote.total" v-model:page="quote.page" v-model:size="quote.size" @change="quoteLoad" @size-change="quoteLoad" />
      </el-tab-pane>

      <el-tab-pane label="供应商绩效" name="perf">
        <div class="toolbar">
          <div class="left">
            <el-select v-model="perf.supplierId" placeholder="选择供应商" filterable style="width: 240px" @change="loadPerf">
              <el-option v-for="s in suppliers" :key="s.id" :label="s.supplierName" :value="s.id" />
            </el-select>
            <el-button type="primary" @click="loadPerf">评估</el-button>
          </div>
        </div>
        <el-empty v-if="!perf.data" description="请选择供应商并点击「评估」查看绩效报告" />
        <div v-if="perf.data" class="perf-wrap">
          <el-descriptions border :column="3" size="small">
            <el-descriptions-item label="供应商">{{ perf.data.supplierName }}</el-descriptions-item>
            <el-descriptions-item label="综合评分">
              <el-tag :type="perf.data.grade === 'A' ? 'success' : perf.data.grade === 'D' ? 'danger' : 'warning'">{{ perf.data.score }}（{{ perf.data.grade }}级）</el-tag>
            </el-descriptions-item>
            <el-descriptions-item label="订单数">{{ perf.data.orderCount }}</el-descriptions-item>
            <el-descriptions-item label="收货单数">{{ perf.data.receiptCount }}</el-descriptions-item>
            <el-descriptions-item label="准时率">{{ perf.data.onTimeRate }}%</el-descriptions-item>
            <el-descriptions-item label="合格率">{{ perf.data.passRate }}%</el-descriptions-item>
            <el-descriptions-item label="订单完成率">{{ perf.data.completeRate }}%</el-descriptions-item>
            <el-descriptions-item label="累计采购额">¥ {{ perf.data.totalAmount }}</el-descriptions-item>
            <el-descriptions-item label="贡献度">{{ perf.data.contribution }}%</el-descriptions-item>
          </el-descriptions>
        </div>
      </el-tab-pane>
    </el-tabs>

    <el-dialog v-model="poCreateVisible" title="新建采购订单" width="760px" destroy-on-close>
      <el-form label-width="90px">
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="供应商"><el-select v-model="poForm.supplierId" filterable style="width:100%"><el-option v-for="s in suppliers" :key="s.id" :label="s.supplierName" :value="s.id" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="下单日期"><el-date-picker v-model="poForm.orderDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item></el-col>
        </el-row>
      </el-form>
      <el-table :data="poForm.lines" border size="small">
        <el-table-column label="物料" min-width="220">
          <template #default="{ row }"><el-select v-model="row.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></template>
        </el-table-column>
        <el-table-column label="数量" width="150"><template #default="{ row }"><el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></template></el-table-column>
        <el-table-column label="单价" width="150"><template #default="{ row }"><el-input-number v-model="row.unitPrice" :min="0" :precision="4" controls-position="right" style="width:100%" /></template></el-table-column>
        <el-table-column label="操作" width="80"><template #default="{ $index }"><el-button link type="danger" @click="poForm.lines.splice($index,1)">删</el-button></template></el-table-column>
      </el-table>
      <div class="dlg-actions"><el-button @click="poForm.lines.push({ materialId: undefined, quantity: 1, unitPrice: 0 })">添加明细</el-button></div>
      <template #footer><el-button @click="poCreateVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitPo">保存</el-button></template>
    </el-dialog>

    <el-dialog v-model="recvVisible" title="采购收货" width="720px" destroy-on-close>
      <el-form label-width="90px"><el-form-item label="收货日期"><el-date-picker v-model="recvForm.receiptDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:240px" /></el-form-item></el-form>
      <el-table :data="recvForm.lines" border size="small">
        <el-table-column label="物料" min-width="240"><template #default="{ row }"><el-select v-model="row.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></template></el-table-column>
        <el-table-column label="收货数量" width="180"><template #default="{ row }"><el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></template></el-table-column>
        <el-table-column label="操作" width="80"><template #default="{ $index }"><el-button link type="danger" @click="recvForm.lines.splice($index,1)">删</el-button></template></el-table-column>
      </el-table>
      <div class="dlg-actions"><el-button @click="recvForm.lines.push({ materialId: undefined, materialName: '', quantity: 1 })">添加明细</el-button></div>
      <template #footer><el-button @click="recvVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitReceive">收货</el-button></template>
    </el-dialog>

    <el-dialog v-model="changeVisible" title="采购订单变更" width="720px" destroy-on-close>
      <el-input v-model="changeForm.reason" placeholder="变更原因" style="margin-bottom:12px" />
      <el-table :data="changeForm.lines" border size="small">
        <el-table-column label="物料" min-width="240"><template #default="{ row }"><el-select v-model="row.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></template></el-table-column>
        <el-table-column label="数量" width="180"><template #default="{ row }"><el-input-number v-model="row.quantity" :min="0" :precision="4" controls-position="right" style="width:100%" /></template></el-table-column>
        <el-table-column label="操作" width="80"><template #default="{ $index }"><el-button link type="danger" @click="changeForm.lines.splice($index,1)">删</el-button></template></el-table-column>
      </el-table>
      <div class="dlg-actions"><el-button @click="changeForm.lines.push({ materialId: undefined, materialName: '', quantity: 0 })">添加明细</el-button></div>
      <template #footer><el-button @click="changeVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitChange">保存变更</el-button></template>
    </el-dialog>

    <el-dialog v-model="reqVisible" :title="reqDialogTitle" width="760px" destroy-on-close>
      <el-form label-width="90px">
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="申请部门"><el-input v-model="reqForm.deptName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="申请日期"><el-date-picker v-model="reqForm.reqDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item></el-col>
        </el-row>
        <el-form-item label="备注"><el-input v-model="reqForm.remark" /></el-form-item>
        <el-form-item v-if="reqMode==='toPo'" label="供应商"><el-select v-model="reqForm.supplierId" filterable style="width:100%"><el-option v-for="s in suppliers" :key="s.id" :label="s.supplierName" :value="s.id" /></el-select></el-form-item>
      </el-form>
      <el-table :data="reqForm.lines" border size="small">
        <el-table-column label="物料" min-width="220"><template #default="{ row }"><el-select v-model="row.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></template></el-table-column>
        <el-table-column label="请购量" width="160"><template #default="{ row }"><el-input-number v-model="row.reqQty" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></template></el-table-column>
        <el-table-column label="操作" width="80"><template #default="{ $index }"><el-button link type="danger" @click="reqForm.lines.splice($index,1)">删</el-button></template></el-table-column>
      </el-table>
      <div class="dlg-actions"><el-button @click="reqForm.lines.push({ materialId: undefined, reqQty: 1 })">添加明细</el-button></div>
      <template #footer><el-button @click="reqVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitReq">保存</el-button></template>
    </el-dialog>

    <el-dialog v-model="quoteVisible" title="登记供应商报价" width="560px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="供应商"><el-select v-model="quoteForm.supplierId" filterable style="width:100%"><el-option v-for="s in suppliers" :key="s.id" :label="s.supplierName" :value="s.id" /></el-select></el-form-item>
        <el-form-item label="物料"><el-select v-model="quoteForm.materialId" filterable style="width:100%"><el-option v-for="m in materials" :key="m.id" :label="`${m.materialCode} ${m.materialName}`" :value="m.id" /></el-select></el-form-item>
        <el-form-item label="报价单价"><el-input-number v-model="quoteForm.quotePrice" :min="0.0001" :precision="4" controls-position="right" style="width:100%" /></el-form-item>
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="生效起"><el-date-picker v-model="quoteForm.validFrom" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="生效止"><el-date-picker v-model="quoteForm.validTo" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" :disabled-date="disableQuoteEnd" style="width:100%" /></el-form-item></el-col>
        </el-row>
      </el-form>
      <template #footer><el-button @click="quoteVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitQuote">保存</el-button></template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { get, post } from '@/utils/request'
import { submitApproval } from '@/utils/submitApproval'
import { fetchOptions } from '@/composables/usePage'

const tab = ref('po')
const saving = ref(false)
const today = new Date().toISOString().slice(0, 10)

const suppliers = ref<any[]>([])
const materials = ref<any[]>([])

const po = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function poLoad() {
  po.loading = true
  try {
    const res: any = await get('/api/po', { page: po.page, size: po.size, keyword: po.keyword })
    po.total = res?.total ?? 0; po.rows = res?.rows ?? []
  } finally { po.loading = false }
}
const poSearch = () => { po.page = 1; poLoad() }
const poReset = () => { po.keyword = ''; poSearch() }
function poStatus(s: number) {
  return s === 0 ? { l: '待审核', t: 'info' } : s === 1 ? { l: '已审核', t: '' } : s === 2 ? { l: '部分收货', t: 'warning' } : { l: '已完成', t: 'success' }
}
async function inspectPo(r: any) {
  try {
    await ElMessageBox.confirm('确认对本次收货质检通过并入库？', '提示', { type: 'warning' })
  } catch {
    return
  }
  await post(`/api/po/${r.id}/inspect?pass=true`); ElMessage.success('质检入库完成'); poLoad()
}

const req = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function reqLoad() {
  req.loading = true
  try { const res: any = await get('/api/po/requisition', { page: req.page, size: req.size, keyword: req.keyword }); req.total = res?.total ?? 0; req.rows = res?.rows ?? [] } finally { req.loading = false }
}
const reqSearch = () => { req.page = 1; reqLoad() }
const reqReset = () => { req.keyword = ''; reqSearch() }

const quote = reactive({ materialId: undefined as number | undefined, page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function quoteLoad() {
  quote.loading = true
  try { const res: any = await get('/api/po/quote', { page: quote.page, size: quote.size, keyword: '', materialId: quote.materialId ?? '' }); quote.total = res?.total ?? 0; quote.rows = res?.rows ?? [] } finally { quote.loading = false }
}

const perf = reactive({ supplierId: undefined as number | undefined, data: null as any })

const poCreateVisible = ref(false)
const recvVisible = ref(false)
const changeVisible = ref(false)
const reqVisible = ref(false)
const quoteVisible = ref(false)
const reqMode = ref<'create' | 'toPo'>('create')
const reqDialogTitle = ref('新建请购单')
const currentPoId = ref<number>()
const currentRecvId = ref<number>()
const currentReqId = ref<number>()

const poForm = reactive({ supplierId: undefined as number | undefined, orderDate: today, lines: [{ materialId: undefined, quantity: 1, unitPrice: 0 }] })
const recvForm = reactive({ receiptDate: today, lines: [] as any[] })
const changeForm = reactive({ reason: '', lines: [] as any[] })
const reqForm = reactive({ deptName: '', reqDate: today, remark: '', supplierId: undefined as number | undefined, lines: [{ materialId: undefined, reqQty: 1 }] })
const quoteForm = reactive({ supplierId: undefined as number | undefined, materialId: undefined as number | undefined, quotePrice: 1, validFrom: '', validTo: '' })
const disableQuoteEnd = (d: Date) => !!quoteForm.validFrom && d.getTime() < new Date(`${quoteForm.validFrom}T00:00:00`).getTime()

function openPoCreate() { poCreateVisible.value = true }
function openReceive(r: any) {
  currentRecvId.value = r.id
  recvForm.receiptDate = today
  get(`/api/po`, { page: 1, size: 1, keyword: r.orderNo }).then(() => {})
  recvForm.lines = [{ materialId: undefined, materialName: '', quantity: 1 }]
  recvVisible.value = true
}
function openPoChange(r: any) { currentPoId.value = r.id; changeForm.reason = ''; changeForm.lines = [{ materialId: undefined, materialName: '', quantity: 0 }]; changeVisible.value = true }
function openReqCreate() { reqMode.value = 'create'; reqDialogTitle.value = '新建请购单'; reqForm.deptName=''; reqForm.remark=''; reqForm.supplierId=undefined; reqForm.lines=[{materialId:undefined,reqQty:1}]; reqVisible.value = true }
function openReqToPo(r: any) { reqMode.value = 'toPo'; currentReqId.value = r.id; reqDialogTitle.value = `请购单转采购订单（${r.reqNo}）`; reqForm.supplierId=undefined; reqForm.reqDate = r.reqDate; reqForm.lines=[{materialId:undefined,reqQty:1}]; reqVisible.value = true }
function openQuote() { quoteForm.supplierId=undefined; quoteForm.materialId=undefined; quoteForm.quotePrice=1; quoteForm.validFrom=''; quoteForm.validTo=''; quoteVisible.value = true }

async function submitPoApproval(order: any) {
  return submitApproval({
    bizType: 'PO',
    bizId: order?.id,
    billNo: order?.orderNo,
    title: `采购订单审批 ${order?.orderNo ?? ''}`,
    amount: order?.totalAmount,
    remark: '新建采购订单自动送审'
  })
}

async function submitPo() {
  if (!poForm.supplierId) return ElMessage.warning('请选择供应商')
  if (!poForm.lines.length || poForm.lines.some(l => !l.materialId || l.quantity <= 0)) return ElMessage.warning('明细不完整')
  saving.value = true
  try {
    const res: any = await post('/api/po', { supplierId: poForm.supplierId, orderDate: poForm.orderDate, lines: poForm.lines.map((l: any) => ({ materialId: l.materialId, quantity: l.quantity, unitPrice: l.unitPrice })) })
    const order = res?.id ? res : res?.data
    const ok = await submitPoApproval(order)
    ElMessage.success(ok ? '创建成功，已提交审批中心审核' : '创建成功（审批单提交失败，请在审批流中手动提交）')
    poCreateVisible.value = false; poLoad()
  } finally { saving.value = false }
}
async function submitReceive() {
  if (!recvForm.lines.length || recvForm.lines.some(l => !l.materialId || l.quantity <= 0)) return ElMessage.warning('收货明细不完整')
  saving.value = true
  try {
    const res: any = await post('/api/po/receive', { orderId: currentRecvId.value, receiptDate: recvForm.receiptDate, lines: recvForm.lines.map((l: any) => ({ materialId: l.materialId, quantity: l.quantity })) })
    const ok = await submitApproval({ bizType: 'RECEIPT', bizId: res?.id, billNo: res?.receiptNo, title: `采购收货审批 ${res?.receiptNo ?? ''}`, remark: '新建采购收货自动送审' })
    ElMessage.success(ok ? '收货成功，已提交审批中心审核' : '收货成功（审批单提交失败，请在审批流中手动提交）')
    recvVisible.value = false; poLoad()
  } finally { saving.value = false }
}
async function submitChange() {
  if (!changeForm.lines.length) return ElMessage.warning('请至少填写一项变更')
  saving.value = true
  try {
    const order = po.rows.find((r: any) => r.id === currentPoId.value)
    await post('/api/po/change', { orderId: currentPoId.value, reason: changeForm.reason, lines: changeForm.lines.filter(l => l.materialId).map((l: any) => ({ materialId: l.materialId, quantity: l.quantity })) })
    const ok = await submitApproval({ bizType: 'PO_CHANGE', bizId: currentPoId.value, billNo: order?.orderNo, title: `采购订单变更审批 ${order?.orderNo ?? ''}`, remark: `变更原因：${changeForm.reason ?? ''}` })
    ElMessage.success(ok ? '已提交变更，已送审批中心审核' : '已提交变更（审批单提交失败，请在审批流中手动提交）')
    changeVisible.value = false; poLoad()
  } finally { saving.value = false }
}
async function submitReq() {
  if (!reqForm.lines.length || reqForm.lines.some(l => !l.materialId || l.reqQty <= 0)) return ElMessage.warning('明细不完整')
  saving.value = true
  try {
    if (reqMode.value === 'create') {
      const res: any = await post('/api/po/requisition', { deptName: reqForm.deptName, reqDate: reqForm.reqDate, remark: reqForm.remark, lines: reqForm.lines.map((l: any) => ({ materialId: l.materialId, reqQty: l.reqQty })) })
      const ok = await submitApproval({ bizType: 'REQUISITION', bizId: res?.id, billNo: res?.reqNo, title: `采购请购审批 ${res?.reqNo ?? ''}`, remark: '新建请购单自动送审' })
      ElMessage.success(ok ? '请购单已创建，已提交审批中心审核' : '请购单已创建（审批单提交失败，请在审批流中手动提交）')
    } else {
      const res: any = await post('/api/po/requisition/to-po', { reqId: currentReqId.value, supplierId: reqForm.supplierId, orderDate: reqForm.reqDate })
      const order = res?.id ? res : res?.data
      const ok = await submitPoApproval(order)
      ElMessage.success(ok ? '已转为采购订单，已提交审批中心审核' : '已转为采购订单（审批单提交失败，请在审批流中手动提交）')
    }
    reqVisible.value = false; reqLoad(); poLoad()
  } finally { saving.value = false }
}
async function submitQuote() {
  if (!quoteForm.supplierId || !quoteForm.materialId) return ElMessage.warning('请选择供应商与物料')
  saving.value = true
  try { await post('/api/po/quote', { ...quoteForm }); ElMessage.success('报价已登记'); quoteVisible.value = false; quoteLoad() } finally { saving.value = false }
}
async function loadPerf() {
  if (!perf.supplierId) { perf.data = null; return }
  perf.data = await get(`/api/po/performance/${perf.supplierId}`)
}

onMounted(async () => {
  suppliers.value = await fetchOptions('supplier')
  materials.value = await fetchOptions('material')
  poLoad(); reqLoad(); quoteLoad()
})
</script>

<style scoped>
.toolbar { display: flex; justify-content: space-between; margin-bottom: 12px; }
.left, .right { display: flex; gap: 8px; align-items: center; }
.pager { margin-top: 12px; justify-content: flex-end; }
.dlg-actions { margin-top: 10px; }
.perf-wrap { margin-top: 12px; }
</style>
