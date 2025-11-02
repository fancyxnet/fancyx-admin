import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), 'VITE_');

  return {
    plugins: [react()],
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
      },
    },
    server: {
      host: '0.0.0.0',
      port: 8080,
      open: false,
      hmr: {
        overlay: false,
      },
      proxy: {
        '/admin-api': {
          target: env.VITE_API_BASE_URL,
          changeOrigin: true,
          configure: (proxy) => {
            proxy.on('proxyReq', (proxyReq) => {
              // 开发环境固定租户ID
              if (env.VITE_TENANT_ID) {
                proxyReq.setHeader('X-Tenant', env.VITE_TENANT_ID || 'platform');
              }
            });
          },
        },
      },
    },
  };
});
