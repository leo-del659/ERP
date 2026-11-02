<template>
  <MdCrud entity="warehouse" :columns="columns" :fields="fields" keyword-placeholder="仓库编码 / 名称" />
</template>

<script setup lang="ts">
import MdCrud, { Field } from '@/components/MdCrud.vue'

const columns = [
  { prop: 'warehouseCode', label: '仓库编码', width: 140 },
  { prop: 'warehouseName', label: '仓库名称', minWidth: 180 },
  { prop: 'location', label: '仓库位置', minWidth: 220 },
  { prop: 'status', label: '状态', width: 90, formatter: (r: any) => (r.status === 1 ? '启用' : '停用') }
]

const fields: Field[] = [
  { prop: 'warehouseCode', label: '仓库编码', type: 'text', required: true },
  { prop: 'warehouseName', label: '仓库名称', type: 'text', required: true },
  { prop: 'location', label: '仓库位置', type: 'text' },
  { prop: 'status', label: '启用状态', type: 'switch' }
]
</script>
