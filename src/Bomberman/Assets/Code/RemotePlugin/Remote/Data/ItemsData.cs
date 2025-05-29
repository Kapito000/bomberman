using System;
using System.Collections.Generic;

namespace RemotePlugin.Remote.Data {
    [Serializable]
    public class ItemsData {
        static readonly System.Text.StringBuilder _stringBuilder = new(512);

        public int BestScore;
        public Dictionary<string, CountableItemRequestInfo> CommonItems;
        public List<string> UniqueItems;
        public Dictionary<string, string> Equipment;

        public override string ToString() {
            var stringBuilder = _stringBuilder;
            stringBuilder.Clear();
            stringBuilder.Append($"{nameof(CommonItems)}: {Environment.NewLine}");

            if (CommonItems != null && CommonItems.Count > 0) {
                stringBuilder.AppendJoin(", ", CommonItems);
            } else {
                stringBuilder.Append($"None{Environment.NewLine}");
            }

            stringBuilder.Append($"{nameof(UniqueItems)}: {Environment.NewLine}");

            if (UniqueItems != null && UniqueItems.Count > 0) {
                stringBuilder.AppendJoin(", ", UniqueItems);
            } else {
                stringBuilder.Append($"None{Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }
    }
}
