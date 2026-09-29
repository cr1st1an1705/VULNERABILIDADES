using SergioSecurityLab.Common;
using SergioSecurityLab.Exercises;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5105");

var app = builder.Build();

app.MapGet(
    "/internal/status",
    () =>
        Results.Json(
            new
            {
                service = "SergioSecurityLab.Internal",
                status = "running",
                secret = "LAB-INTERNAL-ONLY-2026"
            }));

app.MapGet(
    "/public-preview",
    () =>
        Results.Json(
            new
            {
                title = "Public Preview",
                message = "Recurso permitido."
            }));

app.MapGet("/", () =>
    Html.Result(
        "Sergio",
        """
<h1>Sergio - Security Lab</h1>
<div class="grid">
<div class="card">
<h2>JWT Validation</h2>
<a class="btn danger" href="/jwt/vulnerable">1. Vulnerable</a>
<a class="btn safe" href="/jwt/seguro">2. Seguro</a>
</div>
<div class="card">
<h2>SSRF local</h2>
<a class="btn danger" href="/ssrf/vulnerable">3. Vulnerable</a>
<a class="btn safe" href="/ssrf/seguro">4. Seguro</a>
</div>
</div>
"""));

JwtValidationVulnerable.Map(app);
JwtValidationSeguro.Map(app);
SsrfVulnerable.Map(app);
SsrfSeguro.Map(app);

app.Run();
