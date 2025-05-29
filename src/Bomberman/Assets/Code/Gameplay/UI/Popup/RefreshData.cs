using System.Text;

namespace Gameplay.UI.Popup
{
	public struct RefreshData
	{
		public bool CloseButton;
		public string Title;
		public string Message;

		public static RefreshData ProcessPurchaseData() =>
			new()
			{
				Title = "Purchase process.",
				Message = "Purchase process.",
			};

		public static RefreshData NotEnoughCurrency() =>
			new()
			{
				Title = "Not enough currency.",
				Message = "You don't have enough currency.",
				CloseButton = true,
			};

		public static RefreshData PurchaseFailData(string[] itemsNames)
		{
			var messageBuilder = new StringBuilder();
			messageBuilder.Append("Could not purchase the following items:\n");
			foreach (var itemName in itemsNames)
				messageBuilder.Append($"{itemName}; ");

			return new RefreshData()
			{
				Title = "Purchase fail.",
				Message = messageBuilder.ToString(),
				CloseButton = true,
			};
		}
	}
}