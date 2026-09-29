using SergioSecurityLab.Common;

namespace SergioSecurityLab.Exercises;

public static class SsrfSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/ssrf/seguro",
            () =>
                Html.Result(
                    "SSRF seguro",
                    """
<h1>SSRF local - Seguro</h1>
<div class="success">El backend usa una allowlist de rutas permitidas.</div>
<form method="post" action="/ssrf/seguro">
<label>Ruta</label>
<input name="targetPath" value="/internal/status">
<button class="safe">Probar allowlist</button>
</form>
<p>Única ruta permitida: <code>/public-preview</code></p>
"""));

        app.MapPost(
            "/ssrf/seguro",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();
            var targetPath = form["targetPath"].ToString();

            // ==========================================================
            // SOLUCIÓN: ALLOWLIST DE DESTINOS
            //
            // El servidor decide explícitamente qué recurso puede
            // solicitar. El cliente no puede seleccionar libremente una
            // ruta interna.
            // ==========================================================
            if (!string.Equals(
                    targetPath,
                    "/public-preview",
                    StringComparison.Ordinal))
            {
                return Html.Result(
                    "SSRF bloqueado",
                    "<h1>400 Bad Request</h1><div class='warning'>La versión segura solo permite /public-preview.</div>",
                    400);
            }

            using var client = new HttpClient();

            var content =
                await client.GetStringAsync(
                    "http://127.0.0.1:5105/public-preview");

            return Html.Result(
                "SSRF seguro",
                $"""
<h1>Destino permitido</h1>
<div class="success">La ruta está incluida en la allowlist.</div>
<pre>{Html.E(content)}</pre>
<a class="btn" href="/ssrf/seguro">Volver</a>
""");
        });
    }
}
