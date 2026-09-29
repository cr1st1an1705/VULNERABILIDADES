using CristianSecurityLab.Common;

namespace CristianSecurityLab.Exercises;

public static class IdorVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/idor/vulnerable",
            (int? id) =>
        {
            var requestedId = id ?? 101;

            // ==========================================================
            // VULNERABILIDAD: IDOR
            //
            // Se devuelve el recurso solicitado por ID sin comprobar
            // que el OwnerId pertenezca al usuario autenticado.
            // ==========================================================
            var document =
                IdorData.Documents.FirstOrDefault(
                    x => x.Id == requestedId);

            var result =
                document is null
                ? "<div class='warning'>Documento no encontrado.</div>"
                : $"""
<div class="card">
<b>ID:</b> {document.Id}<br>
<b>OwnerId:</b> {document.OwnerId}<br>
<b>Archivo:</b> {Html.E(document.Name)}
<pre>{Html.E(document.Content)}</pre>
</div>
""";

            return Html.Result(
                "IDOR vulnerable",
                $"""
<h1>IDOR - Vulnerable</h1>
<div class="info">Usuario simulado: Cristian / OwnerId = {IdorData.CurrentUserId}</div>
<form method="get">
<label>ID del documento</label>
<input type="number" name="id" value="{requestedId}">
<button class="danger">Consultar</button>
</form>
<p>Prueba primero <code>101</code> y luego <code>102</code> o <code>104</code>.</p>
<p>También puedes interceptar <code>?id=101</code> con Burp Repeater y cambiarlo.</p>
{result}
<p><a class="btn safe" href="/idor/seguro?id=101">Abrir versión segura</a></p>
""");
        });
    }
}
