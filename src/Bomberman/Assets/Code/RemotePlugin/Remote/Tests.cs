// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Security.Cryptography;
// using System.Text;
// using Medo;
// using Remote.Data;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace Remote {
//     [Serializable]
//     public class Tests : MonoBehaviour {
//         [SerializeField] private string UserId = "test_user";
//         [SerializeField] private int PriceAmount = 1;
//         [SerializeField] private string CountableItemId = "countable_test_item";
//         [SerializeField] private string UniqueItemId = "unique_test_item";
//         [SerializeField] private int Amount = 1;
//         [SerializeField] private int Score = 10;
//         [SerializeField] private string AssignEquipmentId = "test_equipment_id";
//
//         [SerializeField] private string _hmacSessionId;
//         [SerializeField] private string _secret;
//         [SerializeField] private string _message;
//         [SerializeField] private string _uuid;
//
//         // Dynamic cache
//         private string _sessionId;
//         private Dictionary<string, int> _countableItems;
//         private List<string> _uniqueItems;
//         private int _roundId;
//
//         [Button("Auth")]
//         public async void AuthTest() {
//             // Arrange & Act
//             AuthResponse authData = await Server.RequestAuth(new AuthRequest {
//                 UserId = UserId,
//                 GameId = Constants.GameId
//             });
//
//             // Assert
//             Debug.Assert(authData != null);
//             Debug.Assert(!string.IsNullOrEmpty(authData.SessionId));
//             Debug.Assert(authData.ItemsData != null);
//
//             _sessionId = authData.SessionId;
//             _countableItems = authData.ItemsData.CountableItems.ToDictionary(
//                 item => item.Key, item => item.Value.Amount);
//             _uniqueItems = authData.ItemsData.UniqueItems;
//
//             Debug.Log("---------- Countable items ----------");
//             if (_countableItems != null) {
//                 foreach (KeyValuePair<string, int> item in _countableItems) {
//                     Debug.Log($"{item.Key} : {item.Value}");
//                 }
//             }
//             else {
//                 Debug.Log("None");
//             }
//
//             Debug.Log("----------- Unique  items -----------");
//             if (_uniqueItems != null) {
//                 foreach (string itemId in _uniqueItems) {
//                     Debug.Log($"{itemId}");
//                 }
//             }
//             else {
//                 Debug.Log("None");
//             }
//         }
//
//         [Button("Purchase countable")]
//         public async void PurchaseCountableTest() {
//             Debug.Assert(!string.IsNullOrEmpty(_sessionId));
//
//             // Arrange & Act
//             PurchaseCountableResponse purchaseCountableData = await Server.RequestCountablePurchase(_sessionId, new PurchaseCountableItemRequest {
//                 PriceId = PriceId.Cats.ToString().ToLower(),
//                 PriceAmount = PriceAmount,
//                 ItemId = CountableItemId,
//                 Amount = Amount
//             });
//
//             // Assert
//             Debug.Assert(purchaseCountableData != null);
//             Debug.Assert(purchaseCountableData.Payload != null);
//         }
//
//         [Button("Purchase unique")]
//         public async void PurchaseUniqueTest() {
//             Debug.Assert(!string.IsNullOrEmpty(_sessionId));
//
//             // Arrange & Act
//             PurchaseUniqueItemResponse purchaseUniqueItemResponse = await Server.RequestUniquePurchase(_sessionId, new PurchaseUniqueItemRequest {
//                 PriceId = PriceId.Cats.ToString().ToLower(),
//                 PriceAmount = PriceAmount,
//                 ItemId = UniqueItemId
//             });
//
//             // Assert
//             Debug.Assert(purchaseUniqueItemResponse != null);
//             Debug.Assert(purchaseUniqueItemResponse.Payload != null);
//         }
//
//         [Button("Purchase unique")]
//         public async void AssignEquipmentTest() {
//             Debug.Assert(!string.IsNullOrEmpty(_sessionId));
//
//             // Arrange
//             Dictionary<string, string> slotChanges = new Dictionary<string, string> {
//                 // TODO: Put data
//             };
//
//             // Act
//             AssignEquipmentResponse requestAssignEquipment = await Server.RequestAssignEquipment(_sessionId, new AssignEquipmentRequest {
//                 SlotChanges = slotChanges
//             });
//
//             // Assert
//             Debug.Assert(requestAssignEquipment != null);
//             Debug.Assert(requestAssignEquipment.ResultCode != EquipmentResultCode.ER_UnknownError);
//         }
//
//         [Button("Start round")]
//         public async void StartRoundTest() {
//             Debug.Assert(!string.IsNullOrEmpty(_sessionId));
//             // Arrange & Act
//             StartRoundResponse response = await Server.RequestStartRound(_sessionId);
//
//             // Assert
//             Debug.Assert(response != null);
//             _roundId = response.RunId;
//             Debug.Log($"Start round with ID: {_roundId}");
//         }
//
//         [Button("Finish round")]
//         public async void FinishRoundTest() {
//             Debug.Assert(!string.IsNullOrEmpty(_sessionId));
//
//             // Arrange
//             Dictionary<string, int> items = new Dictionary<string, int> {
//                 { CountableItemId, 1 }
//             };
//
//             // Act
//             FinishRoundResponse requestFinishRound = await Server.RequestFinishRound(_sessionId, new FinishRoundRequest {
//                 RoundId = _roundId,
//                 Score = Score,
//                 UsedItems = items
//             });
//
//             // Assert
//             Debug.Assert(requestFinishRound != null);
//         }
//
//         [Button]
//         private void CalculateHMAC() {
//             using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacSessionId + _secret));
//             byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(_message + _uuid));
//             Debug.Log(BitConverter.ToString(hashBytes).Replace("-", "").ToLower());
//         }
//
//         [Button]
//         private void CreateUUIDv7() {
//             Uuid7 uuidv7 = Uuid7.NewUuid7();
//             Debug.Log(uuidv7);
//         }
//     }
// }
