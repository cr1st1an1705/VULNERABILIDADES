using Microsoft.Data.Sqlite;

namespace CristianSecurityLab.Common;

public static class LabDatabase
{
    public static string ConnectionString(string root)
    {
        var data = Path.Combine(root, "Data");
        Directory.CreateDirectory(data);
        return $"Data Source={Path.Combine(data, "cristian-securitylab.db")}";
    }

    public static void Initialize(string root)
    {
        using var connection =
            new SqliteConnection(ConnectionString(root));

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY,
    Username TEXT NOT NULL,
    Email TEXT NOT NULL,
    Department TEXT NOT NULL
);

DELETE FROM Users;

INSERT INTO Users VALUES (1,'cristian','cristian@securitylab.local','Development');
INSERT INTO Users VALUES (2,'edward','edward@securitylab.local','Infrastructure');
INSERT INTO Users VALUES (3,'axel','axel@securitylab.local','Support');
INSERT INTO Users VALUES (4,'tavo','tavo@securitylab.local','QA');
INSERT INTO Users VALUES (5,'sergio','sergio@securitylab.local','Security');
""";

        command.ExecuteNonQuery();
    }
}
