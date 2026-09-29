# Tavo — Security Lab

Puerto: `http://localhost:5104`

## Ejecutar

```powershell
cd Tavo
dotnet restore
dotnet build
dotnet run
```

## Los 4 ejercicios

1. Broken Access Control vulnerable
2. Broken Access Control seguro
3. Mass Assignment vulnerable
4. Mass Assignment seguro

### Broken Access Control
Usuario actual: `tavo`, rol: `Employee`. La versión vulnerable permite una operación administrativa; la segura responde 403.

### Mass Assignment
Intercepta el POST y agrega:

```text
IsAdmin=true&Role=Administrator&Balance=99999
```

La versión segura solo procesa Name, Email y Phone.
