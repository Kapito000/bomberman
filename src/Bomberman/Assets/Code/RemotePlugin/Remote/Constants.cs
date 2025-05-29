using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RemotePlugin.Remote {
    public static class Constants {
        public const string GameId = "snk";
        
        private static readonly string ConfigFilePath =
            System.IO.Path.Combine(Application.persistentDataPath, "api.conf");
        private const string FallbackGameApiUrl = "https://service-5.dcsdev.ru/ton-cats/api/";
        private static readonly string GameApiUrl;

        static Constants() {
            if (File.Exists(ConfigFilePath)) {
                string[] lines = File.ReadAllLines(ConfigFilePath);
                
                if (lines.Length > 0) {
                    GameApiUrl = lines[0];
                }
            }
            else {
                GameApiUrl = FallbackGameApiUrl;
            }
        }
        
        public static class Method {
            public const string Get = "GET";
            public const string Post = "POST";
        }

        public static class Path {
            public static string AuthUrl => CombineUrl(GameApiUrl, "v1/auth");
            public static string PurchaseCountableUrl => CombineUrl(GameApiUrl, "v1/session/item/exchange");
            public static string PurchaseUniqueUrl => CombineUrl(GameApiUrl, "v1/session/item/obtain-unique");
            public static string PurchaseBundleUrl => CombineUrl(GameApiUrl, "v1/session/item/obtain-bundle");
            public static string EquipUrl => CombineUrl(GameApiUrl, "v1/session/item/equip");
            public static string StartRoundUrl => CombineUrl(GameApiUrl, "v1/session/run/start");
            public static string FinishRoundUrl => CombineUrl(GameApiUrl, "v1/session/run/finish");
            public static string GetLeaderboardUrl => CombineUrl(GameApiUrl, $"v1/leaderboard/{GameId}");
            public static string GetLeaderboardUserUrl(long telegramID) => CombineUrl(GameApiUrl, $"v1/leaderboard/{GameId}/{telegramID}");

            private static string CombineUrl(string url, string path) {
                url = url.TrimEnd('/');
                path = path.TrimStart('/');
                return $"{url}/{path}";
            }
        }

        public static class Header {
            public const string SessionId = "x-session-id";
            public const string Time = "x-request-start";
            public const string Signature = "x-signature";
            public const string Version = "x-app-version";

            private static class ValueName {
                public const string Auth = "Bearer";
            }

            public static string CreateTimeHeaderValue(DateTime time) {
                return time.ToString("yyyy-dd-MM hh:mm:ss");
            }

            public static string CreateSessionHeaderValue(string sessionId) {
                return sessionId;
            }

            private static string CreateSignatureHeaderValue(string signatureKey) {
                return signatureKey;
            }

            private static string CreateVersionHeaderValue() {
                return Application.version;
            }

            public static string CreateContentTypeHeaderValue() { 
                return "application/json";
            }

            public static KeyValuePair<string, string>[] AuthHeaders() {
                return new[] {
                    new KeyValuePair<string, string>(Time, CreateTimeHeaderValue(DateTime.UtcNow)),
                    // TODO: Change arg to actual signature key
                    new KeyValuePair<string, string>(Signature, CreateSignatureHeaderValue(string.Empty)),
                    new KeyValuePair<string, string>(Version, CreateVersionHeaderValue()),
                };
            }

            public static KeyValuePair<string, string>[] DefaultHeaders(string sessionId) {
                return new[] {
                    new KeyValuePair<string, string>(SessionId, CreateSessionHeaderValue(sessionId)),
                    new KeyValuePair<string, string>(Time, CreateTimeHeaderValue(DateTime.UtcNow)),
                    // TODO: Change arg to actual signature key
                    new KeyValuePair<string, string>(Signature, CreateSignatureHeaderValue(string.Empty)),
                    new KeyValuePair<string, string>(Version, CreateVersionHeaderValue()),
                };
            }
        }

        public static class Encryption {
            public const string PassPhrase = "key";
        }
    }
}
