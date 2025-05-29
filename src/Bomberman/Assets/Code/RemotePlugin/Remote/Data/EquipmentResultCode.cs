namespace RemotePlugin.Remote.Data {
    public enum EquipmentResultCode : byte {
        Equipped = 0,
        AlreadyInSlot = 1,
        Havent = 2,
        NoSuchItem = 3,
        WrongGame = 4,
        UnknownError = 255
    }
}
