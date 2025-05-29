using UnityEngine;

namespace RemotePlugin.Remote.Data {
    public class TelegramData {
        public long ID;
        public string NickName;
        public Sprite Avatar;

        public static string FormatUserNameFromID(long id) {
            var stringedTgID = id.ToString("D4");
            return $"user_{stringedTgID[..2]}{stringedTgID[^2..]}";
        }
    }
}
