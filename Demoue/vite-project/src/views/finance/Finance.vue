<template>
  <div class="page">
    <el-tabs v-model="tab">
      <el-tab-pane label="总账凭证" name="voucher">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="v.keyword" placeholder="凭证号/来源" clearable style="width: 220px" @keyup.enter="vSearch" />
            <el-button type="primary" @click="vSearch">查询</el-button>
            <el-button @click="vReset">重置</el-button>
          </div>
          <div class="right">
            <el-button type="primary" @click="openVoucher">录凭证</el-button>
            <el-button @click="vLoad">刷新</el-button>
          </div>
        </div>
        <el-table :data="v.rows" v-loading="v.loading" border stripe size="small">
          <el-table-column prop="voucherNo" label="凭证号" width="160" />
          <el-table-column prop="voucherDate" label="日期" width="120" />
          <el-table-column prop="sourceModule" label="来源" width="110" />
          <el-table-column prop="totalDebit" label="借方" width="120" align="right" />
          <el-table-column prop="totalCredit" label="贷方" width="120" align="right" />
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }"><el-tag size="small" :type="vStatus(row.status).t">{{ vStatus(row.status).l }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="300" fixed="right">
            <template #default="{ row }">
              <el-button link type="primary" @click="openVoucherDetail(row)">明细</el-button>
              <el-button link type="success" :disabled="row.status !== 0" @click="act(`/api/fin/voucher/${row.id}/audit`, '已审核', vLoad)">审核</el-button>
              <el-button link type="warning" :disabled="row.status !== 1" @click="act(`/api/fin/voucher/${row.id}/post`, '已记账', vLoad)">记账</el-button>
              <el-button link type="danger" :disabled="row.status !== 2" @click="act(`/api/fin/voucher/${row.id}/close`, '已结账', vLoad)">结账</el-button>
            </template>
          </el-table-column>
        </el-table>
        <ErpPagination :total="v.total" v-model:page="v.page" v-model:size="v.size" @change="vLoad" @size-change="vSearch" />
      </el-tab-pane>

      <el-tab-pane label="应收单" name="ar">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="ar.keyword" placeholder="应收单号/客户" clearable style="width: 220px" @keyup.enter="arSearch" />
            <el-button type="primary" @click="arSearch">查询</el-button>
            <el-button @click="arReset">重置</el-button>
          </div>
          <div class="right"><el-button @click="arLoad">刷新</el-button></div>
        </div>
        <el-table :data="ar.rows" v-loading="ar.loading" border stripe size="small">
          <el-table-column prop="recvNo" label="单号" width="160" />
          <el-table-column prop="customerName" label="客户" width="150" show-overflow-tooltip />
          <el-table-column prop="amount" label="应收金额" width="120" align="right" />
          <el-table-column prop="receivedAmount" label="已收" width="120" align="right" />
          <el-table-column prop="dueDate" label="到期日" width="120" />
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.status===2?'success':row.status===1?'warning':'info'">{{ row.status===2?'已结清':row.status===1?'部分收':'未收' }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="120" fixed="right"><template #default="{ row }"><el-button link type="primary" @click="openWriteoff(row, 'ar')">核销收款</el-button></template></el-table-column>
        </el-table>
        <ErpPagination :total="ar.total" v-model:page="ar.page" v-model:size="ar.size" @change="arLoad" @size-change="arSearch" />
      </el-tab-pane>

      <el-tab-pane label="应付单" name="ap">
        <div class="toolbar">
          <div class="left">
            <el-input v-model="ap.keyword" placeholder="应付单号/供应商" clearable style="width: 220px" @keyup.enter="apSearch" />
            <el-button type="primary" @click="apSearch">查询</el-button>
            <el-button @click="apReset">重置</el-button>
          </div>
          <div class="right"><el-button @click="apLoad">刷新</el-button></div>
        </div>
        <el-table :data="ap.rows" v-loading="ap.loading" border stripe size="small">
          <el-table-column prop="payNo" label="单号" width="160" />
          <el-table-column prop="supplierName" label="供应商" width="180" />
          <el-table-column prop="amount" label="应付金额" width="120" align="right" />
          <el-table-column prop="paidAmount" label="已付" width="120" align="right" />
          <el-table-column prop="dueDate" label="到期日" width="120" />
          <el-table-column prop="status" label="状态" width="100" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.status===2?'success':row.status===1?'warning':'info'">{{ row.status===2?'已结清':row.status===1?'部分付':'未付' }}</el-tag></template>
          </el-table-column>
          <el-table-column label="操作" width="120" fixed="right"><template #default="{ row }"><el-button link type="primary" @click="openWriteoff(row, 'ap')">核销付款</el-button></template></el-table-column>
        </el-table>
        <ErpPagination :total="ap.total" v-model:page="ap.page" v-model:size="ap.size" @change="apLoad" @size-change="apSearch" />
      </el-tab-pane>

      <el-tab-pane label="成本核算" name="cost">
        <el-card class="cost-card" shadow="never">
          <template #header><b>单位成本核算（材料+人工+费用归集）</b></template>
          <el-form :inline="true" label-width="120px">
            <el-form-item label="生产订单ID"><el-input-number v-model="costForm.prOrderId" :min="1" controls-position="right" /></el-form-item>
            <el-form-item label="人工成本"><el-input-number v-model="costForm.laborCost" :min="0" :precision="2" controls-position="right" /></el-form-item>
            <el-form-item label="制造费用"><el-input-number v-model="costForm.overheadCost" :min="0" :precision="2" controls-position="right" /></el-form-item>
            <el-form-item><el-button type="primary" :loading="saving" @click="calcCost">计算单位成本</el-button></el-form-item>
          </el-form>
          <el-alert v-if="costResult" type="success" :closable="false" show-icon>
            <template #title>单位成本 = {{ costResult.unitCost }}（总成本 {{ costResult.totalCost }}，材料 {{ costResult.materialCost }} / 人工 {{ costResult.laborCost }} / 费用 {{ costResult.overheadCost }}）</template>
          </el-alert>
        </el-card>
      </el-tab-pane>

      <el-tab-pane label="账龄分析" name="ageing">
        <el-button @click="loadAgeing" style="margin-bottom:12px">刷新账龄</el-button>
        <el-table :data="ageing" v-loading="ageingLoading" border stripe size="small">
          <el-table-column prop="recvNo" label="应收单号" width="160" />
          <el-table-column prop="customerName" label="客户" width="150" show-overflow-tooltip />
          <el-table-column prop="open" label="未收金额" width="140" align="right" />
          <el-table-column prop="bucket" label="账龄区间" width="140" align="center">
            <template #default="{ row }"><el-tag size="small" :type="row.bucket==='未到期'?'success':row.bucket==='90+'?'danger':'warning'">{{ row.bucket }}</el-tag></template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
    </el-tabs>

    <el-dialog v-model="voucherVisible" title="录入记账凭证" width="820px" destroy-on-close>
      <el-form label-width="90px">
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="凭证日期"><el-date-picker v-model="voucherForm.voucherDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item></el-col>
        </el-row>
      </el-form>
      <el-table :data="voucherForm.entries" border size="small">
        <el-table-column label="方向" width="120"><template #default="{ row }"><el-select v-model="row.direction" style="width:100%"><el-option label="借 DR" value="DR" /><el-option label="贷 CR" value="CR" /></el-select></template></el-table-column>
        <el-table-column label="金额" width="160"><template #default="{ row }"><el-input-number v-model="row.amount" :min="0" :precision="2" controls-position="right" style="width:100%" /></template></el-table-column>
        <el-table-column label="摘要" min-width="180"><template #default="{ row }"><el-input v-model="row.memo" /></template></el-table-column>
        <el-table-column label="操作" width="80"><template #default="{ $index }"><el-button link type="danger" @click="voucherForm.entries.splice($index,1)">删</el-button></template></el-table-column>
      </el-table>
      <div class="dlg-actions">
        <el-button @click="voucherForm.entries.push({ accountCode: '', direction: 'DR', amount: 0, memo: '' })">添加分录</el-button>
        <span class="sum">借 {{ debitSum }} / 贷 {{ creditSum }}</span>
      </div>
      <template #footer><el-button @click="voucherVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitVoucher">保存</el-button></template>
    </el-dialog>

    <el-dialog v-model="voucherDetailVisible" :title="`凭证明细 ${voucherDetail.voucher?.voucherNo || ''}`" width="720px" destroy-on-close>
      <el-descriptions :column="2" size="small" border>
        <el-descriptions-item label="日期">{{ voucherDetail.voucher?.voucherDate }}</el-descriptions-item>
        <el-descriptions-item label="状态">{{ vStatus(voucherDetail.voucher?.status ?? 0).l }}</el-descriptions-item>
        <el-descriptions-item label="借方合计">{{ voucherDetail.voucher?.totalDebit }}</el-descriptions-item>
        <el-descriptions-item label="贷方合计">{{ voucherDetail.voucher?.totalCredit }}</el-descriptions-item>
      </el-descriptions>
      <el-table :data="voucherDetail.entries" border size="small" style="margin-top:12px">
        <el-table-column prop="entryNo" label="序号" width="70" />
        <el-table-column prop="accountCode" label="科目" width="140" />
        <el-table-column prop="direction" label="方向" width="80" />
        <el-table-column prop="amount" label="金额" width="120" align="right" />
        <el-table-column prop="memo" label="摘要" min-width="180" />
      </el-table>
    </el-dialog>

    <el-dialog v-model="writeoffVisible" :title="writeoffMode === 'ar' ? '核销收款' : '核销付款'" width="480px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item :label="writeoffMode === 'ar' ? '客户' : '供应商'"><el-input :model-value="writeoffMode === 'ar' ? (writeoffRow.customerName ?? '') : (writeoffRow.supplierName ?? '')" disabled /></el-form-item>
        <el-form-item label="单据号"><el-input :model-value="writeoffMode === 'ar' ? (writeoffRow.recvNo ?? '') : (writeoffRow.payNo ?? '')" disabled /></el-form-item>
        <el-form-item label="核销金额"><el-input-number v-model="writeoffAmount" :min="0.01" :precision="2" controls-position="right" style="width:100%" /></el-form-item>
        <el-form-item label="日期"><el-date-picker v-model="writeoffDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width:100%" /></el-form-item>
      </el-form>
      <template #footer><el-button @click="writeoffVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitWriteoff">确认核销</el-button></template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { ElMessage } from 'element-plus'
