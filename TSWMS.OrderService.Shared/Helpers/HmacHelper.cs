namespace TSWMS.OrderService.Shared.Helpers;

public static class HmacHelper
{
    public static string GenerateHmac(byte[] messageBody, string secretKey)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var hash = hmac.ComputeHash(messageBody);
        return Convert.ToHexString(hash);
    }

    public static bool ValidateHmac(byte[] messageBody, string signature, string secretKey)
    {
        var expected = GenerateHmac(messageBody, secretKey);
        return string.Equals(expected, signature, StringComparison.OrdinalIgnoreCase);
    }
}
