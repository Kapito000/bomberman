namespace Infrastructure.ECS.Wrapper.Factory
{
	public interface IEntityWrapperFactory
	{
		EntityWrapper CreateWrapper();
		EntityWrapper CreateWrapper(int entity);
	}
}