import { get, post } from '@/utils/request'
import { submitApproval } from '@/utils/submitApproval'

const tab = ref('voucher')
const saving = ref(false)
const today = new Date().toISOString().slice(0, 10)

const v = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function vLoad() { v.loading = true; try { const res: any = await get('/api/fin/voucher', { page: v.page, size: v.size, keyword: v.keyword }); v.total = res?.total ?? 0; v.rows = res?.rows ?? [] } finally { v.loading = false } }
const vSearch = () => { v.page = 1; vLoad() }
const vReset = () => { v.keyword = ''; vSearch() }
function vStatus(s: number) { return s === 0 ? { l: '草稿', t: 'info' } : s === 1 ? { l: '已审核', t: '' } : s === 2 ? { l: '已记账', t: 'warning' } : { l: '已结账', t: 'success' } }

const ar = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function arLoad() { ar.loading = true; try { const res: any = await get('/api/fin/receivable', { page: ar.page, size: ar.size, keyword: ar.keyword }); ar.total = res?.total ?? 0; ar.rows = res?.rows ?? [] } finally { ar.loading = false } }
const arSearch = () => { ar.page = 1; arLoad() }
const arReset = () => { ar.keyword = ''; arSearch() }

const ap = reactive({ keyword: '', page: 1, size: 20, total: 0, rows: [] as any[], loading: false })
async function apLoad() { ap.loading = true; try { const res: any = await get('/api/fin/payable', { page: ap.page, size: ap.size, keyword: ap.keyword }); ap.total = res?.total ?? 0; ap.rows = res?.rows ?? [] } finally { ap.loading = false } }
const apSearch = () => { ap.page = 1; apLoad() }
const apReset = () => { ap.keyword = ''; apSearch() }

