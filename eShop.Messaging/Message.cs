using Newtonsoft.Json.Linq;

namespace eShop.Messaging
{
    internal class Message
    {
        public string Name { get; set; }

        public object Data { get; set; }

        public bool TryGetData(Type type, out object? result)
        {
            result = default;
            if (Data is JObject jObject)
            {
                result = jObject.ToObject(type);
            }
            else if (Data.GetType() == type)
            {
                result = Data;
            }
            return result != default;
        }
    }
}
