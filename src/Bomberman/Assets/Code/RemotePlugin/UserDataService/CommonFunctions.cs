using RemotePlugin.Remote.Data;

namespace RemotePlugin.UserDataService
{
	public class CommonFunctions
	{
		readonly UserData _userData;

		public CommonFunctions(UserData userData)
		{
			_userData = userData;
		}
		
		public bool HasCountable(string id) =>
			_userData.CountableItems.ContainsKey(id);

		public bool HasUnique(string id) =>
			_userData.UniqueItems.Contains(id);

		public bool HasItem(string id) => 
			HasUnique(id) || HasCountable(id);

		public int GetItemCount(string id)
		{
			return _userData.CountableItems.TryGetValue(id,
				out CountableItemData info)
				? info.Amount
				: 0;
		}
		
		public int GetItemSpentAmount(string id)
		{
			return _userData.CountableItems.TryGetValue(id,
				out CountableItemData info)
				? info.Spent
				: 0;
		}
	}
}