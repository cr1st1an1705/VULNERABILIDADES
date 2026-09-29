using CristianSecurityLab.Common;
using Microsoft.Data.Sqlite;

namespace CristianSecurityLab.Exercises;

public static class SqlInjectionVulnerable
{
    public static void Map(WebApplication app)
    {
        app.MapGet(
            "/sql-injection/vulnerable",
            (
                string? search,
                IWebHostEnvironment environment
            ) =>
        {
            var rows = new List<string>();
            var executedSql = string.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                using var connection =
                    new SqliteConnection(
                        LabDatabase.ConnectionString(
                            environment.ContentRootPath));

                connection.Open();

                using var command = connection.CreateCommand();

                // ==========================================================
                // VULNERABILIDAD: SQL INJECTION
                //
                // La entrada del usuario se concatena directamente dentro
                // de la consulta SQL. Esto permite alterar su estructura.
                // ==========================================================
                command.CommandText =
                    $"SELECT Id, Username, Email, Department " +
                    $"FROM Users WHERE Username LIKE '%{search}%'";

                executedSql = command.CommandText;

                try
                {
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
                catch (Exception ex)
                {
                    rows.Add(
                        $"<tr><td colspan='4'>{Html.E(ex.Message)}</td></tr>");
                }
            }

            var table =
                rows.Count == 0
                ? string.Empty
                : $"""
<table>
<tr><th>ID</th><th>Usuario</th><th>Email</th><th>Departamento</th></tr>
{string.Join(Environment.NewLine, rows)}
</table>
""";

            return Html.Result(
                "SQL Injection vulnerable",
                $"""
<h1>SQL Injection - Vulnerable</h1>
<div class="warning">El valor recibido se concatena directamente dentro del SQL.</div>
<form method="get">
<label>Buscar usuario</label>
<input name="search" value="{Html.E(search)}" placeholder="cristian">
<button class="danger">Buscar</button>
</form>
<p>Entrada normal: <code>cristian</code></p>
<p>Prueba local: <code>' OR 1=1 --</code></p>
<h3>SQL ejecutado</h3>
<pre>{Html.E(executedSql)}</pre>
{table}
<p><a class="btn safe" href="/sql-injection/seguro">Abrir versión segura</a></p>
""");
        });
    }
}
