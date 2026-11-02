<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <el-input v-model="keyword" placeholder="审批单号/标题" clearable style="width: 220px" @keyup.enter="search" />
        <el-select v-model="status" placeholder="全部状态" clearable style="width: 130px">
          <el-option label="草稿" :value="0" />
          <el-option label="审批中" :value="1" />
          <el-option label="已通过" :value="2" />
          <el-option label="已驳回" :value="3" />
        </el-select>
        <el-switch v-model="onlyMine" active-text="仅我可审批" style="margin-left:8px" />
        <el-button type="primary" @click="search">查询</el-button>
        <el-button @click="reset">重置</el-button>
      </div>
      <div class="right">
        <el-button type="primary" @click="openSubmit">提交审批</el-button>
        <el-button @click="load">刷新</el-button>
      </div>
    </div>

    <el-table :data="displayRows" v-loading="loading" border stripe size="small">
      <el-table-column prop="approvalNo" label="单号" width="160" />
      <el-table-column prop="title" label="标题" min-width="180" show-overflow-tooltip />
      <el-table-column prop="amount" label="金额" width="120" align="right" />
      <el-table-column prop="status" label="状态" width="100" align="center">
        <template #default="{ row }"><el-tag size="small" :type="aStatus(row.status).t">{{ aStatus(row.status).l }}</el-tag></template>
      </el-table-column>
      <el-table-column label="操作角色" min-width="150">
        <template #default="{ row }">
          <el-tag v-if="currentNodeRole(row)" size="small" type="warning">{{ roleText(currentNodeRole(row)) }}</el-tag>
          <span v-else>-</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="240" fixed="right">
        <template #default="{ row }">
          <el-button link type="primary" @click="openDetail(row)">详情</el-button>
          <el-button link type="success" :disabled="row.status !== 1" @click="openHandle(row, 2)">通过</el-button>
          <el-button link type="danger" :disabled="row.status !== 1" @click="openHandle(row, 3)">驳回</el-button>
          <el-button link type="info" :disabled="row.status !== 0 && row.status !== 3" @click="cancel(row)">撤回</el-button>
        </template>
      </el-table-column>
    </el-table>

    <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />

    <el-dialog v-model="submitVisible" title="提交审批单" width="560px" destroy-on-close>
      <el-form label-width="90px">
        <el-form-item label="审批标题"><el-input v-model="submitForm.title" placeholder="如 采购订单PO-xxx审批" /></el-form-item>
        <el-form-item label="业务类型">
          <el-select v-model="submitForm.bizType" filterable allow-create style="width:100%" placeholder="选择或输入业务类型，如 PO / SO">
            <el-option v-for="t in bizTypes" :key="t.value" :label="t.label" :value="t.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="业务单号"><el-input v-model="submitForm.billNo" /></el-form-item>
        <el-form-item label="金额"><el-input-number v-model="submitForm.amount" :min="0" :precision="2" controls-position="right" style="width:100%" /></el-form-item>
        <el-form-item label="备注"><el-input v-model="submitForm.remark" type="textarea" :rows="2" /></el-form-item>
      </el-form>
      <template #footer><el-button @click="submitVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submit">提交</el-button></template>
    </el-dialog>

    <el-dialog v-model="handleVisible" :title="handleAction === 2 ? '审批通过' : '审批驳回'" width="480px" destroy-on-close>
      <el-input v-model="handleOpinion" type="textarea" :rows="3" :placeholder="handleAction === 2 ? '审批意见（可选）' : '驳回理由'" />
      <template #footer><el-button @click="handleVisible=false">取消</el-button><el-button type="primary" :loading="saving" @click="submitHandle">确认</el-button></template>
    </el-dialog>

    <el-dialog v-model="detailVisible" title="审批单详情" width="680px" destroy-on-close>
      <el-descriptions :column="2" size="small" border>
        <el-descriptions-item label="单号">{{ detail.approval?.approvalNo }}</el-descriptions-item>
        <el-descriptions-item label="状态">{{ aStatus(detail.approval?.status ?? 0).l }}</el-descriptions-item>
        <el-descriptions-item label="业务类型">{{ detail.approval?.bizType }}</el-descriptions-item>
        <el-descriptions-item label="金额">{{ detail.approval?.amount }}</el-descriptions-item>
        <el-descriptions-item label="节点">{{ detail.approval?.currentNode }} / {{ detail.approval?.totalNodes }}</el-descriptions-item>
        <el-descriptions-item label="操作角色">{{ roleText(currentNodeRole(detail.approval)) || '—' }}</el-descriptions-item>
        <el-descriptions-item label="节点角色序列">{{ roleSeqText(detail.approval?.nodeRoles) }}</el-descriptions-item>
        <el-descriptions-item label="提交时间">{{ detail.approval?.submitTime }}</el-descriptions-item>
      </el-descriptions>
      <el-timeline style="margin-top:16px">
        <el-timeline-item v-for="(l, i) in detail.logs" :key="i" :type="l.action === 2 ? 'success' : l.action === 3 ? 'danger' : 'primary'">
          <div>节点{{ l.nodeNo }} · {{ actionText(l.action) }} · {{ l.approverName }}</div>
          <div style="color:#909399;font-size:12px">{{ l.opinion || '—' }}（{{ l.operTime }}）</div>
        </el-timeline-item>
      </el-timeline>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { ElMessage } from 'element-plus'
