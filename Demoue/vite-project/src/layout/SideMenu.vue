<template>
  <template v-for="item in visibleItems" :key="item.title">
    <el-sub-menu v-if="item.children && item.children.length" :index="item.title">
      <template #title>
        <el-icon v-if="item.icon"><component :is="item.icon" /></el-icon>
        <span>{{ item.title }}</span>
      </template>
      <SideMenu :items="item.children" />
    </el-sub-menu>
    <el-menu-item v-else-if="item.path" :index="item.path">
      <el-icon v-if="item.icon"><component :is="item.icon" /></el-icon>
      <template #title>{{ item.title }}</template>
    </el-menu-item>
  </template>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type { MenuItem } from '@/router/menu'
import { useAuthStore } from '@/stores/auth'

const props = defineProps<{ items: MenuItem[] }>()
const auth = useAuthStore()

function filter(items: MenuItem[]): MenuItem[] {
  return items
    .map((it) => {
      if (it.children && it.children.length) {
        const kids = filter(it.children)
        return kids.length ? { ...it, children: kids } : null
      }
      // 叶子节点：无 perm 或拥有权限
      if (!it.perm || auth.can(it.perm)) return it
      return null
    })
    .filter((x): x is MenuItem => x !== null)
}

const visibleItems = computed(() => filter(props.items))
</script>
