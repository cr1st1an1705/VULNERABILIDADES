using System.Security.Cryptography;
using System.Text;
using EdwardSecurityLab.Common;

namespace EdwardSecurityLab.Exercises;

public static class CsrfSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/csrf/seguro",
            (HttpResponse response) =>
        {
            var token =
                Convert.ToHexString(
                    RandomNumberGenerator.GetBytes(32));

            response.Cookies.Append(
                "EdwardSecurityLabCsrf",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true
                });

            return Html.Result(
                "CSRF seguro",
                $"""
<h1>CSRF - Seguro</h1>
<div class="success">El formulario y el navegador comparten un token que el servidor verifica.</div>
<div class="card">Correo actual: <b>{Html.E(LabState.Email)}</b></div>
<form method="post" action="/csrf/seguro/change">
<input type="hidden" name="csrfToken" value="{token}">
<input name="email" value="edward.secure@securitylab.local">
<button class="safe">Cambiar con token</button>
</form>
<div class="info">En Burp Repeater elimina <code>csrfToken</code>; la respuesta debe ser HTTP 400.</div>
""");
        });

        app.MapPost(
            "/csrf/seguro/change",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();

            var formToken =
                form["csrfToken"].ToString();

            var cookieToken =
                request.Cookies["EdwardSecurityLabCsrf"] ?? string.Empty;

            // ==========================================================
            // SOLUCIÓN: TOKEN CSRF
            //
            // El backend compara el token enviado por el formulario con
            // el token asociado al navegador antes de modificar el correo.
            // ==========================================================
            if (
                string.IsNullOrWhiteSpace(formToken) ||
                string.IsNullOrWhiteSpace(cookieToken) ||
                !CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(formToken),
                    Encoding.UTF8.GetBytes(cookieToken)))
            {
                return Html.Result(
                    "CSRF bloqueado",
                    "<h1>400 Bad Request</h1><div class='warning'>Token CSRF inexistente o inválido. La operación no fue ejecutada.</div>",
                    400);
            }

            LabState.Email = form["email"].ToString();

            return Results.Redirect("/csrf/seguro");
        });
    }
}
