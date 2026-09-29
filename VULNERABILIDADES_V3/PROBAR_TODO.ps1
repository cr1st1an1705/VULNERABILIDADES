$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $Root
Write-Host "Restaurando..." -ForegroundColor Cyan
dotnet restore .\VULNERABILIDADES_V3.sln
if ($LASTEXITCODE -ne 0) { throw "RESTORE fallo." }
Write-Host "Compilando..." -ForegroundColor Cyan
dotnet build .\VULNERABILIDADES_V3.sln --no-restore
if ($LASTEXITCODE -ne 0) { throw "BUILD fallo." }
Write-Host "OK: los 5 proyectos compilaron." -ForegroundColor Green
