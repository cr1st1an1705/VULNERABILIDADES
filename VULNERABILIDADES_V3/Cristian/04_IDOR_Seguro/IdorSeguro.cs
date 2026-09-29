using CristianSecurityLab.Common;

namespace CristianSecurityLab.Exercises;

public static class IdorSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/idor/seguro",
            (int? id) =>
        {
            var requestedId = id ?? 101;

            var document =
                IdorData.Documents.FirstOrDefault(
                    x => x.Id == requestedId);

            if (document is null)
            {
                return Html.Result(
                    "IDOR seguro",
                    "<h1>404</h1><div class='warning'>Documento no encontrado.</div>",
                    404);
            }

            // ==========================================================
            // SOLUCIÓN: AUTORIZACIÓN A NIVEL DE OBJETO
            //
            // Antes de devolver el documento se valida que el OwnerId
            // corresponda al usuario autenticado.
            // ==========================================================
            if (document.OwnerId != IdorData.CurrentUserId)
            {
                return Html.Result(
                    "IDOR bloqueado",
                    $"""
<h1>403 Forbidden</h1>
<div class="warning">El documento {document.Id} existe, pero no pertenece a Cristian.</div>
<p>Usuario actual OwnerId={IdorData.CurrentUserId}; recurso OwnerId={document.OwnerId}.</p>
<a class="btn" href="/idor/seguro?id=101">Probar documento propio</a>
""",
                    403);
            }

            return Html.Result(
                "IDOR seguro",
                $"""
<h1>IDOR - Seguro</h1>
<div class="success">El backend validó la propiedad del recurso.</div>
<div class="card">
<b>ID:</b> {document.Id}<br>
<b>OwnerId:</b> {document.OwnerId}<br>
<pre>{Html.E(document.Content)}</pre>
</div>
<p>Ahora cambia <code>id=101</code> por <code>id=102</code>; el servidor debe responder 403.</p>
""");
        });
    }
}
