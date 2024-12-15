import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  base: "/budgie/",
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      "/api": {
        target: "https://liamensbey.com",
        changeOrigin: true,
        secure: true,
        rewrite: (path) => path.replace(/^\/api/, "/api"),
        // ws: true
      },
    },
  },
});
