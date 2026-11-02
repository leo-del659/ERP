<template>
  <div class="erp-layout">
    <!-- 侧边栏 -->
    <aside class="erp-side" :class="{ collapsed }">
      <div class="erp-logo">
        <div class="logo-mark">E</div>
        <div class="logo-text" v-show="!collapsed">
          <span class="t1">ERP 管理后台</span>
          <span class="t2">Enterprise Resource</span>
        </div>
      </div>

      <el-scrollbar class="erp-menu-scroll">
        <el-menu
          :default-active="$route.path"
          :collapse="collapsed"
          router
          unique-opened
          background-color="transparent"
          text-color="var(--erp-side-text)"
          active-text-color="var(--erp-side-text-active)"
          class="erp-menu"
        >
          <SideMenu :items="menus" />
        </el-menu>
      </el-scrollbar>

      <div class="erp-side-foot" v-show="!collapsed">
        <span>v0.1.0 · {{ buildTime }} · © 2026 ERP</span>
      </div>
    </aside>

    <!-- 主区域 -->
    <div class="erp-main">
      <header class="erp-header">
        <div class="erp-header-left">
          <el-icon class="collapse-btn" @click="collapsed = !collapsed">
            <Fold v-if="!collapsed" />
            <Expand v-else />
          </el-icon>
          <el-breadcrumb separator="/">
            <el-breadcrumb-item :to="{ path: '/dashboard' }">首页</el-breadcrumb-item>
            <el-breadcrumb-item v-if="$route.meta.title">{{ $route.meta.title }}</el-breadcrumb-item>
          </el-breadcrumb>
        </div>

        <div class="erp-header-right">
          <!-- 消息中心：库存不足等业务提醒集中在这里 -->
          <el-popover placement="bottom-end" :width="360" trigger="click" @show="onMsgOpen">
            <template #reference>
              <span class="msg-bell">
                <el-badge :value="notify.unread" :hidden="notify.unread === 0" :max="99">
                  <el-icon class="h-icon"><Bell /></el-icon>
                </el-badge>
              </span>
            </template>
            <div class="msg-head">
              <b>消息提醒</b>
              <el-button link type="primary" size="small" :disabled="notify.items.length === 0" @click="notify.clear()">清空</el-button>
            </div>
            <el-scrollbar max-height="320px">
              <div v-if="notify.items.length === 0" class="msg-empty">暂无消息</div>
              <div v-for="m in notify.items" :key="m.key" class="msg-item" :class="{ unread: !m.read }">
                <div class="msg-item-top">
                  <el-tag :type="m.type === 'stock' ? 'danger' : m.type === 'so' ? 'warning' : 'info'" size="small">
                    {{ m.type === 'stock' ? '库存预警' : m.type === 'so' ? '销售' : '系统' }}
                  </el-tag>
                  <span class="msg-time">{{ m.time }}</span>
                  <el-icon class="msg-del" @click="notify.remove(m.key)"><Close /></el-icon>
                </div>
                <div class="msg-title">{{ m.title }}</div>
                <div class="msg-content">{{ m.content }}</div>
              </div>
            </el-scrollbar>
          </el-popover>
          <el-dropdown @command="onCommand" trigger="click">
            <div class="erp-user">
              <el-avatar :size="32" class="u-avatar">{{ initial }}</el-avatar>
              <div class="u-meta">
                <span class="u-name">{{ auth.user?.realName || auth.user?.username || '管理员' }}</span>
                <span class="u-role">{{ roleText }}</span>
              </div>
              <el-icon class="u-caret"><ArrowDown /></el-icon>
            </div>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item command="profile">
                  <el-icon><UserFilled /></el-icon> 个人中心
                </el-dropdown-item>
                <el-dropdown-item command="logout" divided>
                  <el-icon><SwitchButton /></el-icon> 退出登录
                </el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </header>

      <main class="erp-content">
        <router-view v-slot="{ Component }">
          <transition name="fade" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessageBox, ElNotification } from 'element-plus'
import SideMenu from './SideMenu.vue'
import { menus } from '@/router/menu'
import { useAuthStore } from '@/stores/auth'
import { useNotifyStore, stockNoticeKey } from '@/stores/notify'
import { get } from '@/utils/request'

const auth = useAuthStore()
const notify = useNotifyStore()
const router = useRouter()
const collapsed = ref(false)

