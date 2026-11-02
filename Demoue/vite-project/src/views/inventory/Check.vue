<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <el-input
          v-model="keyword"
          placeholder="盘点单号"
          clearable
          style="width: 220px"
          @keyup.enter="search"
        />
        <el-button type="primary" @click="search">查询</el-button>
        <el-button @click="reset">重置</el-button>
      </div>
      <div class="right">
        <el-button type="primary" @click="openCreate">新建盘点单</el-button>
        <el-button @click="load">刷新</el-button>
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border stripe size="small">
      <el-table-column prop="checkNo" label="盘点单号" width="170" />
      <el-table-column label="盘点仓库" min-width="140">
        <template #default="{ row }">{{ warehouseName(row.warehouseId) }}</template>
      </el-table-column>
      <el-table-column prop="checkDate" label="盘点日期" width="120" />
      <el-table-column label="状态" width="110" align="center">
        <template #default="{ row }">
          <el-tag size="small" :type="statusTag(row.status).t" effect="light">{{ statusTag(row.status).l }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="审批状态" width="110" align="center">
        <template #default="{ row }">
          <el-tag size="small" :type="auditTag(row.auditStatus).t" effect="light">{{ auditTag(row.auditStatus).l }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="220" fixed="right">
        <template #default="{ row }">
          <el-button link type="primary" :disabled="row.status !== 0" @click="openFinish(row)">录入实盘数</el-button>
          <el-button link type="info" @click="openDetail(row)">查看明细</el-button>
        </template>
      </el-table-column>
    </el-table>

    <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />

    <el-dialog v-model="createVisible" title="新建盘点单" width="480px">
      <el-form :model="createForm" label-width="90px">
        <el-form-item label="盘点仓库" required>
          <el-select v-model="createForm.warehouseId" placeholder="请选择仓库" style="width: 100%">
            <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
          </el-select>
        </el-form-item>
        <el-form-item label="盘点日期" required>
          <el-date-picker
            v-model="createForm.checkDate"
            type="date"
            value-format="YYYY-MM-DD"
            :teleported="false"
            placement="bottom-start"
            style="width: 100%"
          />
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="createForm.remark" type="textarea" :rows="2" placeholder="盘点说明" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="createVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitting" @click="submitCreate">创建</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="finishVisible" title="录入实盘数量" width="860px">
      <el-table :data="finishLines" border size="small" max-height="420">
        <el-table-column prop="materialCode" label="物料编码" width="130" />
        <el-table-column prop="materialName" label="物料名称" min-width="160" show-overflow-tooltip />
        <el-table-column prop="bookQty" label="账面数量" width="110" align="right" />
        <el-table-column label="实盘数量" width="170">
          <template #default="{ row }">
            <el-input-number v-model="row.actualQty" :min="0" :precision="4" controls-position="right" style="width: 100%" />
          </template>
        </el-table-column>
        <el-table-column label="差异" width="110" align="right">
          <template #default="{ row }">
            <span :class="diffClass(row)">{{ diffText(row) }}</span>
          </template>
        </el-table-column>
      </el-table>
      <template #footer>
        <el-button @click="finishVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitting" @click="submitFinish">提交并送审</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="detailVisible" title="盘点明细" width="760px">
      <el-descriptions :column="3" border size="small">
        <el-descriptions-item label="盘点单号">{{ detail.check?.checkNo ?? '-' }}</el-descriptions-item>
        <el-descriptions-item label="仓库">{{ warehouseName(detail.check?.warehouseId) }}</el-descriptions-item>
        <el-descriptions-item label="盘点日期">{{ detail.check?.checkDate ?? '-' }}</el-descriptions-item>
      </el-descriptions>
      <el-table :data="detail.details" border size="small" max-height="380" style="margin-top: 12px">
        <el-table-column prop="materialCode" label="物料编码" width="130" />
        <el-table-column prop="materialName" label="物料名称" min-width="160" show-overflow-tooltip />
        <el-table-column prop="bookQty" label="账面数量" width="110" align="right" />
        <el-table-column prop="actualQty" label="实盘数量" width="110" align="right">
          <template #default="{ row }">{{ row.actualQty ?? '-' }}</template>
        </el-table-column>
        <el-table-column prop="diffQty" label="差异" width="110" align="right">
          <template #default="{ row }">
            <span :class="diffClass(row)">{{ row.diffQty ?? '-' }}</span>
          </template>
        </el-table-column>
      </el-table>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { ElMessage } from 'element-plus'
import { get, post } from '@/utils/request'
import { submitApproval } from '@/utils/submitApproval'
import { usePageList, fetchOptions } from '@/composables/usePage'

interface CheckRow {
  id: number
  checkNo: string
  warehouseId: number
  checkDate: string
  status: number | null
  auditStatus: number | null
}
interface Warehouse {
  id: number
  warehouseName: string
}
interface DetailLine {
  materialId: number
  materialCode: string
  materialName: string
  bookQty: number | null
  actualQty: number | null
  diffQty: number | null
}

const keyword = ref('')
const submitting = ref(false)
const warehouses = ref<Warehouse[]>([])

const { total, rows, loading, page, size, load, search, reset } = usePageList<CheckRow>('/api/inv/check')

const createVisible = ref(false)
const createForm = reactive<{ warehouseId?: number; checkDate: string; remark: string }>({
  warehouseId: undefined,
  checkDate: new Date().toISOString().slice(0, 10),
  remark: ''
})

const finishVisible = ref(false)
const finishLines = ref<DetailLine[]>([])
const finishBill = ref<CheckRow | null>(null)

const detailVisible = ref(false)
const detail = reactive<{ check: CheckRow | null; details: DetailLine[] }>({ check: null, details: [] })

function warehouseName(id?: number) {
  return warehouses.value.find((w) => w.id === id)?.warehouseName ?? '-'
}
function statusTag(status?: number | null) {
  switch (status) {
    case 0: return { l: '盘点中', t: 'warning' as const }
    case 1: return { l: '已完成', t: 'primary' as const }
    case 2: return { l: '已审核', t: 'success' as const }
    default: return { l: '-', t: 'info' as const }
  }
}
function auditTag(status?: number | null) {
  switch (status) {
    case 0: return { l: '待审批', t: 'info' as const }
    case 1: return { l: '已通过', t: 'success' as const }
    case 2: return { l: '已驳回', t: 'danger' as const }
    default: return { l: '-', t: 'info' as const }
  }
}
function diffText(row: DetailLine) {
  const d = (row.actualQty ?? 0) - (row.bookQty ?? 0)
  if (d === 0) return '0'
  return d > 0 ? `+${d}` : `${d}`
}
function diffClass(row: DetailLine) {
  const d = (row.actualQty ?? 0) - (row.bookQty ?? 0)
  return d > 0 ? 'diff-up' : d < 0 ? 'diff-down' : ''
}

function openCreate() {
  createForm.warehouseId = undefined
  createForm.checkDate = new Date().toISOString().slice(0, 10)
  createForm.remark = ''
  createVisible.value = true
}

async function submitCreate() {
  if (!createForm.warehouseId) return ElMessage.warning('请选择盘点仓库')
  submitting.value = true
  try {
    await post('/api/inv/check', {
      warehouseId: createForm.warehouseId,
      checkDate: createForm.checkDate,
      remark: createForm.remark
    })
    ElMessage.success('盘点单已创建，请录入实盘数量后提交审批')
    createVisible.value = false
    load()
  } catch {
  } finally {
    submitting.value = false
  }
}

async function openFinish(row: CheckRow) {
  const res: any = await get(`/api/inv/check/${row.id}`)
  finishBill.value = row
  finishLines.value = (res?.details ?? []).map((d: DetailLine) => ({
    ...d,
    actualQty: d.bookQty ?? 0
  }))
  finishVisible.value = true
}

async function submitFinish() {
  const bill = finishBill.value
  if (!bill) return
  submitting.value = true
  try {
    await post('/api/inv/check/finish', {
      checkId: bill.id,
      lines: finishLines.value.map((l) => ({ materialId: l.materialId, actualQty: l.actualQty ?? 0 }))
    })
    const ok = await submitApproval({
      bizType: 'CHECK',
      bizId: bill.id,
      billNo: bill.checkNo,
      title: `库存盘点审批 ${bill.checkNo}`,
      remark: '盘盈盘亏调整送审，审批通过后自动调整库存'
    })
    ElMessage.success(ok ? '盘点结果已提交，已送审批中心审核' : '盘点结果已提交（审批单提交失败，请在审批流中手动提交）')
    finishVisible.value = false
    load()
  } catch {
  } finally {
    submitting.value = false
  }
}

async function openDetail(row: CheckRow) {
  const res: any = await get(`/api/inv/check/${row.id}`)
  detail.check = res?.check ?? row
  detail.details = res?.details ?? []
  detailVisible.value = true
}

onMounted(async () => {
  warehouses.value = await fetchOptions<Warehouse>('warehouse')
  load()
})
</script>

<style scoped>
.diff-up {
  color: #f56c6c;
  font-weight: 600;
}
.diff-down {
  color: #67c23a;
  font-weight: 600;
}
</style>
