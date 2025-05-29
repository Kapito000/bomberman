using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace MetaSystem.Telegram
{
	public class TelegramBridge
	{
		private static TelegramBridge _instance;

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern int getUserId();
        
        [DllImport("__Internal")]
        private static extern int getUserName();
        
        [DllImport("__Internal")]
        private static extern int getUserPhotoUrl();
        
        [DllImport("__Internal")]
        private static extern int getInitData();
#endif

		public static string GetUserId()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
            try {
                return BufferToString(getUserId());
            }
            catch (Exception e) {
                Debug.LogError($"Error attempt to call '{nameof(getUserId)}': " + e.Message);
            }

            return string.Empty;
#elif UNITY_EDITOR
			const string c_testUserId ="test-user-id";
			
			var testTgIdPath = System.IO.Path.Combine(Application.dataPath, 
				"Code/RemotePlugin/test_tg_id");

			if (System.IO.File.Exists(testTgIdPath) == false)
			{
				Debug.LogError("Cannot to find the file with a test telegram id.");
				return c_testUserId;
			}

			using var reader = new System.IO.StreamReader(testTgIdPath);
			var line = reader.ReadLine();
			return line;
#endif
		}

		public static string GetUserName()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
            try {
                return BufferToString(getUserName());
            }
            catch (Exception e) {
                Debug.LogError($"Error attempt to call '{nameof(getUserName)}': " + e.Message);
            }

            return string.Empty;
#endif
			return "";
		}

		public static string GetUserPhotoUrl()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
            try {
                return BufferToString(getUserPhotoUrl());
            }
            catch (Exception e) {
                Debug.LogError($"Error attempt to call '{nameof(getUserPhotoUrl)}': " + e.Message);
            }

            return string.Empty;
#endif
			return "";
		}

		public static string GetInitData()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
            try {
                return BufferToString(getInitData());
            } catch (Exception e) {
                Debug.LogException(e);
            }
#endif
			return "";
		}

		private static string BufferToString(int buffer)
		{
			try
			{
				if (buffer != 0)
				{
					return Marshal.PtrToStringUTF8((IntPtr)buffer);
				}

				Debug.LogWarning($"Failed to get string from buffer ('{buffer}')");
			}
			catch (Exception e)
			{
				Debug.LogWarning(
					$"Failed to get string from buffer ('{buffer}') : {e.Message}");
			}

			return string.Empty;
		}
	}
}