namespace TavoSecurityLab.Common;

public sealed class AdminUser
{
    public int Id { get; init; }
    public string Username { get; init; } = "";
    public string Role { get; init; } = "";
    public bool Active { get; set; } = true;
}

public sealed class UserProfile
{
    public string Name { get; set; } = "Tavo";
    public string Email { get; set; } = "tavo@securitylab.local";
    public string Phone { get; set; } = "5555-0104";
    public bool IsAdmin { get; set; }
    public decimal Balance { get; set; } = 125.50m;
    public string Role { get; set; } = "User";
}

public static class LabState
{
    public const string CurrentUsername = "tavo";
    public const string CurrentRole = "Employee";

    public static readonly List<AdminUser> Users =
    [
        new() { Id = 1, Username = "cristian", Role = "Administrator" },
        new() { Id = 2, Username = "edward", Role = "Employee" },
        new() { Id = 3, Username = "axel", Role = "Employee" },
        new() { Id = 4, Username = "tavo", Role = "Employee" },
        new() { Id = 5, Username = "sergio", Role = "Auditor" }
    ];

    public static readonly UserProfile Profile = new();
}
