using System;
using System.Text;

namespace TaskBook.Tools
{
    static class Base64Converter
    {
        public static string FromBase64(string base64)
        {
            var data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }

        public static string ToBase64(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

    }
}
