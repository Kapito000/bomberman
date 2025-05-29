namespace RemotePlugin.Remote.Data {
    public enum ResponseResult {
        Success,        // All right
        LimitReached,
        ReqmtsNotMet,   // Already has or smth like that
        NSF,            // Non-Sufficient Funds
        Error           // Something went wrong
    }
}
