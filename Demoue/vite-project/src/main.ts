import { createApp } from 'vue'
import { createPinia } from 'pinia'
import ElementPlus from 'element-plus'
import zhCn from 'element-plus/es/locale/lang/zh-cn'
import 'element-plus/dist/index.css'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

import App from './App.vue'
import router from './router'
import ErpPagination from './components/ErpPagination.vue'
import './style.css'
import './theme.css'

const app = createApp(App)

// 全量注册 Element Plus 图标（管理后台按钮/菜单常用）
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component)
}

app.use(createPinia())
app.use(router)
app.use(ElementPlus, { locale: zhCn }) // 全局中文：日期面板/分页/弹窗等内置文案
app.component('ErpPagination', ErpPagination) // 全局注册：列表页统一分页
app.mount('#app')
