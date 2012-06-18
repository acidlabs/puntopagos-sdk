using System.Collections.Generic;
using ServiceStack.Text;

namespace Acid.PuntoPagos.Sdk.Services
{
    public static class JsonSerializerService
    {
        public static IDictionary<string, string> DeserializeFromString(string json)
        {
            return JsonObject.Parse(json).ToDictionary();
        }
    }
}