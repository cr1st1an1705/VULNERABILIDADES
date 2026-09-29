using TavoSecurityLab.Common;

namespace TavoSecurityLab.Exercises;

public static class BrokenAccessControlVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/access/vulnerable",
            () =>
        {
            var rows =
                string.Join(
                    Environment.NewLine,
                    LabState.Users.Select(
                        user =>
                        $"""
<tr>
<td>{user.Id}</td>
<td>{Html.E(user.Username)}</td>
<td>{Html.E(user.Role)}</td>
<td>{user.Active}</td>
<td>
<form method="post" action="/access/vulnerable/deactivate">
<input type="hidden" name="id" value="{user.Id}">
<button class="danger">Desactivar</button>
</form>
</td>
</tr>
"""));

            return Html.Result(
                "Broken Access Control vulnerable",
                $"""
<h1>Broken Access Control - Vulnerable</h1>
<div class="info">Usuario actual: <b>{LabState.CurrentUsername}</b> / rol: <b>{LabState.CurrentRole}</b></div>
<div class="warning">La operación administrativa no comprueba permisos.</div>
<table>
<tr><th>ID</th><th>Usuario</th><th>Rol</th><th>Activo</th><th>Acción</th></tr>
{rows}
</table>
<p><a class="btn safe" href="/access/seguro">Abrir versión segura</a></p>
""");
        });

        app.MapPost(
            "/access/vulnerable/deactivate",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();

            if (!int.TryParse(
                    form["id"],
                    out var id))
            {
                return Html.Result(
                    "Error",
                    "<div class='warning'>ID inválido.</div>",
                    400);
            }

            // ==========================================================
            // VULNERABILIDAD: BROKEN ACCESS CONTROL
            //
            // El backend ejecuta la operación sensible sin comprobar si
            // el usuario posee el rol requerido.
            // ==========================================================
            var user =
                LabState.Users.FirstOrDefault(
                    x => x.Id == id);

            if (user is null)
            {
                return Html.Result(
                    "No encontrado",
                    "<h1>404</h1>",
                    404);
            }

            user.Active = false;

            return Results.Redirect("/access/vulnerable");
        });
    }
}
