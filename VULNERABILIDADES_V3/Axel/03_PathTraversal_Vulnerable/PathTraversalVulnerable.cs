using AxelSecurityLab.Common;

namespace AxelSecurityLab.Exercises;

public static class PathTraversalVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/path/vulnerable",
            (
                string? file,
                IWebHostEnvironment environment
            ) =>
        {
            var requested =
                string.IsNullOrWhiteSpace(file)
                ? "report-september.txt"
                : file;

            var reportsRoot =
                Path.Combine(
                    environment.ContentRootPath,
                    "Data",
                    "Reports");

            // ==========================================================
            // VULNERABILIDAD: PATH TRAVERSAL
            //
            // La ruta proporcionada por el usuario se combina y se abre
            // sin comprobar que la ruta final siga dentro de Reports.
            // ==========================================================
            var requestedPath =
                Path.Combine(
                    reportsRoot,
                    requested);

            string content;

            try
            {
                content =
                    File.ReadAllText(requestedPath);
            }
            catch (Exception ex)
            {
                content =
                    $"ERROR: {ex.Message}";
            }

            return Html.Result(
                "Path Traversal vulnerable",
                $"""
<h1>Path Traversal - Vulnerable</h1>
<div class="warning">No se restringe la ruta final al directorio autorizado.</div>
<form method="get">
<input name="file" value="{Html.E(requested)}">
<button class="danger">Leer archivo</button>
</form>
<p>Normal: <code>report-september.txt</code></p>
<p>Prueba Windows: <code>..\Secrets\internal-secret.txt</code></p>
<p>Ruta resuelta:</p>
<pre>{Html.E(Path.GetFullPath(requestedPath))}</pre>
<h3>Contenido</h3>
<pre>{Html.E(content)}</pre>
<p><a class="btn safe" href="/path/seguro">Abrir versión segura</a></p>
""");
        });
    }
}
