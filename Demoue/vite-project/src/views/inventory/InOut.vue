<template>
  <div class="page-card">
    <el-tabs v-model="tab">
      <el-tab-pane label="入库单" name="in">
        <el-form :model="inForm" label-width="100px" class="form">
          <el-row :gutter="16">
            <el-col :span="8">
              <el-form-item label="入库类型">
                <el-select v-model="inForm.inType" style="width: 100%">
                  <el-option label="采购入库" value="PO" />
                  <el-option label="生产入库" value="PR" />
                  <el-option label="其它入库" value="OTHER" />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="来源单号">
                <el-input v-model="inForm.sourceNo" placeholder="采购/生产单号" />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="入库仓库">
                <el-select v-model="inForm.warehouseId" style="width: 100%">
                  <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="入库日期">
                <el-date-picker v-model="inForm.inDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width: 100%" />
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>

        <el-table :data="inForm.lines" border size="small" class="lines">
          <el-table-column label="物料" min-width="240">
            <template #default="{ row }">
              <el-select v-model="row.materialId" filterable placeholder="选择物料" style="width: 100%">
                <el-option
                  v-for="m in materials"
                  :key="m.id"
                  :label="`${m.materialCode} ${m.materialName}`"
                  :value="m.id"
                />
              </el-select>
            </template>
          </el-table-column>
          <el-table-column label="数量" width="180">
            <template #default="{ row }">
              <el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width: 100%" />
            </template>
          </el-table-column>
          <el-table-column label="操作" width="90" align="center">
            <template #default="{ $index }">
              <el-button type="danger" link @click="inForm.lines.splice($index, 1)">删除</el-button>
            </template>
          </el-table-column>
        </el-table>

        <div class="actions">
          <el-button @click="inForm.lines.push({ materialId: undefined as any, quantity: 1 })">添加明细</el-button>
          <el-button type="primary" :loading="submitting" @click="submitIn">确认入库</el-button>
        </div>
      </el-tab-pane>

      <el-tab-pane label="出库单" name="out">
        <el-form :model="outForm" label-width="100px" class="form">
          <el-row :gutter="16">
            <el-col :span="8">
              <el-form-item label="出库类型">
                <el-select v-model="outForm.outType" style="width: 100%">
                  <el-option label="销售出库" value="SO" />
                  <el-option label="生产领料" value="PR" />
                  <el-option label="其它出库" value="OTHER" />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="来源单号">
                <el-input v-model="outForm.sourceNo" placeholder="销售/生产单号" />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="出库仓库">
                <el-select v-model="outForm.warehouseId" style="width: 100%">
                  <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="出库日期">
                <el-date-picker v-model="outForm.outDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width: 100%" />
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>

        <el-table :data="outForm.lines" border size="small" class="lines">
          <el-table-column label="物料" min-width="240">
            <template #default="{ row }">
              <el-select v-model="row.materialId" filterable placeholder="选择物料" style="width: 100%">
                <el-option
                  v-for="m in materials"
                  :key="m.id"
                  :label="`${m.materialCode} ${m.materialName}`"
                  :value="m.id"
                />
              </el-select>
            </template>
          </el-table-column>
          <el-table-column label="数量" width="180">
            <template #default="{ row }">
              <el-input-number v-model="row.quantity" :min="0.0001" :precision="4" controls-position="right" style="width: 100%" />
            </template>
          </el-table-column>
          <el-table-column label="操作" width="90" align="center">
            <template #default="{ $index }">
              <el-button type="danger" link @click="outForm.lines.splice($index, 1)">删除</el-button>
            </template>
          </el-table-column>
        </el-table>

        <div class="actions">
          <el-button @click="outForm.lines.push({ materialId: undefined as any, quantity: 1 })">添加明细</el-button>
          <el-button type="primary" :loading="submitting" @click="submitOut">确认出库</el-button>
        </div>
      </el-tab-pane>

      <el-tab-pane label="调拨单" name="transfer">
        <el-form :model="transferForm" label-width="100px" class="form" style="max-width: 720px">
          <el-form-item label="物料">
            <el-select v-model="transferForm.materialId" filterable placeholder="选择物料" style="width: 100%">
              <el-option
                v-for="m in materials"
                :key="m.id"
                :label="`${m.materialCode} ${m.materialName}`"
                :value="m.id"
              />
            </el-select>
          </el-form-item>
          <el-form-item label="调出仓库">
            <el-select v-model="transferForm.fromWarehouseId" style="width: 100%">
              <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
            </el-select>
          </el-form-item>
          <el-form-item label="调入仓库">
            <el-select v-model="transferForm.toWarehouseId" style="width: 100%">
              <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
            </el-select>
          </el-form-item>
          <el-form-item label="调拨数量">
            <el-input-number v-model="transferForm.quantity" :min="0.0001" :precision="4" controls-position="right" />
          </el-form-item>
          <el-form-item label="调拨日期">
            <el-date-picker v-model="transferForm.transferDate" type="date" value-format="YYYY-MM-DD" :teleported="false" placement="bottom-start" style="width: 220px" />
          </el-form-item>
          <el-form-item>
            <el-button type="primary" :loading="submitting" @click="submitTransfer">确认调拨</el-button>
          </el-form-item>
        </el-form>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { ElMessage } from 'element-plus'