const ageing = ref<any[]>([])
const ageingLoading = ref(false)
async function loadAgeing() { ageingLoading.value = true; try { ageing.value = (await get('/api/fin/ageing')) || [] } finally { ageingLoading.value = false } }

const costForm = reactive({ prOrderId: 1, laborCost: 0, overheadCost: 0 })
const costResult = ref<any>(null)

const voucherVisible = ref(false)
const voucherForm = reactive({ voucherDate: today, sourceModule: '手工', entries: [{ accountCode: '', direction: 'DR', amount: 0, memo: '' }] })
const debitSum = computed(() => voucherForm.entries.filter(e => e.direction === 'DR').reduce((s, e) => s + (e.amount || 0), 0))
const creditSum = computed(() => voucherForm.entries.filter(e => e.direction === 'CR').reduce((s, e) => s + (e.amount || 0), 0))

const voucherDetailVisible = ref(false)
const voucherDetail = reactive({ voucher: null as any, entries: [] as any[] })

const writeoffVisible = ref(false)
const writeoffMode = ref<'ar' | 'ap'>('ar')
const writeoffRow = reactive<any>({})
const writeoffAmount = ref(0)
const writeoffDate = ref(today)

function openVoucher() { voucherForm.voucherDate = today; voucherForm.sourceModule = '手工'; voucherForm.entries = [{ accountCode: '', direction: 'DR', amount: 0, memo: '' }]; voucherVisible.value = true }
async function openVoucherDetail(r: any) { try { const res: any = await get(`/api/fin/voucher/${r.id}`); voucherDetail.voucher = res.voucher; voucherDetail.entries = res.entries || []; voucherDetailVisible.value = true } catch {} }
function openWriteoff(r: any, mode: 'ar' | 'ap') { writeoffRow.id = r.id; writeoffRow.customerId = r.customerId; writeoffRow.supplierId = r.supplierId; writeoffMode.value = mode; writeoffAmount.value = (r.amount - (r.receivedAmount ?? r.paidAmount ?? 0)) || 0; writeoffDate.value = today; writeoffVisible.value = true }

