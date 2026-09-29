using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SergioSecurityLab.Common;

public static class JwtTools
{
    public const string RealKey =
        "SECURITY-LAB-REAL-KEY-LOCAL-DEMO-2026";

    public const string FakeKey =
        "ATTACKER-SIMULATED-KEY-LOCAL-DEMO-2026";

    public const string Issuer =
        "SecurityLab";

    public const string Audience =
        "SecurityLabStudents";

    public static string CreateToken(
        string user,
        string role,
        string key)
    {
        var header =
            JsonSerializer.Serialize(
                new
                {
                    alg = "HS256",
                    typ = "JWT"
                });

        var payload =
            JsonSerializer.Serialize(
                new
                {
                    sub = user,
                    role,
                    iss = Issuer,
                    aud = Audience,
                    exp =
                        DateTimeOffset.UtcNow
                            .AddMinutes(30)
                            .ToUnixTimeSeconds()
                });

        var encodedHeader =
            Base64UrlEncode(
                Encoding.UTF8.GetBytes(header));

        var encodedPayload =
            Base64UrlEncode(
                Encoding.UTF8.GetBytes(payload));

        var unsigned =
            $"{encodedHeader}.{encodedPayload}";

        return
            $"{unsigned}.{Sign(unsigned, key)}";
    }

    public static string Sign(
        string value,
        string key)
    {
        using var hmac =
            new HMACSHA256(
                Encoding.UTF8.GetBytes(key));

        return Base64UrlEncode(
            hmac.ComputeHash(
                Encoding.UTF8.GetBytes(value)));
    }

    public static byte[] Base64UrlDecode(
        string value)
    {
        var text =
            value
                .Replace('-', '+')
                .Replace('_', '/');

        text +=
            (text.Length % 4) switch
            {
                2 => "==",
                3 => "=",
                _ => string.Empty
            };

        return Convert.FromBase64String(text);
    }

    private static string Base64UrlEncode(
        byte[] value)
    {
        return Convert
            .ToBase64String(value)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
