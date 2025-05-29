using RemotePlugin.UserDataService;

namespace RemotePlugin.Remote
{
	public static class Services
	{
		public static IExceptionHandler ExceptionHandler { get; private set; }
		public static IUserDataService UserDataService { get; private set; }

		public static void Init(IExceptionHandler exceptionHandler,
			IUserDataService userDataService)
		{
			ExceptionHandler = exceptionHandler;
			UserDataService = userDataService;
		}
	}
}