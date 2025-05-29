using Newtonsoft.Json;
using UnityEngine;

namespace RemotePlugin.Remote.Convert {
    public class JsonService {
        public string Serialize(object obj) {
            return JsonConvert.SerializeObject(obj);
        }

        public (T, System.Exception) DeserializeSafe<T>(string json) {
            try {
                return (Deserialize<T>(json), null);
            } catch (System.Exception exception) {
#if DEBUG
                Debug.LogError($"Deserialization failed: {exception.Message}");
#endif

                return (default, exception);
            }
        }

        private T Deserialize<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
