using System.Web;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Utilities
{
    public static class Base64Utility
    {
        public static string ToBase64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            var encoded = System.Convert.ToBase64String(plainTextBytes);
            return HttpUtility.UrlEncode(encoded);
        }

        public static string ToBase64Decode(this string encodedText)
        {
            var unencoded = HttpUtility.UrlDecode(encodedText);
            var base64EncodedBytes = System.Convert.FromBase64String(unencoded);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
