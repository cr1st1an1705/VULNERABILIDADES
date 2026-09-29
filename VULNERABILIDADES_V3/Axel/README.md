# Axel — Security Lab

Puerto: `http://localhost:5103`

## Ejecutar

```powershell
cd Axel
dotnet restore
dotnet build
dotnet run
```

## Los 4 ejercicios

1. File Upload vulnerable
2. File Upload seguro
3. Path Traversal vulnerable
4. Path Traversal seguro

### File Upload
La versión segura valida tamaño, extensión, MIME, firma real, nombre generado y almacenamiento fuera de `wwwroot`.

### Path Traversal
Prueba Windows: `..\Secrets\internal-secret.txt`

Qué decir: la versión segura normaliza la ruta completa y comprueba que siga dentro de `Data/Reports`.
