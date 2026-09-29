using AxelSecurityLab.Common;

namespace AxelSecurityLab.Exercises;

public static class FileUploadVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/upload/vulnerable",
            () =>
                Html.Result(
                    "File Upload vulnerable",
                    """
<h1>File Upload - Vulnerable</h1>
<div class="warning">El servidor no valida extensión, tamaño, Content-Type ni firma real.</div>
<form method="post" action="/upload/vulnerable" enctype="multipart/form-data">
<input type="file" name="file">
<button class="danger">Subir sin validación</button>
</form>
<p><a class="btn safe" href="/upload/seguro">Abrir versión segura</a></p>
"""));

        app.MapPost(
            "/upload/vulnerable",
            async (
                HttpRequest request,
                IWebHostEnvironment environment
            ) =>
        {
            var form = await request.ReadFormAsync();
            var file = form.Files.GetFile("file");

            if (file is null)
            {
                return Html.Result(
                    "Error",
                    "<div class='warning'>No se recibió archivo.</div>",
                    400);
            }

            var uploadRoot =
                Path.Combine(
                    environment.WebRootPath,
                    "uploads");

            Directory.CreateDirectory(uploadRoot);

            // ==========================================================
            // VULNERABILIDAD: FILE UPLOAD INSEGURO
            //
            // Se acepta el archivo sin validar extensión, MIME, tamaño
            // ni firma. Además se almacena dentro de wwwroot.
            // ==========================================================
            var fileName =
                Path.GetFileName(file.FileName);

            var destination =
                Path.Combine(uploadRoot, fileName);

            await using var output =
                File.Create(destination);

            await file.CopyToAsync(output);

            return Html.Result(
                "Archivo aceptado",
                $"""
<h1>Archivo aceptado sin validar</h1>
<div class="warning">
Nombre: {Html.E(fileName)}<br>
Content-Type: {Html.E(file.ContentType)}
</div>
<a class="btn" href="/uploads/{Uri.EscapeDataString(fileName)}" target="_blank">Abrir archivo subido</a>
<a class="btn danger" href="/upload/vulnerable">Volver</a>
""");
        });
    }
}
