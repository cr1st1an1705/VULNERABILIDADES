# Edward — Security Lab

Puerto: `http://localhost:5102`

## Ejecutar

```powershell
cd Edward
dotnet restore
dotnet build
dotnet run
```

## Los 4 ejercicios

1. Stored XSS vulnerable
2. Stored XSS seguro
3. CSRF vulnerable
4. CSRF seguro

### Stored XSS
Prueba: `<script>alert('XSS')</script>`

Qué decir: el contenido queda almacenado y la versión vulnerable lo inserta sin codificación; la segura aplica output encoding.

### CSRF
Intercepta el POST con Burp. En la versión segura elimina `csrfToken` y repite; debe responder HTTP 400.

Qué decir: la sesión no prueba que el formulario sea legítimo; el token añade una validación adicional.
