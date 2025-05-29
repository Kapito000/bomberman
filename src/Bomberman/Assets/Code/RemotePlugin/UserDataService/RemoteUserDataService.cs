using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using RemotePlugin.Remote;
using RemotePlugin.Remote.Data;
using UnityEngine;
using Environment = System.Environment;
using TelegramData = RemotePlugin.Remote.Data.TelegramData;
using EquipmentChanges =
	System.Collections.Generic.Dictionary<string,
		RemotePlugin.Remote.Data.EquipInfo>;

namespace RemotePlugin.UserDataService
{
	public class RemoteUserDataService : IUserDataService
	{
		readonly UserData _userData;
		readonly IExceptionHandler _exceptionHandler;

		public RemoteUserDataService(UserData userData,
			IExceptionHandler exceptionHandler)
		{
			if (exceptionHandler == null)
			{
				Debug.LogError($"{nameof(IExceptionHandler)} was not provided!");
				return;
			}

			_exceptionHandler = exceptionHandler;

			if (userData == null)
			{
				_exceptionHandler.HandleException(
					$"{nameof(UserData)} was not provided!");
			}

			_userData = userData;
		}

		public IExceptionHandler ExceptionHandler => _exceptionHandler;
		public TelegramData TelegramData => _userData.TelegramData;
		public int LatestScore { get; set; }

		public bool HasCountable(string id)
		{
			return _userData.CountableItems.ContainsKey(id);
		}

		public bool HasUnique(string id)
		{
			return _userData.UniqueItems.Contains(id);
		}

		public bool HasItem(string id)
		{
			return HasUnique(id) || HasCountable(id);
		}

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

		public async UniTask<bool> TryPurchaseCountableItem(string id, int amount,
			string priceId, double priceAmount)
		{
			(PurchaseCountableResponse PurchaseCountableResponse, Exception Exception)
				result =
					await Server.RequestCountablePurchase(_userData.SessionId,
						new PurchaseCountableItemRequest
						{
							ItemId = id.ToLower(),
							Amount = amount,
							PriceId = priceId.ToLower(),
							PriceAmount = (int)priceAmount
						});

			if (result.Exception != null)
			{
				_exceptionHandler.HandleException(result.Exception.Message);
				return false;
			}

			if (result.PurchaseCountableResponse?.Payload == null)
			{
				_exceptionHandler.HandleException(
					$"{nameof(PurchaseCountableResponse)} was null");
				return false;
			}

			// TODO: Change to enum.Success
			if (result.PurchaseCountableResponse.ResultCode != 0)
			{
				_exceptionHandler.HandleException(
					$"Purchase failed with result code: {result.PurchaseCountableResponse.ResultCode}{Environment.NewLine}" +
					$"Payload: {result.PurchaseCountableResponse.Payload}{Environment.NewLine}");
				return false;
			}

			if (!_userData.CountableItems.TryGetValue(id,
				    out CountableItemData itemData))
			{
				itemData = new CountableItemData
				{
					Amount = result.PurchaseCountableResponse.Payload.ItemAmount,
					Spent = 0
				};

				_userData.CountableItems.Add(id, itemData);
			}

			_userData.CountableItems[id].Amount =
				result.PurchaseCountableResponse.Payload.ItemAmount;
			_userData.CountableItems[priceId.ToLower()].Amount =
				result.PurchaseCountableResponse.Payload.PriceAmount;
			return true;
		}

