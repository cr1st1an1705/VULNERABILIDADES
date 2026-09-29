using EdwardSecurityLab.Common;

namespace EdwardSecurityLab.Exercises;

public static class CsrfVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/csrf/vulnerable",
            () =>
            Html.Result(
                "CSRF vulnerable",
                $"""
<h1>CSRF - Vulnerable</h1>
<div class="warning">La operación modifica estado sin verificar un token CSRF.</div>
<div class="card">Correo actual: <b>{Html.E(LabState.Email)}</b></div>
<form method="post" action="/csrf/vulnerable/change">
<input name="email" value="attacker@securitylab.local">
<button class="danger">Cambiar sin token</button>
</form>
<div class="info">Intercepta este POST con Burp. No existe token de verificación.</div>
<p><a class="btn safe" href="/csrf/seguro">Abrir versión segura</a></p>
"""));

        app.MapPost(
            "/csrf/vulnerable/change",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();

            // ==========================================================
            // VULNERABILIDAD: CSRF
            //
            // El servidor ejecuta una modificación sensible sin exigir
            // una prueba de que la solicitud salió del formulario válido.
            // ==========================================================
            LabState.Email = form["email"].ToString();

            return Results.Redirect("/csrf/vulnerable");
        });
    }
}
