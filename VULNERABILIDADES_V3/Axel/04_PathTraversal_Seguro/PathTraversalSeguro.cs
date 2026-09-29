using AxelSecurityLab.Common;

namespace AxelSecurityLab.Exercises;

public static class PathTraversalSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/path/seguro",
            (
                string? file,
                IWebHostEnvironment environment
            ) =>
        {
            var requested =
                string.IsNullOrWhiteSpace(file)
                ? "report-september.txt"
                : file;

            var root =
                Path.GetFullPath(
                    Path.Combine(
                        environment.ContentRootPath,
                        "Data",
                        "Reports"));

            var requestedPath =
                Path.GetFullPath(
                    Path.Combine(
                        root,
                        requested));

            var allowedPrefix =
                root.TrimEnd(
                    Path.DirectorySeparatorChar)
                +
                Path.DirectorySeparatorChar;

            // ==========================================================
            // SOLUCIÓN: NORMALIZACIÓN Y RESTRICCIÓN DE DIRECTORIO
            //
            // Se calcula la ruta absoluta y se exige que permanezca
            // dentro de Data/Reports antes de leer el archivo.
            // ==========================================================
            if (!requestedPath.StartsWith(
                    allowedPrefix,
                    StringComparison.OrdinalIgnoreCase))
            {
                return Html.Result(
                    "Path Traversal bloqueado",
                    $"""
<h1>Solicitud rechazada</h1>
<div class="warning">La ruta intenta salir del directorio permitido.</div>
<pre>{Html.E(requestedPath)}</pre>
<a class="btn" href="/path/seguro">Volver</a>
""",
                    400);
            }

            if (!File.Exists(requestedPath))
            {
                return Html.Result(
                    "No encontrado",
                    "<h1>404</h1>",
                    404);
            }

            return Html.Result(
                "Path Traversal seguro",
                $"""
<h1>Path Traversal - Seguro</h1>
<div class="success">La ruta final permanece dentro de Data/Reports.</div>
<form method="get">
<input name="file" value="{Html.E(requested)}">
<button class="safe">Leer archivo</button>
</form>
<p>Repite: <code>..\Secrets\internal-secret.txt</code></p>
<pre>{Html.E(File.ReadAllText(requestedPath))}</pre>
""");
        });
    }
}
