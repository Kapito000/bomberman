using Zenject;

namespace Infrastructure.Installer
{
	public static class Util
	{
		public static void ResolveDiContainerDependencies(DiContainer container)
		{
			foreach (var dependence in container.Resolve<IDiContainerDependence[]>())
				dependence.SetContainer(container);
		}
	}
}