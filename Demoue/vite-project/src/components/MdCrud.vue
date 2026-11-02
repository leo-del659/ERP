<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <el-input
          v-model="keyword"
          :placeholder="keywordPlaceholder || '关键字搜索'"
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
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border stripe size="small">
      <el-table-column
        v-for="c in columns"
        :key="c.prop"
        :prop="c.prop"
        :label="c.label"
        :width="c.width"
        :min-width="c.minWidth"
        :formatter="c.formatter"
        show-overflow-tooltip
      />
      <el-table-column label="操作" width="150" fixed="right" align="center">
        <template #default="{ row }">
          <el-button type="primary" link @click="openEdit(row)">编辑</el-button>
          <el-button type="danger" link @click="remove(row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />

    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="560px" destroy-on-close>
      <el-form ref="formRef" :model="form" label-width="110px">
        <el-form-item
          v-for="f in fields"
          :key="f.prop"
          :label="f.label"
          :prop="f.prop"
          :rules="f.required ? [{ required: true, message: `请填写${f.label}`, trigger: 'blur' }] : undefined"
        >
          <el-select v-if="f.type === 'select'" v-model="form[f.prop]" style="width: 100%">
            <el-option v-for="o in f.options" :key="o.value" :label="o.label" :value="o.value" />
          </el-select>
          <el-input-number
            v-else-if="f.type === 'number'"
            v-model="form[f.prop]"
            :precision="f.precision ?? 2"
            controls-position="right"
            style="width: 100%"
          />
          <el-switch
            v-else-if="f.type === 'switch'"
            v-model="form[f.prop]"
            :active-value="1"
            :inactive-value="0"
          />
          <el-input v-else v-model="form[f.prop]" :placeholder="f.placeholder" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="saving" @click="submit">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts">
export interface Field {
  prop: string
  label: string
  type?: 'text' | 'number' | 'select' | 'switch'
  required?: boolean
  precision?: number
  placeholder?: string
  options?: { label: string; value: any }[]
}
</script>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import type { FormInstance } from 'element-plus'
import { usePageList } from '@/composables/usePage'
import { post, del } from '@/utils/request'

interface Column {
  prop: string
  label: string
  width?: number
  minWidth?: number
  formatter?: (row: any) => string
}

const props = defineProps<{
  entity: string
  columns: Column[]
  fields: Field[]
  keywordPlaceholder?: string
}>()

const { keyword, total, rows, loading, page, size, load, search, reset } = usePageList<any>(
  `/api/md/${props.entity}`
)

const dialogVisible = ref(false)
const dialogTitle = ref('新增')
const saving = ref(false)
const formRef = ref<FormInstance>()
const form = reactive<Record<string, any>>({})
let currentRow: any = null

function defaults() {
  const d: Record<string, any> = {}
  props.fields.forEach((f) => {
    if (f.type === 'switch') d[f.prop] = 1
    else if (f.type === 'number') d[f.prop] = f.precision === 0 ? 0 : 0
    else d[f.prop] = ''
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
    const payload: Record<string, any> = currentRow
      ? { ...currentRow, ...form }
      : { ...form }
    Object.keys(payload).forEach((k) => payload[k] === undefined && delete payload[k])
    await post(`/api/md/${props.entity}`, payload)
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
    await ElMessageBox.confirm(`确认删除该条记录？`, '提示', { type: 'warning' })
  } catch {
    return
  }
  try {
    await del(`/api/md/${props.entity}/${row.id}`)
    ElMessage.success('删除成功')
    load()
  } catch {
  }
}

onMounted(load)
</script>

<style scoped>
.pager {
  margin-top: 12px;
  justify-content: flex-end;
}
</style>
