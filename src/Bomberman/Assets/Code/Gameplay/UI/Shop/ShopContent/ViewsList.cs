using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI.Shop.ShopContent
{
	public abstract class ViewsList<TViewItem, TItem> : MonoBehaviour
	{
		readonly List<TViewItem> _itemsViews = new();

		protected List<TViewItem> ItemsViews() =>
			_itemsViews;
		
		protected abstract TViewItem CreateItemView();
		protected abstract void HideItemView(int index);
		protected abstract void ShowItemView(int index);
		protected abstract void RefreshItemView(int index, TItem item);

		protected void Refresh(TItem[] items)
		{
			var dif = ItemsViews().Count - items.Length;
			CreateMissingItemsViews(dif);
			HideExcessItemsViews(items.Length, dif);
			RefreshViews(items);
		}

		void CreateMissingItemsViews(int countDif)
		{
			if (countDif < 0)
				for (int i = 0; i < -countDif; i++)
				{
					var itemView = CreateItemView();
					_itemsViews.Add(itemView);
				}
		}

		void HideExcessItemsViews(int itemsCount, int countDif)
		{
			if (countDif > 0)
				for (int i = _itemsViews.Count - 1; i > itemsCount - 1; i--)
					HideItemView(i);
		}

		void RefreshViews(TItem[] items)
		{
			for (var i = 0; i < items.Length; i++)
			{
				var item = items[i];
				RefreshItemView(i, item);
				ShowItemView(i);
			}
		}
	}
}