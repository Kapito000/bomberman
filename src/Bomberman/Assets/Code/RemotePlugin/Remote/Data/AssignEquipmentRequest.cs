using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemotePlugin.Remote.Data {
    public class AssignEquipmentRequest {
        [JsonProperty("SlotChanges")]
        public Dictionary<string, EquipInfo> SlotChanges;
    }

    public struct EquipInfo {
        public string ItemAlias;
        public EquipType EquipType;
    }

    public enum EquipType { Usual, DefaultItem }
}
