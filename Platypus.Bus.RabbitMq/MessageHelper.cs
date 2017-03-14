using System.Text;
using Newtonsoft.Json;

namespace Platypus.Bus.RabbitMq
{
    public static class MessageHelper
    {
        public static T Convert<T>(this byte[] input)
        {
            var json = Encoding.UTF8.GetString(input);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] Convert(this object input)
        {
            var json = JsonConvert.SerializeObject(input);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}