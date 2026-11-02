<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <el-input
          v-model="keyword"
          placeholder="关键字搜索"
          clearable
          style="width: 220px"
          @keyup.enter="search"
        />
        <el-button type="primary" @click="search">查询</el-button>
        <el-button @click="reset">重置</el-button>
      </div>
      <div class="right">
        <el-button type="primary" @click="openCreate">新增</el-button>
        <el-button @click="load">刷新</el-button>
        <el-button
          v-for="act in biz.toolbarActions"
          :key="act.label"
          type="warning"
          :loading="bizLoading === act.url"
          @click="runToolbar(act)"
        >{{ act.label }}</el-button>
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border stripe size="small">
      <el-table-column
        v-for="c in visibleColumns"
        :key="c.prop"
        :prop="c.prop"
        :label="c.label"
        :width="c.width"
        :min-width="c.minWidth"
        show-overflow-tooltip
      >
        <template #default="{ row }">
          <el-tag
            v-if="enumOf(c, row)"
            :type="tagType(enumOf(c, row)!.v)"
            size="small"
            effect="light"
          >{{ enumOf(c, row)!.t }}</el-tag>
          <template v-else>{{ cellText(c, row) }}</template>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="150" fixed="right" align="center">
        <template #default="{ row }">
          <el-button type="primary" link @click="openEdit(row)">编辑</el-button>
          <el-button
            v-for="act in biz.rowActions"
            :key="act.label"
            type="success"
            link
            :loading="bizLoading === act.url.replace('{id}', row.id)"
            @click="runRow(act, row)"
          >{{ act.label }}</el-button>
          <el-button type="danger" link @click="remove(row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />

    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="560px" destroy-on-close>
      <el-form ref="formRef" :model="form" label-width="120px">
        <el-form-item
          v-for="f in fields"
          :key="f.prop"
          :label="f.label"
          :prop="f.prop"
          :rules="f.required ? [{ required: true, message: `请填写${f.label}`, trigger: 'blur' }] : undefined"
        >
          <el-radio-group
            v-if="f.options?.length"
            v-model="form[f.prop]"
            size="small"
          >
            <el-radio-button
              v-for="o in f.options"
              :key="o.value"
              :label="o.value"
            >{{ o.label }}</el-radio-button>
          </el-radio-group>
          <el-select
            v-else-if="f.ref && refOptions(f.ref)"
            v-model="form[f.prop]"
            filterable
            placeholder="请选择"
            style="width: 100%"
          >
            <el-option
              v-for="o in refOptions(f.ref)!"
              :key="o.value"
              :label="o.label"
              :value="o.value"
            />
          </el-select>
          <el-input-number
            v-else-if="f.type === 'number'"
            v-model="form[f.prop]"
            :precision="f.precision ?? 2"
            controls-position="right"
            style="width: 100%"
          />
          <el-input v-else v-model="form[f.prop]" :placeholder="`请输入${f.label}`" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="saving" @click="submit">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { FormInstance } from 'element-plus'
import { usePageList } from '@/composables/usePage'
import { get, post, put, del } from '@/utils/request'
import { hrBizActions, type HrRowAction, type HrToolbarAction } from '@/views/hr/hrBizActions'

interface Column {
  prop: string
  label: string
  width?: number
  minWidth?: number
  enums?: Record<number, string>
  ref?: { resource: string; field: string; prefix?: string }
}
interface Field {
  prop: string
  label: string
  type?: 'text' | 'number'
  precision?: number
  required?: boolean
  options?: { label: string; value: number }[]
  ref?: { resource: string; field: string; prefix?: string }
}

const refMaps: Record<string, Record<number, string>> = {}

const props = defineProps<{
  resource: string
  columns: Column[]
  fields: Field[]
}>()

const { keyword, total, rows, loading, page, size, load, search, reset } = usePageList<any>(
  `/api/hr/${props.resource}`
)

const visibleColumns = computed(() => props.columns.filter((c) => c.prop !== 'id'))

const refLoaded = ref(0)

function enumOf(c: Column, row: any): { v: number; t: string } | null {
  if (!c.enums || c.ref) return null
  const v = row[c.prop]
  if (v === null || v === undefined) return null
  const t = c.enums[v as number]
  return t ? { v: v as number, t } : null
}

const TAG_PALETTE = ['info', 'success', 'primary', 'warning', 'danger'] as const
function tagType(v: number) {
  return TAG_PALETTE[((v % 5) + 5) % 5]
}

