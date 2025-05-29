namespace RemotePlugin.Remote.Data {
    public class LeaderboardResponse {
        public struct Item {
            public long TelegramID;
            public int Place;
            public long Score;
        }

        public Item[] Leaders = { };
        public Item UserInfo;
    }
}