// 构建时间戳，由 vite.config.ts 的 define 注入
const buildTime = (globalThis as any).__APP_BUILD_TIME__ ?? '-'

// 消息中心：启动时拉取库存预警
onMounted(async () => {
  notify.restore()
  refreshStockAlerts()
  // 每 60s 轮询一次库存预警，出现新缺口时弹窗提醒
  setInterval(refreshStockAlerts, 60_000)
})

async function refreshStockAlerts() {
  try {
    const rows: any[] = (await get('/api/inv/alerts')) || []
    for (const r of rows) {
      const key = stockNoticeKey(r.materialCode, r.warehouseId)
      const known = notify.items.find((i) => i.key === key)
      const gap = (Number(r.safetyStock) || 0) - (Number(r.currentQty) || 0)
      // 新缺口，或缺口比上次更深时 → 弹窗提醒
      const prevGap = known ? knownGap(known) : null
      if (prevGap === null || gap > prevGap + 0.0001) {
        ElNotification.warning({
          title: '库存不足提醒',
          message: `${r.materialName}（${r.materialCode}）低于安全库存：现存 ${r.currentQty} / 安全 ${r.safetyStock}`,
          duration: 6000
        })
      }
      notify.push({
        key,
        type: 'stock',
        title: `${r.materialName}（${r.materialCode}）库存不足`,
        content: `仓库 #${r.warehouseId} 现存 ${r.currentQty}，安全库存 ${r.safetyStock}，缺口 ${gap}`
      })
    }
  } catch { /* 未登录或接口异常时静默 */ }
}

function onMsgOpen() {
  notify.markAllRead()
}

/** 从历史消息内容中解析上次缺口 */
function knownGap(n: { content: string }): number {
  const m = n.content.match(/缺口 ([\d.]+)/)
  return m ? Number(m[1]) : -1
}

const initial = computed(() => {
  const name = auth.user?.realName || auth.user?.username || '管'
  return name.trim().charAt(0).toUpperCase()
})

const roleText = computed(() => {
  const map: Record<string, string> = {
    admin: '系统管理员',
    manager: '总经理',
    finance: '财务专员',
    hr: '人事专员',
    warehouse: '仓管员',
    sale: '销售员', sales: '销售员',
    purchase: '采购员', purchaser: '采购员',
    prod: '生产计划员', production: '生产计划员'
  }
  return (auth.user?.roleCode && map[auth.user.roleCode]) || '管理员'
})

async function onCommand(cmd: string) {
  if (cmd === 'logout') {
    await ElMessageBox.confirm('确认退出登录？', '提示', { type: 'warning' }).catch(() => 'cancel')
    auth.logout()
    router.replace('/login')
  } else if (cmd === 'profile') {
    // 预留：个人中心
  }
}
</script>

<style scoped>
.erp-layout {
  display: flex;
  height: 100%;
  overflow: hidden;
}

/* ---------------- 侧边栏 ---------------- */
.erp-side {
  width: var(--erp-side-width);
  flex: 0 0 var(--erp-side-width);
  background: var(--erp-side-grad);
  display: flex;
  flex-direction: column;
  transition: width 0.22s ease, flex-basis 0.22s ease;
  position: relative;
}
.erp-side.collapsed {
  width: var(--erp-side-width-collapsed);
  flex-basis: var(--erp-side-width-collapsed);
}

