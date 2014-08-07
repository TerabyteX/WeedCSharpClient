using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WeedCSharpClient.Helper
{
    public static class JsonHelper
    {
        /// <summary>
        /// JSON Serialize
        /// </summary>
        public static string Serialize<T>(T t)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, t);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// JSON Deserialize
        /// </summary>
        public static T Deserialize<T>(string jsonStr)
        {
            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return default(T);
            }

            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}
