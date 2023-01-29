namespace Vite.Extensions.AspNetCore
{
    public class ViteManifestItem
    {
        public string? file { get; set; }
        public string? src { get; set; }
        public bool isEntry { get; set; }
        public string[]? imports { get; set; }
        public string[]? dynamicImports { get; set; }
        public string[]? css { get; set; }
        public string[]? assets { get; set; }
    }
}