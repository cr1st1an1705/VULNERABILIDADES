using AxelSecurityLab.Common;

namespace AxelSecurityLab.Exercises;

public static class FileUploadSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/upload/seguro",
            () =>
                Html.Result(
                    "File Upload seguro",
                    """
<h1>File Upload - Seguro</h1>
<div class="success">Se valida tamaño, extensión, Content-Type y firma real. El archivo se guarda fuera de wwwroot con un nombre generado.</div>
<form method="post" action="/upload/seguro" enctype="multipart/form-data">
<input type="file" name="file">
<button class="safe">Subir con validaciones</button>
</form>
<p>Permitidos: PDF, PNG, JPG/JPEG. Máximo 2 MB.</p>
"""));

        app.MapPost(
            "/upload/seguro",
            async (
                HttpRequest request,
                IWebHostEnvironment environment
            ) =>
        {
            var form = await request.ReadFormAsync();
            var file = form.Files.GetFile("file");

            if (file is null)
            {
                return Reject("No se recibió archivo.");
            }

            const long maxSize =
                2 * 1024 * 1024;

            var allowedExtensions =
                new HashSet<string>(
                    [".pdf", ".png", ".jpg", ".jpeg"],
                    StringComparer.OrdinalIgnoreCase);

            var allowedContentTypes =
                new HashSet<string>(
                    ["application/pdf", "image/png", "image/jpeg"],
                    StringComparer.OrdinalIgnoreCase);

            var extension =
                Path.GetExtension(file.FileName);

            // ==========================================================
            // SOLUCIÓN: VALIDACIÓN EN CAPAS
            //
            // Se comprueba tamaño, extensión, MIME y firma real. Después
            // se genera un nombre y se almacena fuera de wwwroot.
            // ==========================================================
            if (file.Length > maxSize)
            {
                return Reject("El archivo supera 2 MB.");
            }

            if (!allowedExtensions.Contains(extension))
            {
                return Reject("Extensión no permitida.");
            }

            if (!allowedContentTypes.Contains(file.ContentType))
            {
                return Reject("Content-Type no permitido.");
            }

            if (!await HasValidSignature(file, extension))
            {
                return Reject("La firma real no coincide con la extensión.");
            }

            var safeRoot =
                Path.Combine(
                    environment.ContentRootPath,
                    "Data",
                    "SafeUploads");

            Directory.CreateDirectory(safeRoot);

            var generatedName =
                $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";

            await using var output =
                File.Create(
                    Path.Combine(
                        safeRoot,
                        generatedName));

            await file.CopyToAsync(output);

            return Html.Result(
                "Archivo seguro",
                $"""
<h1>Archivo validado</h1>
<div class="success">Guardado fuera de wwwroot como <code>{Html.E(generatedName)}</code>.</div>
<a class="btn" href="/upload/seguro">Volver</a>
""");
        });
    }

    private static IResult Reject(string message) =>
        Html.Result(
            "Archivo rechazado",
            $"<h1>Archivo rechazado</h1><div class='warning'>{Html.E(message)}</div>",
            400);

    private static async Task<bool> HasValidSignature(
        IFormFile file,
        string extension)
    {
        var signatures =
            new Dictionary<string, byte[][]>(
                StringComparer.OrdinalIgnoreCase)
            {
                [".pdf"] = [new byte[] { 0x25, 0x50, 0x44, 0x46 }],
                [".png"] = [new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }],
                [".jpg"] = [new byte[] { 0xFF, 0xD8, 0xFF }],
                [".jpeg"] = [new byte[] { 0xFF, 0xD8, 0xFF }]
            };

        if (!signatures.TryGetValue(
                extension,
                out var allowed))
        {
            return false;
        }

        var length =
            allowed.Max(x => x.Length);

        var buffer =
            new byte[length];

        await using var stream =
            file.OpenReadStream();

        var read =
            await stream.ReadAsync(
                buffer.AsMemory(
                    0,
                    length));

        return allowed.Any(
            signature =>
                read >= signature.Length &&
                buffer
                    .AsSpan(
                        0,
                        signature.Length)
                    .SequenceEqual(signature));
    }
}
