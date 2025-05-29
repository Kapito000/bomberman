using UniquesMap = System.Collections.Generic.HashSet<string>;
using CountablesMap = System.Collections.Generic
	.Dictionary<string, RemotePlugin.Remote.Data.CountableItemData>;
using UsedItemsMap = System.Collections.Generic.Dictionary<string, int>;
using EquipmentMap = System.Collections.Generic.Dictionary<string, string>;

namespace RemotePlugin.Remote.Data
{
	public class UserData
	{
		// Server.
		public int BestScore;
		public string SessionId = string.Empty;
		public int RoundId = -1;
		public CountablesMap CountableItems = new();
		public UniquesMap UniqueItems = new();
		public EquipmentMap Equipment = new();
		public UsedItemsMap UsedItems = new();
		
		// Telegram.
		public TelegramData TelegramData;
	}
}