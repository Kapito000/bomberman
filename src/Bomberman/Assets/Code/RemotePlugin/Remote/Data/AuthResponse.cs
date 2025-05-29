using System;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class AuthResponse {
        public string SID;
        public ItemsData Data;

        public override string ToString() {
            return $"{nameof(SID)}: {SID}{Environment.NewLine}" +
                   $"{nameof(Data)}: {Data}";
        }
    }
}
