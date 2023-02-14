namespace Unibrics.Utils.Json
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string raw);
    }

    class JsonDotNetSerializer : IJsonSerializer
    {
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw);
        }
    }
}