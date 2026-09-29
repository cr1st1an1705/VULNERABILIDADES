using CristianSecurityLab.Common;
using CristianSecurityLab.Exercises;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5101");

var app = builder.Build();

LabDatabase.Initialize(app.Environment.ContentRootPath);

app.MapGet("/", () =>
    Html.Result(
        "Cristian",
        """
<h1>Cristian - Security Lab</h1>
<p>Este proyecto contiene únicamente los cuatro ejercicios asignados a Cristian.</p>
<div class="grid">
<div class="card">
<h2>SQL Injection</h2>
<a class="btn danger" href="/sql-injection/vulnerable">1. Vulnerable</a>
<a class="btn safe" href="/sql-injection/seguro">2. Seguro</a>
</div>
<div class="card">
<h2>IDOR</h2>
<a class="btn danger" href="/idor/vulnerable?id=101">3. Vulnerable</a>
<a class="btn safe" href="/idor/seguro?id=101">4. Seguro</a>
</div>
</div>
"""));

SqlInjectionVulnerable.Map(app);
SqlInjectionSeguro.Map(app);
IdorVulnerable.Map(app);
IdorSeguro.Map(app);

app.Run();
