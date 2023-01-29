# Vite.Extensions.ASP.NET Core

Small helper library to integrate Vite applications into existing Asp.Net Core application

## History

This library is born from requirement to migrate existing ASP.NET Core application views to Vue application.

Because of the size of legacy code we were not able to replace everything at once, so we use Vite backend integration capability to inject Vue application into existing ASP.NET Core backend/frontend code

## Hot it works

There is few requirements that should be met:

- During development ASP.NET Core application should be able to communicate with Vite application
    - Vite HMR should work as is
    - Minimal changes to Vite config required
    - No custom code or Vite plugins required
- For production build of Vite application ASP.NET Core should serve Vite build results
- For Release builds of ASP.NET Core application Vite connection middleware should not work


### Vite backend integration

This library is a C# implementation of the following guide from Vite documentation https://vitejs.dev/guide/backend-integration.html 


`Vite.Extensions.ASP.NET Core` library will add to the Razor views all required script entries depending on the availability of generated Vite build results.

Main piece of code responsible for Vite integration is `ViteTagHelperComponent` - implementation of Tag Helper Components https://learn.microsoft.com/en-us/ASP.NET/core/mvc/views/tag-helpers/th-components?view=ASP.NET Core-7.0


On each render of `head` or `body` tags `ViteTagHelperComponent` checks for presence of `assets/manifest.json` file for the Vite build output. 

If manifest file present - then required script/style tags are rendered based on Vite manifest.

If manifest file is not found - then following rendered as stated in Vite documentation

```html
<script type="module" src="http://localhost:5173/@vite/client"></script>
<script type="module" src="http://localhost:5173/main.js"></script>
```

## Getting started

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddViteHelper()
    .AddControllersWithViews();

var app = builder.Build();

// Integration point for all Vite related requests
app.UseViteForwarder();
```

~/Views/Shared/vue.cshtml
```html
@{
    ViewBag.Title = "title";
    Layout = "_Layout";
}
<div id="vue-app" />
```

In controller:
```csharp
public IActionResult VueSample() => View("vue");
```

See sample application on integration of Vue app as an example in the [Vite.Extensions.AspNetCore.Sample](/Vite.Extensions.AspNetCore.Sample).

### Defaults

Default integration options is 
```csharp
builder.Services        
    .AddViteHelper(o =>
    {
        o.VitePort = 3000;
        o.Entry = "src/main.ts";
    })
    .AddControllersWithViews();

// another way to programmatically change Vite integration options
builder.Services.Configure<ViteTagOptions>(c => c.Entry = "src/main.ts");
```

To change this settings in the app:
```
Vite.Extensions.AspNetCore.Sample
```

## Vite config changes

### Output dir

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