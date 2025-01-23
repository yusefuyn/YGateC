using Newtonsoft.Json;

namespace YGate.Json.Operations
{
    public static class JsonDeserialize<T>
    {
        public static T Deserialize(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T Deserialize(object obj)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(obj.ToString());
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T Deserialize(byte[] value)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }

        }
    }
}
