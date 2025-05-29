using Zenject;

namespace Infrastructure.ECS.Wrapper.Factory
{
	public sealed class EntityWrapperFactory : IEntityWrapperFactory
	{
		[Inject] EntityWrapper _template;
		
		public EntityWrapper CreateWrapper() => 
			_template;

		public EntityWrapper CreateWrapper(int entity)
		{
			_template.SetEntity(entity);
			return _template;
		}
	}
}