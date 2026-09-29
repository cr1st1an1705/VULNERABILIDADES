using CristianSecurityLab.Common;
using Microsoft.Data.Sqlite;

namespace CristianSecurityLab.Exercises;

public static class SqlInjectionSeguro
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/sql-injection/seguro",
            (
                string? search,
                IWebHostEnvironment environment
            ) =>
        {
            var rows = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                using var connection =
                    new SqliteConnection(
                        LabDatabase.ConnectionString(
                            environment.ContentRootPath));

                connection.Open();

                using var command = connection.CreateCommand();

                // ==========================================================
                // SOLUCIÓN: CONSULTA PARAMETRIZADA
                //
                // La consulta SQL y los datos proporcionados por el usuario
                // se envían por separado. La entrada ya no se interpreta
                // como sintaxis SQL.
                // ==========================================================
                command.CommandText =
                    """
SELECT Id, Username, Email, Department
FROM Users
WHERE Username LIKE @search
""";

                command.Parameters.AddWithValue(
                    "@search",
                    $"%{search}%");

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rows.Add(
                        $"""
<tr>
<td>{reader.GetInt32(0)}</td>
<td>{Html.E(reader.GetString(1))}</td>
<td>{Html.E(reader.GetString(2))}</td>
<td>{Html.E(reader.GetString(3))}</td>
</tr>
""");
                }
            }

            var result =
                rows.Count == 0
                ? "<div class='info'>Sin coincidencias. La entrada se trató como texto.</div>"
                : $"""
<table>
<tr><th>ID</th><th>Usuario</th><th>Email</th><th>Departamento</th></tr>
{string.Join(Environment.NewLine, rows)}
</table>
""";

            return Html.Result(
                "SQL Injection seguro",
                $"""
<h1>SQL Injection - Seguro</h1>
<div class="success">La consulta utiliza el parámetro <code>@search</code>.</div>
<form method="get">
<label>Buscar usuario</label>
<input name="search" value="{Html.E(search)}">
<button class="safe">Buscar</button>
</form>
<p>Repite exactamente: <code>' OR 1=1 --</code></p>
<pre>SELECT ... FROM Users WHERE Username LIKE @search</pre>
{result}
<p><a class="btn danger" href="/sql-injection/vulnerable">Volver a vulnerable</a></p>
""");
        });
    }
}
