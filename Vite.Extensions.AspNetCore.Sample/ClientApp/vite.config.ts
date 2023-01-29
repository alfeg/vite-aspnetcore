import { defineConfig } from "vite"
import vue from "@vitejs/plugin-vue"
import path from "path"
import fs from 'fs/promises'
const outDir = path.resolve(__dirname + "/../wwwroot/assets")

// https://vitejs.dev/config/
export default defineConfig(async (context) => {
    
    // clearing up output assets directory
    if (context.command == "serve" && context.mode != "test") {
        // rimraf.sync(outDir)
        await fs.rm(outDir, {recursive: true, force: true});
        await fs.mkdir(outDir)
        // fs.mkdirSync(outDir)
    }

    // Required fot Hot Reload and 
    const base = context.command == "serve" ? "/.vite/" : "/assets/"

    return {
        base: base,

        plugins: [

            vue(),

            {
                name: "CopyManifest",
            },
        ],

        server: {
            host: "localhost",
            strictPort: true,
            base: base,
            port: 3000,
        },

        build: {
            manifest: true,
            sourcemap: false,
            chunkSizeWarningLimit: 1856,
            rollupOptions: {
                input: "./src/main.ts",               
            },
            outDir,
            minify: "esbuild",
            emptyOutDir: true,
        },
    }
})