.erp-logo {
  height: var(--erp-header-height);
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 0 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
  overflow: hidden;
  white-space: nowrap;
}
.logo-mark {
  flex: 0 0 30px;
  width: 30px;
  height: 30px;
  border-radius: 8px;
  background: linear-gradient(135deg, #4d7bff, #2f54eb);
  color: #fff;
  font-weight: 700;
  font-size: 18px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 10px rgba(47, 84, 235, 0.45);
}
.logo-text {
  display: flex;
  flex-direction: column;
  line-height: 1.2;
}
.logo-text .t1 {
  color: #fff;
  font-size: 15px;
  font-weight: 600;
}
.logo-text .t2 {
  color: rgba(201, 211, 230, 0.6);
  font-size: 10px;
  letter-spacing: 0.5px;
}

.erp-menu-scroll {
  flex: 1 1 auto;
  margin-top: 8px;
}
.erp-menu-scroll :deep(.el-scrollbar__view) {
  height: 100%;
}
.erp-menu {
  border-right: none;
  padding: 0 8px;
}
.erp-menu :deep(.el-sub-menu__title),
.erp-menu :deep(.el-menu-item) {
  height: 46px;
  line-height: 46px;
  border-radius: 8px;
  margin: 3px 0;
}
.erp-menu :deep(.el-menu-item) {
  position: relative;
}
.erp-menu :deep(.el-menu-item:hover),
.erp-menu :deep(.el-sub-menu__title:hover) {
  background: rgba(255, 255, 255, 0.08) !important;
}
.erp-menu :deep(.el-menu-item.is-active) {
  background: rgba(77, 123, 255, 0.18) !important;
  color: var(--erp-side-text-active) !important;
  font-weight: 600;
}
.erp-menu :deep(.el-menu-item.is-active)::before {
  content: '';
  position: absolute;
  left: 0;
  top: 9px;
  bottom: 9px;
  width: 3px;
  border-radius: 0 3px 3px 0;
  background: var(--erp-accent);
}
.erp-menu :deep(.el-sub-menu.is-active > .el-sub-menu__title) {
  color: var(--erp-side-text-active) !important;
}
.erp-menu :deep(.el-menu-item .el-icon),
.erp-menu :deep(.el-sub-menu__title .el-icon) {
  font-size: 17px;
}

.erp-side-foot {
  padding: 12px 18px;
  font-size: 11px;
  color: rgba(201, 211, 230, 0.45);
  border-top: 1px solid rgba(255, 255, 255, 0.06);
  white-space: nowrap;
}

/* ---------------- 主区域 ---------------- */
.erp-main {
  flex: 1 1 auto;
  display: flex;
  flex-direction: column;
  min-width: 0;
  background: var(--erp-bg);
}

.erp-header {
  height: var(--erp-header-height);
  flex: 0 0 var(--erp-header-height);
  background: #fff;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 18px;
  box-shadow: var(--erp-shadow-sm);
  z-index: 10;
}
.erp-header-left {
  display: flex;
  align-items: center;
  gap: 14px;
}
.collapse-btn {
  font-size: 20px;
  cursor: pointer;
  color: #5b6577;
  transition: color 0.15s;
}
.collapse-btn:hover {
  color: var(--el-color-primary);
}

.erp-header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}
.h-icon {
  font-size: 18px;
  color: #5b6577;
  cursor: pointer;
  transition: color 0.15s;
}
.h-icon:hover {
  color: var(--el-color-primary);
}

/* ---------------- 消息中心 ---------------- */
.msg-bell {
  display: inline-flex;
  outline: none;
  cursor: pointer;
}
.msg-head {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 8px;
  border-bottom: 1px solid #f0f2f5;
}
.msg-empty {
  text-align: center;
  color: #909399;
  font-size: 13px;
  padding: 24px 0;
}
.msg-item {
  padding: 8px 0;
  border-bottom: 1px solid #f5f7fa;
}
.msg-item.unread .msg-title {
  color: #303133;
  font-weight: 600;
}
.msg-item-top {
  display: flex;
  align-items: center;
  gap: 8px;
}
.msg-time {
  flex: 1;
  font-size: 11px;
  color: #c0c4cc;
}
.msg-del {
  cursor: pointer;
  color: #c0c4cc;
  font-size: 13px;
}
.msg-del:hover {
  color: #f56c6c;
}
.msg-title {
  margin-top: 4px;
  font-size: 13px;
  color: #606266;
}
.msg-content {
  margin-top: 2px;
  font-size: 12px;
  color: #909399;
  line-height: 1.5;
}

.erp-user {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  outline: none;
  padding: 4px 6px;
  border-radius: 8px;
  transition: background 0.15s;
}
.erp-user:hover {
  background: #f3f5f9;
}
.u-avatar {
  background: linear-gradient(135deg, #4d7bff, #2f54eb);
  font-weight: 600;
}
.u-meta {
  display: flex;
  flex-direction: column;
  line-height: 1.25;
  text-align: left;
}
.u-name {
  font-size: 13px;
  font-weight: 600;
  color: #303133;
}
.u-role {
  font-size: 11px;
  color: #909399;
}
.u-caret {
  color: #b0b6c0;
  font-size: 12px;
}

.erp-content {
  flex: 1 1 auto;
  padding: 16px;
  overflow: auto;
}
</style>
