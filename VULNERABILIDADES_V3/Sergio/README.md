# Sergio — Security Lab

Puerto: `http://localhost:5105`

## Ejecutar

```powershell
cd Sergio
dotnet restore
dotnet build
dotnet run
```

## Los 4 ejercicios

1. JWT Validation vulnerable
2. JWT Validation seguro
3. SSRF local vulnerable
4. SSRF local seguro

### JWT
La versión vulnerable decodifica un token y confía en claims sin validar firma. La segura verifica HMAC, issuer, audience y expiración.

### SSRF
El laboratorio está restringido a `127.0.0.1:5105`.

Prueba vulnerable: `/internal/status`

Única ruta permitida por la versión segura: `/public-preview`
