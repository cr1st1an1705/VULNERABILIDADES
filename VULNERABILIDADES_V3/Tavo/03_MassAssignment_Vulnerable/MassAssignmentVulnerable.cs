using TavoSecurityLab.Common;

namespace TavoSecurityLab.Exercises;

public static class MassAssignmentVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/mass/vulnerable",
            () =>
                Html.Result(
                    "Mass Assignment vulnerable",
                    Page()));

        app.MapPost(
            "/mass/vulnerable/update",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();

            // ==========================================================
            // VULNERABILIDAD: MASS ASSIGNMENT / OVERPOSTING
            //
            // Si propiedades sensibles aparecen en la petición, el backend
            // también las actualiza aunque el formulario normal no las
            // muestre al usuario.
            // ==========================================================
            if (form.TryGetValue("Name", out var name))
            {
                LabState.Profile.Name = name.ToString();
            }

            if (form.TryGetValue("Email", out var email))
            {
                LabState.Profile.Email = email.ToString();
            }

            if (form.TryGetValue("Phone", out var phone))
            {
                LabState.Profile.Phone = phone.ToString();
            }

            if (
                form.TryGetValue("IsAdmin", out var isAdmin) &&
                bool.TryParse(
                    isAdmin,
                    out var adminValue))
            {
                LabState.Profile.IsAdmin = adminValue;
            }

            if (form.TryGetValue("Role", out var role))
            {
                LabState.Profile.Role = role.ToString();
            }

            if (
                form.TryGetValue("Balance", out var balance) &&
                decimal.TryParse(
                    balance,
                    out var balanceValue))
            {
                LabState.Profile.Balance = balanceValue;
            }

            return Results.Redirect("/mass/vulnerable");
        });
    }

    private static string Page() =>
        $"""
<h1>Mass Assignment - Vulnerable</h1>
<div class="card">
<b>Name:</b> {Html.E(LabState.Profile.Name)}<br>
<b>Email:</b> {Html.E(LabState.Profile.Email)}<br>
<b>Phone:</b> {Html.E(LabState.Profile.Phone)}<br>
<b>IsAdmin:</b> {LabState.Profile.IsAdmin}<br>
<b>Balance:</b> {LabState.Profile.Balance}<br>
<b>Role:</b> {Html.E(LabState.Profile.Role)}
</div>
<form method="post" action="/mass/vulnerable/update">
<label>Nombre</label><input name="Name" value="{Html.E(LabState.Profile.Name)}">
<label>Email</label><input name="Email" value="{Html.E(LabState.Profile.Email)}">
<label>Teléfono</label><input name="Phone" value="{Html.E(LabState.Profile.Phone)}">
<button class="danger">Actualizar</button>
</form>
<div class="warning">
Intercepta el POST en Burp y agrega:
<pre>IsAdmin=true&Role=Administrator&Balance=99999</pre>
</div>
<p><a class="btn safe" href="/mass/seguro">Abrir versión segura</a></p>
""";
}