		public async UniTask<bool> TryPurchaseUniqueItem(string id, string priceId,
			double priceAmount)
		{
			(PurchaseUniqueItemResponse PurchaseUniqueResponse, Exception Exception)
				result =
					await Server.RequestUniquePurchase(_userData.SessionId,
						new PurchaseUniqueItemRequest
						{
							ItemId = id.ToLower(),
							PriceId = priceId.ToLower(),
							PriceAmount = (int)priceAmount
						});

			if (result.Exception != null)
			{
				_exceptionHandler.HandleException(result.Exception.Message);
				return false;
			}

			if (result.PurchaseUniqueResponse?.Payload == null)
			{
				_exceptionHandler.HandleException(
					$"{nameof(PurchaseCountableResponse)} was null");
				return false;
			}

			// TODO: Change to enum.Success
			if (result.PurchaseUniqueResponse.ResultCode != 0)
			{
				_exceptionHandler.HandleException(
					$"Purchase failed with result code: {result.PurchaseUniqueResponse.ResultCode}{Environment.NewLine}" +
					$"Payload: {result.PurchaseUniqueResponse.Payload}{Environment.NewLine}");
				return false;
			}

			if (_userData.UniqueItems.Contains(id))
			{
				_exceptionHandler.HandleException(
					$"Failed to purchase unique item '{id}': already obtain item");
				return false;
			}

			_userData.UniqueItems.Add(id);
			_userData.CountableItems[priceId.ToLower()].Amount =
				result.PurchaseUniqueResponse.Payload.PriceAmount;
			return true;
		}

		public async UniTask<bool> TryPurchaseBundle(
			Dictionary<(string ItemId, int Amount), (string PriceId, double
				PriceAmount)> commonItems,
			Dictionary<string, (string PriceId, double PriceAmount)> uniqueItems)
		{
			(PurchaseBundleResponse PurchaseBundleResponse, Exception Exception)
				result =
					await Server.RequestBundlePurchase(_userData.SessionId,
						new PurchaseBundleRequest
						{
							CommonItems = commonItems.Select(x =>
								new PurchaseCountableItemRequest
								{
									ItemId = x.Key.ItemId.ToLower(),
									Amount = x.Key.Amount,
									PriceId = x.Value.PriceId.ToLower(),
									PriceAmount = (int)x.Value.PriceAmount,
								}).ToArray(),
							UniqueItems = uniqueItems.Select(x =>
								new PurchaseUniqueItemRequest
								{
									ItemId = x.Key.ToLower(),
									PriceId = x.Value.PriceId.ToLower(),
									PriceAmount = (int)x.Value.PriceAmount,
								}).ToArray(),
						});

			if (result.Exception != null)
			{
				_exceptionHandler.HandleException(result.Exception.Message);
				return false;
			}

			if (result.PurchaseBundleResponse?.CommonItems == null
			    || result.PurchaseBundleResponse.UniqueItems == null)
			{
				_exceptionHandler.HandleException(
					$"{nameof(PurchaseBundleResponse)} was null");
				return false;
			}

			foreach (PurchaseCountableResponse countableResponse
			         in result.PurchaseBundleResponse.CommonItems)
			{
				// TODO: Change to enum.Success
				if (countableResponse.ResultCode != 0)
				{
					_exceptionHandler.HandleException(
						$"Purchase failed with result code: {countableResponse.ResultCode}{Environment.NewLine}" +
						$"Payload: {countableResponse.Payload}{Environment.NewLine}");
					return false;
				}

				if (!_userData.CountableItems.TryGetValue(countableResponse.Payload.Id,
					    out CountableItemData itemData))
				{
					itemData = new CountableItemData
					{
						Amount = countableResponse.Payload.ItemAmount,
						Spent = 0
					};

					_userData.CountableItems.Add(countableResponse.Payload.Id, itemData);
				}

				_userData.CountableItems[countableResponse.Payload.Id].Amount =
					countableResponse.Payload.ItemAmount;
				_userData.CountableItems[countableResponse.Payload.PriceId].Amount =
					countableResponse.Payload.PriceAmount;
			}

			foreach (PurchaseUniqueItemResponse uniqueResponse in result
				         .PurchaseBundleResponse.UniqueItems)
			{
				// TODO: Change to enum.Success
				if (uniqueResponse.ResultCode != 0)
				{
					_exceptionHandler.HandleException(
						$"Purchase failed with result code: {uniqueResponse.ResultCode}{Environment.NewLine}" +
						$"Payload: {uniqueResponse.Payload}{Environment.NewLine}");
					return false;
				}

				if (_userData.UniqueItems.Contains(uniqueResponse.Payload.Id))
				{
					_exceptionHandler.HandleException(
						$"Failed to purchase unique item '{uniqueResponse.Payload.Id}': already obtain item");
					return false;
				}

				_userData.UniqueItems.Add(uniqueResponse.Payload.Id);
				_userData.CountableItems[uniqueResponse.Payload.PriceId.ToLower()]
					.Amount = uniqueResponse.Payload.PriceAmount;
			}

			return true;
		}

