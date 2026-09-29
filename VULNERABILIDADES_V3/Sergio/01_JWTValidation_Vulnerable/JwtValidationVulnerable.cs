using System.Text;
using System.Text.Json;
using SergioSecurityLab.Common;

namespace SergioSecurityLab.Exercises;

public static class JwtValidationVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/jwt/vulnerable",
            () =>
        {
            var forged =
                JwtTools.CreateToken(
                    "sergio",
                    "Administrator",
                    JwtTools.FakeKey);

            return Html.Result(
                "JWT vulnerable",
                $"""
<h1>JWT Validation - Vulnerable</h1>
<div class="warning">El validador únicamente decodifica el JWT; no comprueba la firma.</div>
<h3>Token simulado con Role=Administrator firmado con una clave NO confiable</h3>
<textarea rows="9" readonly>{Html.E(forged)}</textarea>
<form method="post" action="/jwt/vulnerable">
<input type="hidden" name="token" value="{Html.E(forged)}">
<button class="danger">Validar de forma vulnerable</button>
</form>
<p><a class="btn safe" href="/jwt/seguro">Abrir versión segura</a></p>
""");
        });

        app.MapPost(
            "/jwt/vulnerable",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();
            var token = form["token"].ToString();

            try
            {
                var parts = token.Split('.');

                if (parts.Length != 3)
                {
                    throw new InvalidOperationException(
                        "Formato JWT incorrecto.");
                }

                // ==========================================================
                // VULNERABILIDAD: JWT SIN VALIDAR FIRMA
                //
                // Se decodifica el payload y se confía en sus claims sin
                // verificar que la firma provenga de la clave confiable.
                // ==========================================================
                var payloadJson =
                    Encoding.UTF8.GetString(
                        JwtTools.Base64UrlDecode(
                            parts[1]));

                using var json =
                    JsonDocument.Parse(payloadJson);

                var user =
                    json.RootElement
                        .GetProperty("sub")
                        .GetString();

                var role =
                    json.RootElement
                        .GetProperty("role")
                        .GetString();

                return Html.Result(
                    "JWT vulnerable",
                    $"""
<h1>Token aceptado SIN verificar firma</h1>
<div class="warning">
Usuario: <b>{Html.E(user)}</b><br>
Rol: <b>{Html.E(role)}</b>
</div>
<a class="btn" href="/jwt/vulnerable">Volver</a>
""");
            }
            catch (Exception ex)
            {
                return Html.Result(
                    "JWT error",
                    $"<div class='warning'>{Html.E(ex.Message)}</div>",
                    400);
            }
        });
    }
}
