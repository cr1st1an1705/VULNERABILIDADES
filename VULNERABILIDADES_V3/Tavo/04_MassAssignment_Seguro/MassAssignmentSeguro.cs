using TavoSecurityLab.Common;

namespace TavoSecurityLab.Exercises;

public static class MassAssignmentSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/mass/seguro",
            () =>
                Html.Result(
                    "Mass Assignment seguro",
                    Page()));

        app.MapPost(
            "/mass/seguro/update",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();

            // ==========================================================
            // SOLUCIÓN: ALLOWLIST DE CAMPOS EDITABLES
            //
            // Esta operación únicamente procesa Name, Email y Phone.
            // IsAdmin, Balance y Role se ignoran aunque aparezcan en la
            // petición manipulada.
            // ==========================================================
            LabState.Profile.Name =
                form["Name"].ToString();

            LabState.Profile.Email =
                form["Email"].ToString();

            LabState.Profile.Phone =
                form["Phone"].ToString();

            return Results.Redirect("/mass/seguro");
        });
    }

    private static string Page() =>
        $"""
<h1>Mass Assignment - Seguro</h1>
<div class="card">
<b>Name:</b> {Html.E(LabState.Profile.Name)}<br>
<b>Email:</b> {Html.E(LabState.Profile.Email)}<br>
<b>Phone:</b> {Html.E(LabState.Profile.Phone)}<br>
<b>IsAdmin:</b> {LabState.Profile.IsAdmin}<br>
<b>Balance:</b> {LabState.Profile.Balance}<br>
<b>Role:</b> {Html.E(LabState.Profile.Role)}
</div>
<form method="post" action="/mass/seguro/update">
<label>Nombre</label><input name="Name" value="{Html.E(LabState.Profile.Name)}">
<label>Email</label><input name="Email" value="{Html.E(LabState.Profile.Email)}">
<label>Teléfono</label><input name="Phone" value="{Html.E(LabState.Profile.Phone)}">
<button class="safe">Actualizar</button>
</form>
<div class="success">
Repite en Burp agregando <code>IsAdmin</code>, <code>Role</code> y <code>Balance</code>. El servidor los ignorará.
</div>
""";
}
