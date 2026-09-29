namespace AxelSecurityLab.Common;

public static class LabFiles
{
    public static void Initialize(string root)
    {
        var reports = Path.Combine(root, "Data", "Reports");
        var secrets = Path.Combine(root, "Data", "Secrets");
        var safeUploads = Path.Combine(root, "Data", "SafeUploads");
        var publicUploads = Path.Combine(root, "wwwroot", "uploads");

        Directory.CreateDirectory(reports);
        Directory.CreateDirectory(secrets);
        Directory.CreateDirectory(safeUploads);
        Directory.CreateDirectory(publicUploads);

        File.WriteAllText(
            Path.Combine(reports, "report-september.txt"),
            "Reporte público de septiembre.");

        File.WriteAllText(
            Path.Combine(secrets, "internal-secret.txt"),
            "LAB-SECRET-AXEL: información simulada fuera del directorio de reportes.");
    }
}
