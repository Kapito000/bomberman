namespace RemotePlugin.Remote.Data {
    public enum DeliveryResult : byte {
        Succes = 0,
        LimitReached = 1,
        RequirementNotMet = 2,
        CantAfford = 3,
        UnknownError = 255,
    }
}
