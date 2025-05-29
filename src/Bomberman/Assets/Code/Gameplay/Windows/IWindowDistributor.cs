namespace Gameplay.Windows
{
	public interface IWindowDistributor
	{
		bool TryGetWindow<TWindow>(WindowId id, out TWindow window)
			where TWindow : class, IWindow;
	}
}