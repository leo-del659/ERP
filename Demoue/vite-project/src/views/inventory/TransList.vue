<template>
  <div class="page-card">
    <div class="toolbar">
      <div class="left">
        <el-input
          v-model="keyword"
          placeholder="流水单号 / 来源单号"
          clearable
          style="width: 220px"
          @keyup.enter="search"
        />
        <el-button type="primary" @click="search">查询</el-button>
        <el-button @click="reset">重置</el-button>
      </div>
      <div class="right">
        <el-button @click="load">刷新</el-button>
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border stripe size="small">
      <el-table-column prop="transNo" label="流水单号" width="170" />
      <el-table-column prop="transType" label="业务类型" width="120">
        <template #default="{ row }">
          <el-tag size="small" :type="typeTag(row.transType)">{{ typeText(row.transType) }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="direction" label="方向" width="90" align="center">
        <template #default="{ row }">
          <span :style="{ color: row.direction > 0 ? '#f56c6c' : '#67c23a', fontWeight: 600 }">
            {{ row.direction > 0 ? '入库' : '出库' }}
          </span>
        </template>
      </el-table-column>
      <el-table-column prop="materialName" label="物料" min-width="160" show-overflow-tooltip />
      <el-table-column prop="warehouseName" label="仓库" min-width="140" show-overflow-tooltip />
      <el-table-column prop="quantity" label="变动数量" width="110" align="right" />
      <el-table-column prop="beforeQty" label="变动前" width="100" align="right" />
      <el-table-column prop="afterQty" label="变动后" width="100" align="right" />
      <el-table-column prop="sourceNo" label="来源单据" width="170" show-overflow-tooltip />
      <el-table-column prop="transTime" label="发生时间" width="170">
        <template #default="{ row }">{{ row.transTime?.replace('T', ' ').slice(0, 19) ?? '-' }}</template>
      </el-table-column>
    </el-table>

    <ErpPagination :total="total" v-model:page="page" v-model:size="size" @change="load" @size-change="search" />
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { usePageList } from '@/composables/usePage'

interface TransRow {
  id: number
  transNo: string
  transType: string
  direction: number
  materialId: number
  warehouseId: number
  quantity: number
  beforeQty: number
  afterQty: number
  sourceNo: string
  transTime: string
}

const { keyword, total, rows, loading, page, size, load, search, reset } = usePageList<TransRow>(
  '/api/inv/trans'
)

function typeText(t: string) {
  const map: Record<string, string> = {
    IN_PO: '采购入库',
    IN_PR: '生产入库',
    IN_OTHER: '其它入库',
    OUT_SO: '销售出库',
    OUT_PR: '生产领料',
    OUT_OTHER: '其它出库',
    ADJ: '库存调拨'
  }
  return map[t] || t
}

function typeTag(t: string) {
  if (t.startsWith('IN')) return 'danger'
  if (t.startsWith('OUT')) return 'success'
  return 'info'
}

onMounted(load)
</script>

<style scoped>
.pager {
  margin-top: 12px;
  justify-content: flex-end;
}
</style>
