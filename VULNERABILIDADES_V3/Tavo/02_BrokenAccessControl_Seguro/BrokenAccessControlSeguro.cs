using TavoSecurityLab.Common;

namespace TavoSecurityLab.Exercises;

public static class BrokenAccessControlSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/access/seguro",
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
<form method="post" action="/access/seguro/deactivate">
<input type="hidden" name="id" value="{user.Id}">
<button class="safe">Intentar desactivar</button>
</form>
</td>
</tr>
"""));

            return Html.Result(
                "Broken Access Control seguro",
                $"""
<h1>Broken Access Control - Seguro</h1>
<div class="info">Usuario actual: <b>{LabState.CurrentUsername}</b> / rol: <b>{LabState.CurrentRole}</b></div>
<div class="success">El backend exige el rol Administrator antes de ejecutar la acción.</div>
<table>
<tr><th>ID</th><th>Usuario</th><th>Rol</th><th>Activo</th><th>Prueba</th></tr>
{rows}
</table>
""");
        });

        app.MapPost(
            "/access/seguro/deactivate",
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
            // SOLUCIÓN: AUTORIZACIÓN EN EL BACKEND
            //
            // El permiso se verifica antes de ejecutar la operación. No
            // importa si el usuario fabrica la petición desde Burp.
            // ==========================================================
            if (!string.Equals(
                    LabState.CurrentRole,
                    "Administrator",
                    StringComparison.Ordinal))
            {
                return Html.Result(
                    "Acceso denegado",
                    "<h1>403 Forbidden</h1><div class='warning'>El usuario actual no posee el rol Administrator.</div>",
                    403);
            }

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

            return Results.Redirect("/access/seguro");
        });
    }
}
