using TavoSecurityLab.Common;
using TavoSecurityLab.Exercises;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5104");

var app = builder.Build();

app.MapGet("/", () =>
    Html.Result(
        "Tavo",
        """
<h1>Tavo - Security Lab</h1>
<div class="grid">
<div class="card">
<h2>Broken Access Control</h2>
<a class="btn danger" href="/access/vulnerable">1. Vulnerable</a>
<a class="btn safe" href="/access/seguro">2. Seguro</a>
</div>
<div class="card">
<h2>Mass Assignment</h2>
<a class="btn danger" href="/mass/vulnerable">3. Vulnerable</a>
<a class="btn safe" href="/mass/seguro">4. Seguro</a>
</div>
</div>
"""));

BrokenAccessControlVulnerable.Map(app);
BrokenAccessControlSeguro.Map(app);
MassAssignmentVulnerable.Map(app);
MassAssignmentSeguro.Map(app);

app.Run();
