<template>
  <div>
    <div class="hr-page-head">
      <el-icon class="hr-page-icon"><component :is="icon" /></el-icon>
      <div>
        <h2 class="hr-page-title">{{ meta?.title || '人事管理' }}</h2>
      </div>
    </div>

    <HrCrud
      v-if="meta"
      :key="resource"
      :resource="resource"
      :columns="meta.columns"
      :fields="meta.fields"
    />
    <el-empty v-else description="未找到该人事资源" />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import HrCrud from '@/components/HrCrud.vue'
import { hrResources } from './hrConfig'

const route = useRoute()
const resource = computed(() => (route.params.resource as string) || '')
const meta = computed(() => hrResources[resource.value])
const icon = computed(() => {
  const map: Record<string, string> = {
    department: 'OfficeBuilding',
    position: 'Postcard',
    employee: 'User',
    contract: 'Files',
    shift: 'Clock',
    attendance: 'Calendar',
    leave: 'Sunny',
    overtime: 'AlarmClock',
    salary: 'Money',
    'recruit-plan': 'Avatar',
    candidate: 'Avatar',
    interview: 'ChatDotRound',
    offer: 'Promotion',
    'perf-plan': 'TrendCharts',
    'train-plan': 'Reading'
  }
  return map[resource.value] || 'User'
})
</script>

<style scoped>
.hr-page-head {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 14px;
}
.hr-page-icon {
  font-size: 28px;
  color: var(--el-color-primary);
  background: color-mix(in srgb, var(--el-color-primary) 12%, #fff);
  padding: 10px;
  border-radius: 10px;
}
.hr-page-title {
  margin: 0;
  font-size: 18px;
  font-weight: 700;
  color: #303133;
}
.hr-page-sub {
  margin: 2px 0 0;
  font-size: 12px;
  color: #909399;
}
</style>
