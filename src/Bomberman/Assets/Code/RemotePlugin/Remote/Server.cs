using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using RemotePlugin.Remote.Convert;
using RemotePlugin.Remote.Data;
using RemotePlugin.Remote.Security;
using UnityEngine;
using UnityEngine.Networking;

namespace RemotePlugin.Remote
{
	public static class Server
	{
		static readonly JsonService JsonService = new JsonService();

		public static async
			UniTask<(AuthResponse AuthResponse, Exception Exception)> RequestAuth(
				AuthRequest authRequest)
		{
			string content = JsonService.Serialize(authRequest);
			WebResponse response = await SendRequestSafe(
				CreatePostRequest(
					Constants.Path.AuthUrl,
					content,
					Constants.Header.AuthHeaders()
				)
			);

			return JsonService.DeserializeSafe<AuthResponse>(response.Data);
		}

		public static async
			UniTask<(PurchaseCountableResponse PurchaseCountableResponse, Exception
				Exception)> RequestCountablePurchase(
				string sessionId, PurchaseCountableItemRequest requestData,
				bool useEncryption = false)
		{
			WebResponse response = await SendDefaultPostRequestSafe(
				Constants.Path.PurchaseCountableUrl, sessionId,
				requestData, useEncryption);

			return JsonService.DeserializeSafe<PurchaseCountableResponse>(
				response.Data);
		}

		public static async
			UniTask<(PurchaseUniqueItemResponse PurchaseUniqueItemResponse, Exception
				Exception)> RequestUniquePurchase(
				string sessionId, PurchaseUniqueItemRequest requestData,
				bool useEncryption = false)
		{
			WebResponse response = await SendDefaultPostRequestSafe(
				Constants.Path.PurchaseUniqueUrl, sessionId,
				requestData, useEncryption);

			return JsonService.DeserializeSafe<PurchaseUniqueItemResponse>(
				response.Data);
		}

		public static async
			UniTask<(PurchaseBundleResponse PurchaseBundleResponse, Exception
				Exception)> RequestBundlePurchase(
				string sessionId, PurchaseBundleRequest requestData,
				bool useEncryption = false)
		{
			WebResponse response = await SendDefaultPostRequestSafe(
				Constants.Path.PurchaseBundleUrl, sessionId,
				requestData, useEncryption);

			return JsonService.DeserializeSafe<PurchaseBundleResponse>(response.Data);
		}

		public static async
			UniTask<(AssignEquipmentResponse AssignEquipmentResponse, Exception
				Exception)> RequestAssignEquipment(
				string sessionId,
				AssignEquipmentRequest requestData, bool useEncryption = false)
		{
			WebResponse response = await SendDefaultPostRequestSafe(
				Constants.Path.EquipUrl, sessionId,
				requestData, useEncryption);

			return JsonService
				.DeserializeSafe<AssignEquipmentResponse>(response.Data);
		}

		// returns common leaders table, not userwise
		public static async
			UniTask<(LeaderboardResponse response, Exception exception)>
			RequestLeaderboard(int limit, int offset, DateTime since)
		{
			var response = await SendRequestSafe(CreateGetRequest(
				Constants.Path.GetLeaderboardUrl,
				UrlParam(nameof(since), since.ToString("yyyy-MM-dd")),
				UrlParam(nameof(limit), limit),
				UrlParam(nameof(offset), offset)
			));
			return JsonService.DeserializeSafe<LeaderboardResponse>(response.Data);
		}

		// only returns users place in current leaders table
		public static async
			UniTask<(LeaderboardResponse response, Exception exception)>
			RequestUserPlaceInfo(long telegramID, DateTime since)
		{
			var response = await SendRequestSafe(CreateGetRequest(
				Constants.Path.GetLeaderboardUserUrl(telegramID),
				UrlParam(nameof(since), since.ToString("yyyy-MM-dd"))
			));
			return JsonService.DeserializeSafe<LeaderboardResponse>(response.Data);
		}

		public static async
			UniTask<(FinishRoundResponse FinishRoundResponse, Exception Exception)>
			RequestFinishRound(
				string sessionId, FinishRoundRequest requestData,
				bool useEncryption = false)
		{
			WebResponse response =
				await SendDefaultPostRequestSafe(Constants.Path.FinishRoundUrl,
					sessionId, requestData, useEncryption);

			return JsonService.DeserializeSafe<FinishRoundResponse>(response.Data);
		}
		
