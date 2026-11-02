<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <span style="font-weight: 600; font-size: 14px; color: #303133">低于安全库存的物料</span>
      </div>
      <div class="right">
        <el-button @click="load">刷新</el-button>
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border stripe size="small">
      <el-table-column prop="materialCode" label="物料编码" width="150" />
      <el-table-column prop="materialName" label="物料名称" min-width="180" show-overflow-tooltip />
      <el-table-column prop="warehouseId" label="仓库ID" width="110" />
      <el-table-column prop="currentQty" label="现存量" width="110" align="right">
        <template #default="{ row }">
          <span style="color: #f56c6c; font-weight: 600">{{ row.currentQty }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="safetyStock" label="安全库存" width="110" align="right" />
      <el-table-column label="缺口" width="110" align="right">
        <template #default="{ row }">
          <span style="color: #f56c6c">{{ (row.safetyStock - row.currentQty).toFixed(2) }}</span>
        </template>
      </el-table-column>
    </el-table>

    <el-empty v-if="!loading && rows.length === 0" description="暂无预警数据" />
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { get } from '@/utils/request'

interface AlertRow {
  materialCode: string
  materialName: string
  warehouseId: number
  currentQty: number
  safetyStock: number
}

const rows = ref<AlertRow[]>([])
const loading = ref(false)

async function load() {
  loading.value = true
  try {
    const res = await get<AlertRow[]>('/api/inv/alerts')
    rows.value = res ?? []
  } finally {
    loading.value = false
  }
}

onMounted(load)
</script>
