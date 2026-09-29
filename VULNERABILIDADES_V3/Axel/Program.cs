using AxelSecurityLab.Common;
using AxelSecurityLab.Exercises;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5103");

var app = builder.Build();

app.UseStaticFiles();
LabFiles.Initialize(app.Environment.ContentRootPath);

app.MapGet("/", () =>
    Html.Result(
        "Axel",
        """
<h1>Axel - Security Lab</h1>
<div class="grid">
<div class="card">
<h2>File Upload</h2>
<a class="btn danger" href="/upload/vulnerable">1. Vulnerable</a>
<a class="btn safe" href="/upload/seguro">2. Seguro</a>
</div>
<div class="card">
<h2>Path Traversal</h2>
<a class="btn danger" href="/path/vulnerable">3. Vulnerable</a>
<a class="btn safe" href="/path/seguro">4. Seguro</a>
</div>
</div>
"""));

FileUploadVulnerable.Map(app);
FileUploadSeguro.Map(app);
PathTraversalVulnerable.Map(app);
PathTraversalSeguro.Map(app);

app.Run();
