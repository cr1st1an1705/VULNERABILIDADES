using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SergioSecurityLab.Common;

namespace SergioSecurityLab.Exercises;

public static class JwtValidationSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/jwt/seguro",
            () =>
        {
            var legitimate =
                JwtTools.CreateToken(
                    "sergio",
                    "User",
                    JwtTools.RealKey);

            var forged =
                JwtTools.CreateToken(
                    "sergio",
                    "Administrator",
                    JwtTools.FakeKey);

            return Html.Result(
                "JWT seguro",
                $"""
<h1>JWT Validation - Seguro</h1>
<div class="success">Se valida firma, issuer, audience y expiración antes de confiar en los claims.</div>
<div class="card">
<h3>Token legítimo</h3>
<textarea rows="8" readonly>{Html.E(legitimate)}</textarea>
<form method="post" action="/jwt/seguro">
<input type="hidden" name="token" value="{Html.E(legitimate)}">
<button class="safe">Validar legítimo</button>
</form>
</div>
<div class="card">
<h3>Token Administrator con firma no confiable</h3>
<textarea rows="8" readonly>{Html.E(forged)}</textarea>
<form method="post" action="/jwt/seguro">
<input type="hidden" name="token" value="{Html.E(forged)}">
<button class="safe">Intentar validar token falso</button>
</form>
</div>
""");
        });

        app.MapPost(
            "/jwt/seguro",
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

                var unsigned =
                    $"{parts[0]}.{parts[1]}";

                var expectedSignature =
                    JwtTools.Sign(
                        unsigned,
                        JwtTools.RealKey);

                // ==========================================================
                // SOLUCIÓN: VALIDACIÓN CRIPTOGRÁFICA DEL JWT
                //
                // Se recalcula la firma con la clave confiable y se valida
                // antes de leer como confiables los claims del token.
                // ==========================================================
                if (!CryptographicOperations.FixedTimeEquals(
                        Encoding.UTF8.GetBytes(expectedSignature),
                        Encoding.UTF8.GetBytes(parts[2])))
                {
                    return Html.Result(
                        "JWT rechazado",
                        "<h1>401 Unauthorized</h1><div class='warning'>Firma criptográfica inválida.</div>",
                        401);
                }

                var payloadJson =
                    Encoding.UTF8.GetString(
                        JwtTools.Base64UrlDecode(
                            parts[1]));

                using var json =
                    JsonDocument.Parse(payloadJson);

                var root = json.RootElement;

                var issuer =
                    root.GetProperty("iss").GetString();

                var audience =
                    root.GetProperty("aud").GetString();

                var expiration =
                    root.GetProperty("exp").GetInt64();

                if (
                    issuer != JwtTools.Issuer ||
                    audience != JwtTools.Audience ||
                    expiration <
                        DateTimeOffset.UtcNow
                            .ToUnixTimeSeconds())
                {
                    return Html.Result(
                        "JWT rechazado",
                        "<h1>401 Unauthorized</h1><div class='warning'>Issuer, audience o expiración inválidos.</div>",
                        401);
                }

                return Html.Result(
                    "JWT válido",
                    $"""
<h1>Token válido</h1>
<div class="success">
Usuario: <b>{Html.E(root.GetProperty("sub").GetString())}</b><br>
Rol: <b>{Html.E(root.GetProperty("role").GetString())}</b>
</div>
<a class="btn" href="/jwt/seguro">Volver</a>
""");
            }
            catch (Exception ex)
            {
                return Html.Result(
                    "JWT rechazado",
                    $"<div class='warning'>{Html.E(ex.Message)}</div>",
                    401);
            }
        });
    }
}