		public static async
			UniTask<(StartRoundResponse StartRoundResponse, Exception Exception)>
			RequestStartRound(
				string sessionId, bool useEncryption = false)
		{
			WebResponse response =
				await SendDefaultPostRequestSafe(Constants.Path.StartRoundUrl,
					sessionId, useEncryption: useEncryption);

			return JsonService.DeserializeSafe<StartRoundResponse>(response.Data);
		}

		private static async UniTask<WebResponse> SendDefaultPostRequestSafe(
			string url, string sessionId,
			object requestData = null, bool useEncryption = false)
		{
			string content = string.Intern("{}");

			if (requestData != null)
			{
				content = JsonService.Serialize(requestData);
			}

			if (useEncryption)
			{
				content = EncryptContent(content);
			}

			using UnityWebRequest request = CreatePostRequest(
				url,
				content,
				Constants.Header.DefaultHeaders(sessionId)
			);

			return await SendRequestSafe(request);
		}

		static KeyValuePair<string, object> UrlParam(string paramName, object value)
		{
			return KeyValuePair.Create(paramName, value);
		}

		static UnityWebRequest CreateGetRequest(
			string url, params KeyValuePair<string, object>[] urlParams)
		{
			var builder = new UriBuilder(url);
			builder.Query =
				string.Join("&", urlParams.Select(s => $"{s.Key}={s.Value}"));
			var request = UnityWebRequest.Get(builder.Uri);
			// TODO: provide headers
			return request;
		}

		private static UnityWebRequest CreatePostRequest(
			string url, string content,
			params KeyValuePair<string, string>[] headers)
		{
			UnityWebRequest request = UnityWebRequest.Put(url, content);

			request.method = Constants.Method.Post;
			request.uploadHandler.contentType =
				Constants.Header.CreateContentTypeHeaderValue();

			foreach (KeyValuePair<string, string> header in headers)
			{
				request.SetRequestHeader(header.Key, header.Value);
			}

			return request;
		}

		public static async UniTask<Sprite> DownloadImage(string url)
		{
			UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
			WebResponse response = await SendRequestSafe(request);

			if (!response.IsSuccess())
			{
				Debug.LogError(
					$"Failed to get image '{url}': request was not successful");
				return null;
			}

			Texture2D texture = DownloadHandlerTexture.GetContent(request);
			return Sprite.Create(texture,
				new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}

		private static async UniTask<WebResponse> SendRequestSafe(
			UnityWebRequest request)
		{
			try
			{
				await request.SendWebRequest();
			}
			catch (UnityWebRequestException webRequestException)
			{
				Services.ExceptionHandler.HandleException(
					$"resp_code {request.responseCode}:  {request.downloadHandler?.text}");

				return new WebResponse
				{
					Code = webRequestException.ResponseCode,
					Result = webRequestException.Result,
					Data = webRequestException.Text
				};
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}

#if UNITY_EDITOR
			Debug.Log(
				$"resp_code {request.responseCode}:  {request.downloadHandler?.text}");
#endif
			return new WebResponse
			{
				Code = request.responseCode,
				Result = request.result,
				Data = request.downloadHandler?.text
			};
		}

		private static string EncryptContent(string content)
		{
			Cryptography.AES256 aes256 = new Cryptography.AES256();
			return JsonService.Serialize(new
			{
				data = aes256.Encrypt(content, Constants.Encryption.PassPhrase)
			});
		}
	}

	public static class Signing
	{
		const string signKey = "SECRETKEY";
		readonly static Encoding enc = Encoding.UTF8;

		public static string SHA256Sign(string inputData)
		{
			return SHA256Sign(inputData, signKey);
		}

		public static string SHA256Sign(string inputData, string signKey)
		{
			using var hasher = new HMACSHA256(enc.GetBytes(signKey));
			//
			// using var hasher = SHA256.Create();
			var hash = hasher.ComputeHash(enc.GetBytes(inputData));
			var sb = new StringBuilder(hash.Length * 2);
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("x2"));
			}
			return sb.ToString();
		}
	}
}