using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Forwarder;

namespace Vite.Extensions.AspNetCore;

public static class ViteHelper
{
    public static IServiceCollection AddViteHelper(this IServiceCollection services, Action<ViteTagOptions>? options = null)
    {
        services.AddOptions<ViteTagOptions>()
            .PostConfigure(c =>
            {
                options?.Invoke(c);
            });
        
        services.AddTransient<ITagHelperComponent, ViteTagHelperComponent>();
        services.AddMemoryCache();

        #if DEBUG
        services.AddHttpForwarder();
        #endif
        return services;
    }

    [Conditional("DEBUG")]
    public static void UseViteForwarder(this WebApplication app)
    {
        // Configure our own HttpMessageInvoker for outbound calls for proxy operations
        var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
        {
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false,
            EnableMultipleHttp2Connections = true
        });

        // All /.vite requests will be proxied to Vite running on 3000 port
        app.Map("/.vite", vite =>
        {
            vite.Run(async (ctx) =>
            {
                var forwarder = ctx.RequestServices.GetRequiredService<IHttpForwarder>();
                var config = ctx.RequestServices.GetRequiredService<IOptions<ViteTagOptions>>();
                await forwarder.SendAsync(ctx, $"http://localhost:{config.Value.VitePort}/.vite/", httpClient);
            });
        });
    }
}