using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace ProfitMiner.Core
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj, bool indented = false)
        {
            return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
        }
        public static T FromJson<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}