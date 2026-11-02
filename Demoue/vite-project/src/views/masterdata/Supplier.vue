<template>
  <MdCrud entity="supplier" :columns="columns" :fields="fields" keyword-placeholder="供应商编码 / 名称 / 联系人" />
</template>

<script setup lang="ts">
import MdCrud, { Field } from '@/components/MdCrud.vue'

const columns = [
  { prop: 'supplierCode', label: '供应商编码', width: 140 },
  { prop: 'supplierName', label: '供应商名称', minWidth: 180 },
  { prop: 'contactPerson', label: '联系人', width: 110 },
  { prop: 'phone', label: '联系电话', width: 140 },
  { prop: 'address', label: '地址', minWidth: 200 },
  { prop: 'status', label: '状态', width: 90, formatter: (r: any) => (r.status === 1 ? '启用' : '停用') }
]

const fields: Field[] = [
  { prop: 'supplierCode', label: '供应商编码', type: 'text', required: true },
  { prop: 'supplierName', label: '供应商名称', type: 'text', required: true },
  { prop: 'contactPerson', label: '联系人', type: 'text' },
  { prop: 'phone', label: '联系电话', type: 'text' },
  { prop: 'address', label: '地址', type: 'text' },
  { prop: 'status', label: '启用状态', type: 'switch' }
]
</script>
