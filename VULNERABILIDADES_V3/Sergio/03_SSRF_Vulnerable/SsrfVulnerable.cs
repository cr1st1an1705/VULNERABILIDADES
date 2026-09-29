using SergioSecurityLab.Common;

namespace SergioSecurityLab.Exercises;

public static class SsrfVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/ssrf/vulnerable",
            () =>
                Html.Result(
                    "SSRF vulnerable",
                    """
<h1>SSRF local - Vulnerable</h1>
<div class="warning">El usuario decide qué ruta local solicitará el backend. El host está forzado a 127.0.0.1:5105 para mantener la práctica contenida.</div>
<form method="post" action="/ssrf/vulnerable">
<label>Ruta</label>
<input name="targetPath" value="/internal/status">
<button class="danger">Solicitar desde el backend</button>
</form>
<p><a class="btn safe" href="/ssrf/seguro">Abrir versión segura</a></p>
"""));

        app.MapPost(
            "/ssrf/vulnerable",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();
            var targetPath = form["targetPath"].ToString();

            if (
                string.IsNullOrWhiteSpace(targetPath) ||
                !targetPath.StartsWith('/'))
            {
                return Html.Result(
                    "Error",
                    "<div class='warning'>Solo se permiten rutas locales que comiencen con /.</div>",
                    400);
            }

            // ==========================================================
            // VULNERABILIDAD: SSRF LOCAL CONTROLADO
            //
            // El cliente controla la ruta que solicitará el backend.
            // Para seguridad del laboratorio, el host queda fijado a
            // 127.0.0.1:5105 y no acepta destinos externos.
            // ==========================================================
            var target =
                new Uri(
                    $"http://127.0.0.1:5105{targetPath}");

            using var client = new HttpClient();

            var response =
                await client.GetAsync(target);

            var content =
                await response.Content
                    .ReadAsStringAsync();

            return Html.Result(
                "SSRF vulnerable",
                $"""
<h1>Respuesta obtenida por el backend</h1>
<p>Destino:</p>
<pre>{Html.E(target.ToString())}</pre>
<p>HTTP: <b>{(int)response.StatusCode}</b></p>
<pre>{Html.E(content)}</pre>
<a class="btn" href="/ssrf/vulnerable">Volver</a>
""");
        });
    }
}