import { get, post } from '@/utils/request'
import { approvalBizTypes } from '@/constants/approval'

const bizTypes = approvalBizTypes

const roleMap = ref<Record<string, string>>({})
const roleFallback: Record<string, string> = { admin: '管理员', deptManager: '部门经理', finance: '财务', hr: '人事', warehouse: '仓管', purchaser: '采购', sales: '销售', manager: '总经理' }
function roleText(code?: string): string {
  if (!code) return ''
  return roleMap.value[code] || roleFallback[code] || code
}
function roleSeqText(nodeRoles?: string): string {
  if (!nodeRoles) return '—'
  return nodeRoles.split(',').map(x => roleText(x.trim())).filter(Boolean).join(' → ')
}
async function loadRoles() {
  try {
    const res: any = await get('/api/md/role', { page: 1, size: 200 })
    roleMap.value = Object.fromEntries((res?.rows ?? []).map((r: any) => [r.roleCode, r.roleName]))
  } catch {}
}

const keyword = ref('')
const status = ref<number | undefined>()
const onlyMine = ref(false)
const page = ref(1)
const size = ref(20)
const total = ref(0)
const rows = ref<any[]>([])
const loading = ref(false)

async function load() {
  loading.value = true
  try {
    const res: any = await get('/api/approval', { page: page.value, size: size.value, keyword: keyword.value, status: status.value ?? '' })
    total.value = res?.total ?? 0; rows.value = res?.rows ?? []
  } finally { loading.value = false }
}
const search = () => { page.value = 1; load() }
const reset = () => { keyword.value = ''; status.value = undefined; search() }
function aStatus(s: number) { return s === 0 ? { l: '草稿', t: 'info' } : s === 1 ? { l: '审批中', t: 'warning' } : s === 2 ? { l: '已通过', t: 'success' } : { l: '已驳回', t: 'danger' } }
function actionText(a: number) { return a === 1 ? '提交' : a === 2 ? '通过' : a === 3 ? '驳回' : a === 4 ? '撤回' : '操作' }

function myRole(): string {
  try {
    const t = localStorage.getItem('erp_token') || ''
    const p = JSON.parse(atob(t.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')))
    return p.role || ''
  } catch { return '' }
}
function currentNodeRole(row: any): string {
  if (!row?.nodeRoles) return ''
  const roles = (row.nodeRoles as string).split(',').map(x => x.trim()).filter(Boolean)
  const idx = row.currentNode ?? 0
  return roles.length > idx ? roles[idx] : (roles.length ? 'admin' : '')
}
const displayRows = computed(() => {
  if (!onlyMine.value) return rows.value
  const role = myRole()
  return rows.value.filter(r => {
    const need = currentNodeRole(r)
    return role === 'admin' || need === 'admin' || need === role
  })
})

const submitVisible = ref(false)
const submitForm = reactive({ title: '', bizType: '', billNo: '', amount: 0, totalNodes: 1, remark: '' })
const saving = ref(false)
function openSubmit() { submitForm.title = ''; submitForm.bizType = ''; submitForm.billNo = ''; submitForm.amount = 0; submitForm.totalNodes = 1; submitForm.remark = ''; submitVisible.value = true }
async function submit() {
  if (!submitForm.title) return ElMessage.warning('请填写标题')
  if (!submitForm.bizType) return ElMessage.warning('请选择业务类型')
  saving.value = true
  try {
    await post('/api/approval/submit', { title: submitForm.title, bizType: submitForm.bizType, billNo: submitForm.billNo, amount: submitForm.amount, remark: submitForm.remark })
    ElMessage.success('已提交，审批级数由规则自动决定'); submitVisible.value = false; load()
  } finally { saving.value = false }
}

const handleVisible = ref(false)
const handleAction = ref<number>(2)
const handleOpinion = ref('')
const currentId = ref<number>()
function openHandle(r: any, action: number) { currentId.value = r.id; handleAction.value = action; handleOpinion.value = ''; handleVisible.value = true }
async function submitHandle() {
  saving.value = true
  try { await post(`/api/approval/${currentId.value}/handle`, { action: handleAction.value, opinion: handleOpinion.value }); ElMessage.success('处理完成'); handleVisible.value = false; load() } finally { saving.value = false }
}
async function cancel(r: any) { try { await post(`/api/approval/${r.id}/cancel`, {}); ElMessage.success('已撤回'); load() } catch {} }

const detailVisible = ref(false)
const detail = reactive({ approval: null as any, logs: [] as any[] })
async function openDetail(r: any) { try { const res: any = await get(`/api/approval/${r.id}`); detail.approval = res.approval; detail.logs = res.logs || []; detailVisible.value = true } catch {} }

onMounted(() => { loadRoles(); load() })
</script>

<style scoped>
.toolbar { display: flex; justify-content: space-between; margin-bottom: 12px; }
.left, .right { display: flex; gap: 8px; align-items: center; }
.pager { margin-top: 12px; justify-content: flex-end; }
</style>
