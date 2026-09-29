namespace EdwardSecurityLab.Common;

public sealed record StoredComment(
    int Id,
    string Author,
    string Message);

public static class LabState
{
    public static readonly List<StoredComment> Comments =
    [
        new(1, "system", "Comentario inicial del laboratorio.")
    ];

    public static string Email { get; set; } =
        "edward@securitylab.local";
}
