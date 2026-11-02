import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { fileURLToPath, URL } from 'node:url'

// ERP 管理后台前端构建配置
// 开发时通过 /api 代理到 .NET 后端（http://localhost:5290）
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  // 注入构建时间戳，供页脚显示
  define: {
    __APP_BUILD_TIME__: JSON.stringify(new Date().toISOString().slice(0, 19).replace('T', ' '))
  },
  server: {
    // 监听所有地址，兼容 IPv4 / IPv6 访问
    host: '0.0.0.0',
    port: 5173,
    strictPort: true,
    // 开发期禁用浏览器缓存，避免加载旧的 .vue 模块
    headers: {
      'Cache-Control': 'no-store, no-cache, must-revalidate'
    },
    proxy: {
      '/api': {
        target: 'http://127.0.0.1:5290',
        changeOrigin: true
      }
    }
  }
})
