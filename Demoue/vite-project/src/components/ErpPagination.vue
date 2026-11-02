<template>
  <el-pagination
    class="erp-pager"
    background
    layout="total, sizes, prev, pager, next, jumper, slot"
    :total="total"
    :page-sizes="pageSizes"
    :pager-count="7"
    :current-page="page"
    :page-size="size"
    @current-change="onPageChange"
    @size-change="onSizeChange"
  >
    <template #default>
      <span class="erp-pager-pages">第 {{ page }} / {{ totalPages }} 页</span>
    </template>
  </el-pagination>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = withDefaults(
  defineProps<{
    total: number
    page: number
    size: number
    pageSizes?: number[]
  }>(),
  { pageSizes: () => [10, 20, 50, 100] }
)

const emit = defineEmits<{
  (e: 'update:page', v: number): void
  (e: 'update:size', v: number): void
  (e: 'change', v: number): void
  (e: 'size-change', v: number): void
}>()

const totalPages = computed(() => Math.max(1, Math.ceil((props.total || 0) / (props.size || 1))))

function onPageChange(v: number) {
  emit('update:page', v)
  emit('change', v)
}

function onSizeChange(v: number) {
  emit('update:size', v)
  emit('size-change', v)
}
</script>
