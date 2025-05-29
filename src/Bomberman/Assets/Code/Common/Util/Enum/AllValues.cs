using System.Linq;

namespace Common.Util.Enum
{
	public static class AllValues<T> where T : System.Enum
	{
		public static T[] Values { get; private set; }

		static AllValues()
		{
			Values = System.Enum.GetValues(typeof(T)).Cast<T>().ToArray();
		}
	}
}