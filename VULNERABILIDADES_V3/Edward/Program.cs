using EdwardSecurityLab.Common;
using EdwardSecurityLab.Exercises;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5102");

var app = builder.Build();

app.MapGet("/", () =>
    Html.Result(
        "Edward",
        """
<h1>Edward - Security Lab</h1>
<div class="grid">
<div class="card">
<h2>Stored XSS</h2>
<a class="btn danger" href="/xss/vulnerable">1. Vulnerable</a>
<a class="btn safe" href="/xss/seguro">2. Seguro</a>
</div>
<div class="card">
<h2>CSRF</h2>
<a class="btn danger" href="/csrf/vulnerable">3. Vulnerable</a>
<a class="btn safe" href="/csrf/seguro">4. Seguro</a>
</div>
</div>
"""));

StoredXssVulnerable.Map(app);
StoredXssSeguro.Map(app);
CsrfVulnerable.Map(app);
CsrfSeguro.Map(app);

app.Run();
