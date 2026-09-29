using EdwardSecurityLab.Common;

namespace EdwardSecurityLab.Exercises;

public static class StoredXssVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/xss/vulnerable",
            () =>
        {
            // ==========================================================
            // VULNERABILIDAD: STORED XSS
            //
            // El mensaje almacenado se inserta directamente en el HTML
            // sin aplicar codificación de salida.
            // ==========================================================
            var comments =
                string.Join(
                    Environment.NewLine,
                    LabState.Comments.Select(
                        x =>
                        $"""
<div class="card">
<b>{Html.E(x.Author)}</b>
<div>{x.Message}</div>
</div>
"""));

            return Html.Result(
                "Stored XSS vulnerable",
                $"""
<h1>Stored XSS - Vulnerable</h1>
<div class="warning">El comentario almacenado se inserta sin output encoding.</div>
<form method="post" action="/xss/vulnerable">
<textarea name="message" rows="4"></textarea>
<button class="danger">Publicar</button>
</form>
<p>Prueba: <code>&lt;script&gt;alert('XSS')&lt;/script&gt;</code></p>
{comments}
<p><a class="btn safe" href="/xss/seguro">Abrir versión segura</a></p>
""");
        });

        app.MapPost(
            "/xss/vulnerable",
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

            return Results.Redirect("/xss/vulnerable");
        });
    }
}