function cellText(c: Column, row: any) {
  const val = row[c.prop]
  if (c.ref && val !== null && val !== undefined && val !== '') {
    const m = refMaps[`${c.ref.prefix ?? 'hr'}:${c.ref.resource}:${c.ref.field}`]
    refLoaded.value // 建立响应依赖，引用数据到达后自动刷新显示
    const name = m?.[Number(val)]
    if (name !== undefined && name !== '') return name
    if (c.enums && String(c.enums[val as number] ?? '') !== '') return c.enums[val as number]
    return val === 0 ? '-' : val
  }
  if (c.enums && val !== null && val !== undefined && String(c.enums[val as number] ?? '') !== '') {
    return c.enums[val as number]
  }
  return val ?? ''
}

function refOptions(r: { resource: string; field: string; prefix?: string }) {
  refLoaded.value
  const m = refMaps[`${r.prefix ?? 'hr'}:${r.resource}:${r.field}`]
  if (!m) return null
  return Object.entries(m).map(([k, v]) => ({ value: Number(k), label: v }))
}

async function ensureRefs() {
  const need = new Set<string>()
  props.columns.forEach((c) => c.ref && need.add(`${c.ref.prefix ?? 'hr'}:${c.ref.resource}:${c.ref.field}`))
  props.fields.forEach((f) => f.ref && need.add(`${f.ref.prefix ?? 'hr'}:${f.ref.resource}:${f.ref.field}`))
  for (const key of need) {
    if (refMaps[key]) continue
    const [prefix, resource, field] = key.split(':')
    try {
      const url = '/' + ['api', prefix, resource].filter(Boolean).join('/')
      const res: any = await get(`${url}?page=1&size=1000`)
      const list = res?.data?.rows ?? res?.rows ?? []
      const m: Record<number, string> = {}
      list.forEach((x: any) => { m[Number(x.id)] = String(x[field] ?? '') })
      refMaps[key] = m
    } catch {
      refMaps[key] = {}
    }
  }
  refLoaded.value++
}

const biz = hrBizActions[props.resource] || {}

const dialogVisible = ref(false)
const dialogTitle = ref('新增')
const saving = ref(false)
const formRef = ref<FormInstance>()
const form = reactive<Record<string, any>>({})
let currentRow: any = null

function defaults() {
  const d: Record<string, any> = {}
  props.fields.forEach((f) => {
    d[f.prop] = f.ref ? undefined : f.type === 'number' ? 0 : ''
  })
  return d
}

function openCreate() {
  currentRow = null
  dialogTitle.value = '新增'
  Object.keys(form).forEach((k) => delete form[k])
  Object.assign(form, defaults())
  dialogVisible.value = true
}

function openEdit(row: any) {
  currentRow = row
  dialogTitle.value = '编辑'
  Object.keys(form).forEach((k) => delete form[k])
  Object.assign(form, row)
  dialogVisible.value = true
}

async function submit() {
  const valid = await formRef.value?.validate().catch(() => false)
  if (!valid) return
  saving.value = true
  try {
    const payload: Record<string, any> = { ...(currentRow || {}), ...form }
    Object.keys(payload).forEach((k) => payload[k] === undefined && delete payload[k])
    if (currentRow) {
      await put(`/api/hr/${props.resource}/${currentRow.id}`, payload)
    } else {
      await post(`/api/hr/${props.resource}`, payload)
    }
    ElMessage.success('保存成功')
    dialogVisible.value = false
    load()
  } catch {
  } finally {
    saving.value = false
  }
}

async function remove(row: any) {
  try {
    await ElMessageBox.confirm('确认删除该条记录？', '提示', { type: 'warning' })
  } catch {
    return
  }
  try {
    await del(`/api/hr/${props.resource}/${row.id}`)
    ElMessage.success('删除成功')
    load()
  } catch {
  }
}

const bizLoading = ref('')
async function runRow(act: HrRowAction, row: any) {
  if (act.confirm) {
    try {
      await ElMessageBox.confirm(`确认执行「${act.label}」？`, '提示', { type: 'warning' })
    } catch {
      return
    }
  }
  const url = act.url.replace('{id}', String(row.id))
  bizLoading.value = url
  try {
    if (act.method === 'put') await put(url)
    else await post(url)
    ElMessage.success('操作成功')
    load()
  } catch {
  } finally {
    bizLoading.value = ''
  }
}

async function runToolbar(act: HrToolbarAction) {
  bizLoading.value = act.url
  try {
    await post(act.url, act.body || {})
    ElMessage.success('操作成功')
    load()
  } catch {
  } finally {
    bizLoading.value = ''
  }
}

onMounted(() => { load(); ensureRefs() })
</script>

<style scoped>
.toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}
.left,
.right {
  display: flex;
  gap: 8px;
  align-items: center;
}
</style>
