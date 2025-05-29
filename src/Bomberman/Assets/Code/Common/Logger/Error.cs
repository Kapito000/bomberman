using UnityEngine;

namespace Common.Logger
{
	public static class Error
	{
		public static void CannotUnpackEntity() =>
			Debug.LogError("Cannot to unpack entity");

		public static void CannotFind(string str) =>
			Debug.LogError($"Cannot find {str}");

		public static void NoImplementation() =>
			Debug.LogError("No implementation");

		public static void NullReference() =>
			Debug.LogError("Null reference");
		
		public static void Incorrect(string str) =>
			Debug.LogError($"An incorrect {str}");
	}
}