// ReSharper disable once CheckNamespace

namespace System
{
    public static class StringUriExtensions
    {
        public static string NormalizeHttpUri(this string uri)
        {
            var normalized = uri.Replace("\\", "/");

            if (normalized.Length>1 && normalized.EndsWith("/"))
            {
                normalized = normalized.Substring(0, normalized.Length - 1);
            }
            if (normalized.Length>1 && normalized.StartsWith("/"))
            {
                normalized = normalized.Substring(1, normalized.Length - 1);
            }
            if (string.IsNullOrEmpty(normalized))
            {
                normalized = "";
            }
            return normalized;
        }
    }
}