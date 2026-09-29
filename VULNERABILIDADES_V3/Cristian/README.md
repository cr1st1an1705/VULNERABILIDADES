# Cristian — Security Lab

Puerto: `http://localhost:5101`

## Ejecutar

```powershell
cd Cristian
dotnet restore
dotnet build
dotnet run
```

## Los 4 ejercicios

1. `01_SQLInjection_Vulnerable`
2. `02_SQLInjection_Seguro`
3. `03_IDOR_Vulnerable`
4. `04_IDOR_Seguro`

## SQL Injection

Entrada normal: `cristian`

Prueba local:

```text
' OR 1=1 --
```

Qué decir:

> La vulnerabilidad aparece porque la aplicación concatena la entrada dentro del SQL. La solución separa la instrucción de los datos mediante un parámetro.

## IDOR

Usuario simulado: Cristian, `OwnerId = 1`.

Prueba `id=101` y luego cambia a `id=102`.

Qué decir:

> El servidor vulnerable comprueba que el recurso exista, pero no que pertenezca al usuario. La versión segura realiza autorización a nivel de objeto y responde 403 cuando el propietario no coincide.
