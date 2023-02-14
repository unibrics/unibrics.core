namespace Unibrics.Utils.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Tools;

    public class AttributedObjectsConverter<TAttr, TClass> : JsonConverter, IJsonSerializerInitializable
        where TAttr : IdAttribute
    {
        private readonly List<(TAttr, Type)> attributes;

        private const string TypeKey = "type";

        protected virtual Type DefaultType() => null;

        private JsonSerializer cachedSerializer = new JsonSerializer()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
 
        public AttributedObjectsConverter()
        {
            attributes = Types.AnnotatedWith<TAttr>().ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value, cachedSerializer);
            var tuple = attributes.FirstOrDefault(valueTuple => valueTuple.Item2 == value.GetType());
            if (tuple != default)
            {
                jObject[TypeKey] = tuple.Item1.Id;
            }

            serializer.Serialize(writer, jObject);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var jToken = jObject[TypeKey];
            if (jToken == null)
            {
                return null;
            }

            var typeString = jToken.Value<string>();
            var tuple = attributes.FirstOrDefault(valueTuple => valueTuple.Item1.Id == typeString);
            if (tuple != default)
            {
                return Deserialize(tuple.Item2);
            }

            var defaultType = DefaultType();
            if (defaultType != null)
            {
                return Deserialize(defaultType);
            }

            return null;

            object Deserialize(Type type)
            {
                return jObject.ToObject(type, cachedSerializer);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TClass).IsAssignableFrom(objectType);
        }

        public void Init(JsonSerializerSettings serializerSettings)
        {
            cachedSerializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Converters = serializerSettings.Converters.Except(new[] {this}).ToList(),
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
        }
    }

    [MeansImplicitUse, AttributeUsage(AttributeTargets.Class)]
    public abstract class IdAttribute : Attribute
    {
        public string Id { get; }

        protected IdAttribute(string id)
        {
            Id = id;
        }
    }
}