import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5017',
        changeOrigin: true,
        secure: false
      },
      '/catalog-api': {
        target: 'http://localhost:5180',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/catalog-api/, '')
      }
    }
  }
})
