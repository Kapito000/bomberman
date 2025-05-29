#if UNITY_EDITOR
using Gameplay.Feature.Dev.Component;
using Infrastructure.ECS.Wrapper;
using Infrastructure.ECS.Wrapper.Factory;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Gameplay.Feature.Dev.Behaviour
{
	public class CreateNewEntity : MonoBehaviour
	{
		[Inject] IEntityWrapperFactory _factory;

		public EntityWrapper CreateDevEntity() =>
			_factory.CreateWrapper()
				.NewEntity()
				.Add<DevMarker>();
	}
}
#endif