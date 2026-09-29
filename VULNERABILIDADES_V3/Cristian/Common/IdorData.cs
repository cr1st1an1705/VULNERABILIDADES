namespace CristianSecurityLab.Common;

public sealed record LabDocument(
    int Id,
    int OwnerId,
    string Name,
    string Content);

public static class IdorData
{
    public const int CurrentUserId = 1;

    public static readonly IReadOnlyList<LabDocument> Documents =
    [
        new(101, 1, "cristian-project.txt", "Documento privado de Cristian."),
        new(102, 2, "edward-network.txt", "Documento privado de Edward."),
        new(103, 1, "cristian-notes.txt", "Segundo documento privado de Cristian."),
        new(104, 3, "axel-support.txt", "Documento privado de Axel.")
    ];
}
