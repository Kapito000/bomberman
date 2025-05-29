namespace RemotePlugin.Remote.Data {
    public class FinishRoundResponse {
        public System.Collections.Generic.Dictionary<string, CountableItemRequestInfo> ResultItems { get; set; }
        public int? ResultScore;
        public int? ResultPPWR;
        public bool Ok;
    }
}
