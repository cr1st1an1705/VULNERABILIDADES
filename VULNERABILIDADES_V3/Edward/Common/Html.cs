using System.Net;

namespace EdwardSecurityLab.Common;

public static class Html
{
    public static string E(string? value) =>
        WebUtility.HtmlEncode(value ?? string.Empty);

    public static IResult Result(
        string title,
        string body,
        int statusCode = 200)
    {
        return Results.Content(
            Page(title, body),
            "text/html; charset=utf-8",
            statusCode: statusCode);
    }

    public static string Page(
        string title,
        string body)
    {
        const string template = """
<!doctype html>
<html lang="es">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>__TITLE__ - SecurityLab</title>
<style>
* { box-sizing: border-box; }
body { margin:0; font-family:Arial,Helvetica,sans-serif; background:#f5f6f8; color:#1f2937; }
nav { background:#111827; color:white; padding:16px 24px; }
nav a { color:white; text-decoration:none; font-weight:700; margin-right:18px; }
main { max-width:1050px; margin:28px auto; padding:0 18px; }
.grid { display:grid; grid-template-columns:repeat(auto-fit,minmax(280px,1fr)); gap:18px; }
.card { background:white; border:1px solid #d9dee5; border-radius:10px; padding:18px; margin-bottom:16px; }
input, textarea { width:100%; padding:9px; margin:6px 0 12px; border:1px solid #b7c0ca; border-radius:5px; }
button, .btn { display:inline-block; border:0; border-radius:6px; padding:10px 14px; margin:4px; cursor:pointer; text-decoration:none; background:#1f2937; color:white; }
.danger { background:#991b1b !important; }
.safe { background:#166534 !important; }
.warning { padding:12px; border-left:4px solid #b91c1c; background:#fef2f2; margin:14px 0; }
.success { padding:12px; border-left:4px solid #15803d; background:#f0fdf4; margin:14px 0; }
.info { padding:12px; border-left:4px solid #1d4ed8; background:#eff6ff; margin:14px 0; }
pre { white-space:pre-wrap; background:#111827; color:#e5e7eb; padding:14px; border-radius:8px; overflow:auto; }
code { background:#e5e7eb; padding:2px 5px; border-radius:4px; }
table { width:100%; border-collapse:collapse; background:white; }
th, td { padding:9px; border:1px solid #d9dee5; text-align:left; }
</style>
</head>
<body>
<nav><a href="/">SecurityLab</a>__MEMBER__</nav>
<main>__BODY__</main>
</body>
</html>
""";

        return template
            .Replace("__TITLE__", E(title))
            .Replace("__MEMBER__", "Edward")
            .Replace("__BODY__", body);
    }
}
