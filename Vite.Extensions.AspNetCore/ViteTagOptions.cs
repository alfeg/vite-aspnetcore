namespace Vite.Extensions.AspNetCore;

public class ViteTagOptions
{
    public int VitePort { get; set; } = 3000;
    
    /// <summary>
    /// Entry file that is used for backend integration
    /// <a href="https://vitejs.dev/guide/backend-integration.html">See Vite documentation</a>
    /// </summary>
    public string Entry { get; set; } = "src/main.ts";
}