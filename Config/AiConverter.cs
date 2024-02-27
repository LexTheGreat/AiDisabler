using System;
using AiDisabler.Config.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiDisabler.Config {
    public class AiConverter : JsonCreationConverter<Ai> {
        protected override Ai Create(Type objectType, JObject jObject) {
            if (FieldExists("Children", jObject)) {
                return new AiGroup();
            } else {
                return new Ai();
            }
        }

        private bool FieldExists(string fieldName, JObject jObject) {
            return jObject[fieldName] != null;
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter {
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType) {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer) {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
