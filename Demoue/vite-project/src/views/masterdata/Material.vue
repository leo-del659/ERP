<template>
  <MdCrud entity="material" :columns="columns" :fields="fields" keyword-placeholder="物料编码 / 名称 / 规格" />
</template>

<script setup lang="ts">
import MdCrud, { Field } from '@/components/MdCrud.vue'

const columns = [
  { prop: 'materialCode', label: '物料编码', width: 140 },
  { prop: 'materialName', label: '物料名称', minWidth: 180 },
  { prop: 'spec', label: '规格型号', width: 150 },
  { prop: 'unit', label: '单位', width: 80 },
  { prop: 'category', label: '分类', width: 100 },
  { prop: 'safetyStock', label: '安全库存', width: 110, formatter: (r: any) => r.safetyStock ?? '-' },
  { prop: 'status', label: '状态', width: 90, formatter: (r: any) => (r.status === 1 ? '启用' : '停用') }
]

const fields: Field[] = [
  { prop: 'materialCode', label: '物料编码', type: 'text', required: true },
  { prop: 'materialName', label: '物料名称', type: 'text', required: true },
  { prop: 'spec', label: '规格型号', type: 'text' },
  { prop: 'unit', label: '计量单位', type: 'text', placeholder: '如 PCS / KG' },
  {
    prop: 'category',
    label: '物料分类',
    type: 'select',
    options: [
      { label: '成品', value: '成品' },
      { label: '半成品', value: '半成品' },
      { label: '原材料', value: '原材料' },
      { label: '辅料', value: '辅料' }
    ]
  },
  { prop: 'safetyStock', label: '安全库存', type: 'number', precision: 4 },
  { prop: 'status', label: '启用状态', type: 'switch' }
]
</script>
