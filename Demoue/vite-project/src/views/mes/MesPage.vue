<template>
  <div>
    <div class="mes-page-head">
      <el-icon class="mes-page-icon"><component :is="icon" /></el-icon>
      <div>
        <h2 class="mes-page-title">{{ meta?.title || '生产制造' }}</h2>
      </div>
    </div>

    <MesCrud
      v-if="meta"
      :key="resource"
      :resource="resource"
      :columns="meta.columns"
      :fields="meta.fields"
    />
    <el-empty v-else description="未找到该生产资源" />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import MesCrud from '@/components/MesCrud.vue'
import { mesResources } from './mesConfig'

const route = useRoute()
const resource = computed(() => (route.params.resource as string) || '')
const meta = computed(() => mesResources[resource.value])
const icon = computed(() => {
  const map: Record<string, string> = {
    'work-order': 'Files',
    workstation: 'Grid',
    'production-order': 'Box',
    'material-issue': 'Timer',
    'work-report': 'TrendCharts',
    routing: 'Connection',
    'routing-step': 'Sort',
    'quality-check': 'CircleCheck'
  }
  return map[resource.value] || 'Box'
})
</script>

<style scoped>
.mes-page-head {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 14px;
}
.mes-page-icon {
  font-size: 28px;
  color: var(--el-color-primary);
  background: color-mix(in srgb, var(--el-color-primary) 12%, #fff);
  padding: 10px;
  border-radius: 10px;
}
.mes-page-title {
  margin: 0;
  font-size: 18px;
  font-weight: 700;
  color: #303133;
}
.mes-page-sub {
  margin: 2px 0 0;
  font-size: 12px;
  color: #909399;
}
</style>
