# VULNERABILIDADES_V3

Repositorio/laboratorio con cinco proyectos totalmente independientes.

Cada integrante entra a su propia carpeta y ejecuta únicamente sus cuatro ejercicios: dos implementaciones vulnerables y las dos versiones corregidas.

## Estructura

```text
VULNERABILIDADES_V3/
├── Cristian/
│   ├── 01_SQLInjection_Vulnerable/
│   ├── 02_SQLInjection_Seguro/
│   ├── 03_IDOR_Vulnerable/
│   └── 04_IDOR_Seguro/
├── Edward/
│   ├── 01_StoredXSS_Vulnerable/
│   ├── 02_StoredXSS_Seguro/
│   ├── 03_CSRF_Vulnerable/
│   └── 04_CSRF_Seguro/
├── Axel/
│   ├── 01_FileUpload_Vulnerable/
│   ├── 02_FileUpload_Seguro/
│   ├── 03_PathTraversal_Vulnerable/
│   └── 04_PathTraversal_Seguro/
├── Tavo/
│   ├── 01_BrokenAccessControl_Vulnerable/
│   ├── 02_BrokenAccessControl_Seguro/
│   ├── 03_MassAssignment_Vulnerable/
│   └── 04_MassAssignment_Seguro/
└── Sergio/
    ├── 01_JWTValidation_Vulnerable/
    ├── 02_JWTValidation_Seguro/
    ├── 03_SSRF_Vulnerable/
    └── 04_SSRF_Seguro/
```

## Ejecutar cada integrante

### Cristian

```powershell
cd .\Cristian
dotnet run
```

Abrir `http://localhost:5101`.

### Edward

```powershell
cd .\Edward
dotnet run
```

Abrir `http://localhost:5102`.

### Axel

```powershell
cd .\Axel
dotnet run
```

Abrir `http://localhost:5103`.

### Tavo

```powershell
cd .\Tavo
dotnet run
```

Abrir `http://localhost:5104`.

### Sergio

```powershell
cd .\Sergio
dotnet run
```

Abrir `http://localhost:5105`.

## Forma de exponer

Para cada vulnerabilidad:

1. Explicar la función normal.
2. Mostrar el archivo vulnerable.
3. Señalar el comentario `VULNERABILIDAD`.
4. Ejecutar la prueba normal.
5. Manipular la entrada o request.
6. Mostrar el resultado inseguro.
7. Explicar por qué ocurre.
8. Abrir la versión segura.
9. Señalar el comentario `SOLUCIÓN`.
10. Repetir exactamente la misma prueba.
11. Comparar el resultado.
12. Explicar el control aplicado.

## Burp Suite

Los ejercicios que más aprovechan Burp Repeater son:

- Cristian: IDOR.
- Edward: CSRF.
- Axel: File Upload.
- Tavo: Broken Access Control y Mass Assignment.
- Sergio: JWT y SSRF.

Todo está diseñado para `localhost` con datos ficticios. No desplegar las versiones vulnerables a Internet.
