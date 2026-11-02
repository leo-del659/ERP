<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <el-input
          v-model="keyword"
          placeholder="物料编码 / 名称"
          clearable
          style="width: 220px"
          @keyup.enter="search"
        />
        <el-select v-model="warehouseId" placeholder="全部仓库" clearable style="width: 180px">
          <el-option v-for="w in warehouses" :key="w.id" :label="w.warehouseName" :value="w.id" />
        </el-select>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button @click="reset">重置</el-button>
      </div>
      <div class="right">
        <el-button @click="load">刷新</el-button>
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border stripe size="small">
      <el-table-column prop="materialCode" label="物料编码" width="140" />
      <el-table-column prop="materialName" label="物料名称" min-width="160" show-overflow-tooltip />
      <el-table-column prop="warehouseName" label="仓库" width="140" />
      <el-table-column prop="currentQty" label="现存量" width="110" align="right" sortable />
      <el-table-column prop="lockedQty" label="锁定量" width="100" align="right" />
      <el-table-column prop="safetyStock" label="安全库存" width="110" align="right">
        <template #default="{ row }">
          <span v-if="row.safetyStock == null">-</span>
          <el-tag
            v-else
            :type="row.currentQty < row.safetyStock ? 'danger' : 'success'"
            size="small"
            effect="light"
          >
            {{ row.safetyStock }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="lastUpdate" label="更新时间" width="170">
        <template #default="{ row }">{{ row.lastUpdate?.replace('T', ' ').slice(0, 19) ?? '-' }}</template>
      </el-table-column>
    </el-table>

    <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { usePageList, fetchOptions } from '@/composables/usePage'

interface StockRow {
  id: number
  materialId: number
  materialCode: string
  materialName: string
  warehouseId: number
  warehouseName: string
  currentQty: number
  lockedQty: number
  safetyStock: number | null
  lastUpdate: string
}

interface Warehouse {
  id: number
  warehouseCode: string
  warehouseName: string
}

const warehouseId = ref<number | undefined>()
const warehouses = ref<Warehouse[]>([])

const { keyword, total, rows, loading, page, size, load, search, reset } = usePageList<StockRow>(
  '/api/inv/stock',
  () => ({ warehouseId: warehouseId.value })
)

onMounted(async () => {
  warehouses.value = await fetchOptions<Warehouse>('warehouse')
  load()
})
</script>

<style scoped>
.pager {
  margin-top: 12px;
  justify-content: flex-end;
}
</style>
