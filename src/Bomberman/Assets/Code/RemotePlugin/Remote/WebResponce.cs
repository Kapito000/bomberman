using UnityEngine.Networking;

namespace RemotePlugin.Remote {
    public struct WebResponse {
        // MIME application/json data format is assumed
        public long Code;
        public UnityWebRequest.Result Result;
        public string Data;

        public bool IsSuccess() => Result == UnityWebRequest.Result.Success;
    }
}
