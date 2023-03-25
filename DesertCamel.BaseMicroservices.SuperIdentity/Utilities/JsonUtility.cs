using Newtonsoft.Json;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Utilities
{
    public static class JsonUtility
    {
        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }
    }
}