import { post } from '@/utils/request'
import { submitApproval } from '@/utils/submitApproval'
import { fetchOptions } from '@/composables/usePage'

interface Material {
  id: number
  materialCode: string
  materialName: string
}
interface Warehouse {
  id: number
  warehouseName: string
}
interface Line {
  materialId?: number
  quantity: number
}

const tab = ref('in')
const submitting = ref(false)
const materials = ref<Material[]>([])
const warehouses = ref<Warehouse[]>([])

const today = new Date().toISOString().slice(0, 10)

const inForm = reactive<{ inType: string; sourceNo: string; warehouseId?: number; inDate: string; lines: Line[] }>({
  inType: 'PO',
  sourceNo: '',
  warehouseId: undefined,
  inDate: today,
  lines: [{ materialId: undefined, quantity: 1 }]
})

const outForm = reactive<{ outType: string; sourceNo: string; warehouseId?: number; outDate: string; lines: Line[] }>({
  outType: 'SO',
  sourceNo: '',
  warehouseId: undefined,
  outDate: today,
  lines: [{ materialId: undefined, quantity: 1 }]
})

const transferForm = reactive({
  materialId: undefined as number | undefined,
  fromWarehouseId: undefined as number | undefined,
  toWarehouseId: undefined as number | undefined,
  quantity: 1,
  transferDate: today
})

function validateIn() {
  if (!inForm.warehouseId) return '请选择仓库'
  if (!inForm.lines.length) return '请添加明细'
  if (inForm.lines.some((l) => !l.materialId || l.quantity <= 0)) return '明细物料与数量必须填写'
  return ''
}
function validateOut() {
  if (!outForm.warehouseId) return '请选择仓库'
  if (!outForm.lines.length) return '请添加明细'
  if (outForm.lines.some((l) => !l.materialId || l.quantity <= 0)) return '明细物料与数量必须填写'
  return ''
}

async function submitIn() {
  const err = validateIn()
  if (err) return ElMessage.warning(err)
  submitting.value = true
  try {
    const res: any = await post('/api/inv/in', {
      inType: inForm.inType,
      sourceNo: inForm.sourceNo || null,
      warehouseId: inForm.warehouseId,
      inDate: inForm.inDate,
      lines: inForm.lines.map((l) => ({ materialId: l.materialId, quantity: l.quantity }))
    })
    const ok = await submitApproval({ bizType: 'IN_ORDER', bizId: res?.id, billNo: res?.inNo, title: `入库单审批 ${res?.inNo ?? ''}`, remark: '入库单自动送审' })
    ElMessage.success(ok ? '入库成功，已提交审批中心审核' : '入库成功（审批单提交失败，请在审批流中手动提交）')
    inForm.lines = [{ materialId: undefined, quantity: 1 }]
  } catch {
  } finally {
    submitting.value = false
  }
}

async function submitOut() {
  const err = validateOut()
  if (err) return ElMessage.warning(err)
  submitting.value = true
  try {
    const res: any = await post('/api/inv/out', {
      outType: outForm.outType,
      sourceNo: outForm.sourceNo || null,
      warehouseId: outForm.warehouseId,
      outDate: outForm.outDate,
      lines: outForm.lines.map((l) => ({ materialId: l.materialId, quantity: l.quantity }))
    })
    const ok = await submitApproval({ bizType: 'OUT_ORDER', bizId: res?.id, billNo: res?.outNo, title: `出库单审批 ${res?.outNo ?? ''}`, remark: '出库单自动送审' })
    ElMessage.success(ok ? '出库成功，已提交审批中心审核' : '出库成功（审批单提交失败，请在审批流中手动提交）')
    outForm.lines = [{ materialId: undefined, quantity: 1 }]
  } catch {
  } finally {
    submitting.value = false
  }
}

async function submitTransfer() {
  if (!transferForm.materialId) return ElMessage.warning('请选择物料')
  if (!transferForm.fromWarehouseId || !transferForm.toWarehouseId)
    return ElMessage.warning('请选择调出/调入仓库')
  if (transferForm.fromWarehouseId === transferForm.toWarehouseId)
    return ElMessage.warning('调出与调入仓库不能相同')
  if (transferForm.quantity <= 0) return ElMessage.warning('调拨数量必须大于0')

  submitting.value = true
  try {
    const res: any = await post('/api/inv/transfer', {
      materialId: transferForm.materialId,
      fromWarehouseId: transferForm.fromWarehouseId,
      toWarehouseId: transferForm.toWarehouseId,
      quantity: transferForm.quantity,
      transferDate: transferForm.transferDate
    })
    const ok = await submitApproval({ bizType: 'TRANSFER', bizId: res?.id, billNo: res?.transferNo, title: `调拨单审批 ${res?.transferNo ?? ''}`, remark: '调拨单自动送审' })
    ElMessage.success(ok ? '调拨成功，已提交审批中心审核' : '调拨成功（审批单提交失败，请在审批流中手动提交）')
  } catch {
  } finally {
    submitting.value = false
  }
}

onMounted(async () => {
  materials.value = await fetchOptions<Material>('material')
  warehouses.value = await fetchOptions<Warehouse>('warehouse')
})
</script>

<style scoped>
.lines {
  margin-top: 4px;
}
.actions {
  margin-top: 14px;
  display: flex;
  gap: 8px;
}
.form :deep(.el-form-item) {
  margin-bottom: 16px;
}
</style>
