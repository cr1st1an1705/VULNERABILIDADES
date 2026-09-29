using EdwardSecurityLab.Common;

namespace EdwardSecurityLab.Exercises;

public static class StoredXssSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/xss/seguro",
            () =>
        {
            // ==========================================================
            // SOLUCIÓN: OUTPUT ENCODING
            //
            // El contenido almacenado se codifica antes de insertarse en
            // el HTML. Las etiquetas se muestran como texto.
            // ==========================================================
            var comments =
                string.Join(
                    Environment.NewLine,
                    LabState.Comments.Select(
                        x =>
                        $"""
<div class="card">
<b>{Html.E(x.Author)}</b>
<div>{Html.E(x.Message)}</div>
</div>
"""));

            return Html.Result(
                "Stored XSS seguro",
                $"""
<h1>Stored XSS - Seguro</h1>
<div class="success">El comentario se codifica antes de enviarse al navegador.</div>
<form method="post" action="/xss/seguro">
<textarea name="message" rows="4"></textarea>
<button class="safe">Publicar</button>
</form>
<p>Repite la misma cadena con <code>&lt;script&gt;</code>.</p>
{comments}
<p><a class="btn danger" href="/xss/vulnerable">Volver a vulnerable</a></p>
""");
        });

        app.MapPost(
            "/xss/seguro",
            async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();
            var message = form["message"].ToString();

            if (!string.IsNullOrWhiteSpace(message))
            {
                LabState.Comments.Add(
                    new StoredComment(
                        LabState.Comments.Count + 1,
                        "edward",
                        message));
            }

            return Results.Redirect("/xss/seguro");
        });
    }
}
