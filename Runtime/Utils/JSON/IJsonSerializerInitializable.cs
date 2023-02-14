namespace Unibrics.Utils.Json
{
    using Newtonsoft.Json;

    public interface IJsonSerializerInitializable
    {
        void Init(JsonSerializerSettings serializerSettings);
    }
}