		public async UniTask<bool> TryStartRound()
		{
			(StartRoundResponse StartRoundResponse, Exception Exception) result =
				await Server.RequestStartRound(_userData.SessionId);

			if (result.Exception != null)
			{
				_exceptionHandler.HandleException(result.Exception.Message);
				return false;
			}

			if (result.StartRoundResponse == null)
			{
				_exceptionHandler.HandleException(
					$"{nameof(PurchaseCountableResponse)} was null");
				return false;
			}

			_userData.RoundId = result.StartRoundResponse.RunId;
			return true;
		}

		public async UniTask<bool> TryFinishRound()
		{
			var (result, error) =
				await Server.RequestFinishRound(_userData.SessionId,
					new FinishRoundRequest
					{
						RoundId = _userData.RoundId,
						Score = LatestScore,
						UsedItems = _userData.UsedItems
					});

			if (error != null)
			{
				_exceptionHandler.HandleException(error.Message);
				return false;
			}

			_userData.RoundId = -1;
			_userData.UsedItems.Clear();
			foreach (var i in result.ResultItems)
			{
				_userData.CountableItems[i.Key] = new()
				{
					Amount = i.Value.Amount,
					Spent = i.Value.Spent,
				};
			}

			LatestScore = 0;
			return true;
		}

		public void UseItem(string id, int amount)
		{
			if (_userData.UsedItems.ContainsKey(id))
			{
				_userData.UsedItems[id] += amount;
				return;
			}

			_userData.UsedItems.Add(id, amount);
		}

		public bool HasEquipment(string slotID, out string itemID)
		{
			return _userData.Equipment.TryGetValue(slotID, out itemID);
		}

		// TODO: change return value to more informative
		public async UniTask<bool> EquipItems(EquipmentChanges changes)
		{
			var (result, error) = await Server.RequestAssignEquipment(
				_userData.SessionId, new()
				{
					SlotChanges = changes,
				});
			if (error == null)
			{
				// foreach(var c in result.ResultCode)
				foreach (var c in changes)
				{
					if (c.Value.EquipType == EquipType.DefaultItem)
					{
						_userData.UniqueItems.Add(c.Value.ItemAlias);
					}

					_userData.Equipment[c.Key] = c.Value.ItemAlias;
				}
			}

			return NoProblem(error);
		}

		public async UniTask<LeaderboardResponse> GetLeaderboard(bool askPlayer,
			DateTime since, int limit, int offset = 0)
		{
			var (result, error) =
				await Server.RequestLeaderboard(limit, offset, since);
			if (error != null)
			{
				return result;
			}

			if (!askPlayer && _userData.TelegramData == null)
			{
				return result;
			}

			var playerTgID = _userData.TelegramData.ID;
			var playerIdx =
				Array.FindIndex(result.Leaders, f => f.TelegramID == playerTgID);
			if (playerIdx < 0)
			{
				var (result2, error2) =
					await Server.RequestUserPlaceInfo(playerTgID, since);
				result.UserInfo = result2.UserInfo;
			}
			else
			{
				result.UserInfo = result.Leaders[playerIdx];
			}

			return result;
		}

		bool NoProblem(Exception error)
		{
			if (error != null)
			{
				_exceptionHandler?.HandleException(error.Message);
			}

			return error == null;
		}
	}
}