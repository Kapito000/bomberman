using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using MetaSystem.Telegram;
using RemotePlugin.Remote.Data;
using UnityEngine;
using TelegramData = RemotePlugin.Remote.Data.TelegramData;
using UserData = RemotePlugin.Remote.Data.UserData;

namespace RemotePlugin.Remote
{
	public sealed class UserDataLoader
	{
		const float c_serverLoadingMaxValue = 1f;
		const string c_testInitData =
			"user=%7B%22id%22%3A491717355%2C%22first_name%22%3A%22Egor%22%2C%22last" +
			"_name%22%3A%22Kapitanov%22%2C%22username%22%3A%22Egor_Kap%22%2C%22lang" +
			"uage_code%22%3A%22en%22%2C%22allows_write_to_pm%22%3Atrue%2C%22photo_u" +
			"rl%22%3A%22https%3A%5C%2F%5C%2Ft.me%5C%2Fi%5C%2Fuserpic%5C%2F320%5C%2F" +
			"gRz7eaKEvS6GJWaMktUPTYN7LxUQYqFAHijDPxx7jcs.svg%22%7D&chat_instance=-9" +
			"197050329908776859&chat_type=sender&auth_date=1739365605&signature=8r2" +
			"eV7lsUzvM_wbFKKSg5tFUPOH0Amr5qHRHbVmMAa6MqnWAv1DZj86e8vfPAGLbi0SZ_amBz" +
			"hRj3KrOEgBdDQ&hash=d81aa6a59ae433a21753e7f5caf96acc2b7bede833e33f5a936" +
			"221946c8bd4b5";

		float _serverProgress;

		public async UniTask<UserData> LoadUserDataAsync()
		{
			var userId = TelegramBridge.GetUserId();

			if (Int64.TryParse(userId, out var telegramID) == false)
			{
				Debug.LogError("Cannot parse Telegram ID to UserData");
				telegramID = -1;
			}

			return await LoadUserData(telegramID);
		}

		async UniTask<UserData> LoadUserData(Int64 telegramID)
		{
			UserData userData = await LoadServerDataAsync(telegramID);
			// TODO: Get from js
			var userName = "";
			try
			{
				userName = TelegramBridge.GetUserName();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}

			if (string.IsNullOrEmpty(userName))
			{
				userName = TelegramData.FormatUserNameFromID(telegramID);
			}

			userData.TelegramData = new()
			{
				ID = telegramID,
				NickName = userName,
				Avatar = null,
			};
			return userData;
		}

		async UniTask<UserData> LoadServerDataAsync(Int64 telegramID)
		{
			(Data.AuthResponse AuthResponse, Exception Exception) data =
				await Server.RequestAuth(new AuthRequest
				{
					UserId = telegramID,
					GameId = Constants.GameId,
#if UNITY_EDITOR
					InitData = c_testInitData,
#elif !UNITY_EDITOR
					InitData = TelegramBridge.GetInitData(),
#endif
				});

			if (data.Exception != null)
			{
				Services.ExceptionHandler.HandleException(data.Exception.Message);
				return null;
			}

			if (data.AuthResponse == null ||
			    string.IsNullOrEmpty(data.AuthResponse.SID) ||
			    data.AuthResponse.Data == null)
			{
				Services.ExceptionHandler.HandleException(
					$"Auth data has empty required field(s):{Environment.NewLine}" +
					$"{data.AuthResponse}");
				return null;
			}

			data.AuthResponse.Data.CommonItems ??= new();
			data.AuthResponse.Data.Equipment ??= new();
			data.AuthResponse.Data.UniqueItems ??= new();
			UserData userData = new UserData
			{
				BestScore = data.AuthResponse.Data.BestScore,
				SessionId = data.AuthResponse.SID,
				CountableItems = data.AuthResponse.Data.CommonItems.ToDictionary(
					item => item.Key,
					item => new CountableItemData
					{
						Amount = item.Value.Amount,
						Spent = item.Value.Spent
					}),
				UniqueItems = new(data.AuthResponse.Data.UniqueItems),
				Equipment = data.AuthResponse.Data.Equipment,
			};

			_serverProgress = c_serverLoadingMaxValue;
			return userData;
		}
	}
}