async function submitVoucher() {
  if (!voucherForm.entries.length) return ElMessage.warning('请添加分录')
  if (Math.abs(debitSum.value - creditSum.value) > 0.001) return ElMessage.warning('借贷必须平衡')
  saving.value = true
  try {
    const res: any = await post('/api/fin/voucher', { ...voucherForm })
    const ok = await submitApproval({ bizType: 'VOUCHER', bizId: res?.id, billNo: res?.voucherNo, title: `记账凭证审批 ${res?.voucherNo ?? ''}`, amount: debitSum.value, remark: '手工凭证自动送审' })
    ElMessage.success(ok ? '凭证已保存，已提交审批中心审核' : '凭证已保存（审批单提交失败，请在审批流中手动提交）')
    voucherVisible.value = false; vLoad()
  } finally { saving.value = false }
}
async function act(url: string, msg: string, cb: () => void) { try { await post(url); ElMessage.success(msg); cb() } catch {} }
async function submitWriteoff() {
  saving.value = true
  try {
    if (writeoffMode.value === 'ar') {
      const res: any = await post('/api/fin/receipt', { customerId: writeoffRow.customerId, receivableId: writeoffRow.id, amount: writeoffAmount.value, receiptDate: writeoffDate.value })
      const ok = await submitApproval({ bizType: 'FIN_RECEIPT', bizId: res?.id, billNo: res?.receiptNo, title: `收款单审批 ${res?.receiptNo ?? ''}`, amount: writeoffAmount.value, remark: '收款核销自动送审' })
      ElMessage.success(ok ? '核销成功，已提交审批中心审核' : '核销成功（审批单提交失败，请在审批流中手动提交）')
    } else {
      const res: any = await post('/api/fin/payment', { supplierId: writeoffRow.supplierId, payableId: writeoffRow.id, amount: writeoffAmount.value, paymentDate: writeoffDate.value })
      const ok = await submitApproval({ bizType: 'FIN_PAYMENT', bizId: res?.id, billNo: res?.paymentNo, title: `付款单审批 ${res?.paymentNo ?? ''}`, amount: writeoffAmount.value, remark: '付款核销自动送审' })
      ElMessage.success(ok ? '核销成功，已提交审批中心审核' : '核销成功（审批单提交失败，请在审批流中手动提交）')
    }
    writeoffVisible.value = false; arLoad(); apLoad()
  } finally { saving.value = false }
}
async function calcCost() {
  if (!costForm.prOrderId) return ElMessage.warning('请填写生产订单ID')
  saving.value = true
  try { costResult.value = await post('/api/fin/cost', { ...costForm }); ElMessage.success('计算完成') } finally { saving.value = false }
}

onMounted(() => { vLoad(); arLoad(); apLoad(); loadAgeing() })
</script>

<style scoped>
.toolbar { display: flex; justify-content: space-between; margin-bottom: 12px; }
.left, .right { display: flex; gap: 8px; align-items: center; }
.pager { margin-top: 12px; justify-content: flex-end; }
.dlg-actions { margin-top: 10px; display: flex; align-items: center; gap: 16px; }
.sum { color: #909399; font-size: 13px; }
.cost-card { max-width: 920px; }
</style>
