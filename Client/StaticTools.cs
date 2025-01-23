using YGate.Entities;

namespace YGate.Client
{
    public static class StaticTools
    {

        /// <summary>
        /// Burası Result durumunu kontrol ediyor.
        /// Amacımız sadece objeyi almak ise sizin Result'u kontrol etmenize gerek yok.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static T ConvertRequestObject<T>(this RequestResult result)
        {
            if (result.Result == EnumRequestResult.Success)
                return YGate.Json.Operations.JsonDeserialize<T>.Deserialize(result.Object.ToString());
            else
                return default(T);
        }

        public static List<DateTime> MyRequestLog = new();

    }
}
