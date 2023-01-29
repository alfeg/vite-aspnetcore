# Vue 3 + TypeScript + Vite + Vite.AspNetCore.Extensions

This is a sample Vue+Vite app that integrates with AspNet application

# Required changes to `vite.config.ts`

See `vite.config.ts` as a working example

## Output dir

Production build output dir should point to AspNetCore `wwwooot/assets` folder

```
const outDir = path.resolve(__dirname + "/../wwwroot/assets")
```

### Development mode

For development mode make sure that `wwwroot/assets` folder is cleaned up. If there is a files, then `Vite.Extensions.AspNetCore` middleware will serve those files instead of vite dev server

```typescript
import fs from 'fs/promises'

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
```

Vite Configurations:

## base

https://vitejs.dev/config/shared-options.html#base

Base public path when served in development or production should be set to `.vite` during `vite` dev mode running and to `assets` for assets integration in production mode

```
// Required fot Hot Reload and 
const base = context.command == "serve" ? "/.vite/" : "/assets/"
```    

## server

Make sure that Vite port configured in vite.config.ts is same as configured in the AspNet Core `ViteTagOptions`

```typescript
server: {
   host: "localhost",
   strictPort: true,
   base: base, // required
   port: 3000, // required to be same as `ViteTagOptions.Port`
},
```

### build

For Vite production build configuration please refer to the Vite documentation but make sure that rollup input file is same as in `ViteTagOptions.Entry`

```typescript
build: {
   rollupOptions: {
       input: "./src/main.ts", // required to be same as `ViteTagOptions.Entry`
   },
   outDir,
},
```

