using System.Text;
using Newtonsoft.Json;

namespace NeatCoin
{
    public static class JsonExtensions
    {
        public static string ToJson(this object o) =>
            JsonConvert.SerializeObject(o);

        public static byte[] ToByteArray2(this string @string) =>
            Encoding.Default.GetBytes(@string);
    }
}