using Common.Component;
using Gameplay.Audio.Service;
using Gameplay.Feature.Bomb;
using Gameplay.Feature.Bomb.Behaviour;
using Gameplay.Feature.Bomb.Component;
using Gameplay.Feature.Hero.Component;
using Gameplay.Feature.Hero.StaticData;
using Gameplay.Feature.Input.Component;
using Infrastructure.ECS.Wrapper;
using Infrastructure.ECS.Wrapper.Factory;
using Infrastructure.Factory.Kit;
using UnityEngine;
using Zenject;
using Vector2 = UnityEngine.Vector2;

namespace Gameplay.Feature.Hero.Factory
{
	public sealed class HeroFactory : IHeroFactory
	{
		const int c_bombStackSizeAtStart = 1;

		[Inject] IHeroData _heroData;
		[Inject] IFactoryKit _kit;
		[Inject] IAudioService _audioService;
		[Inject] IEntityWrapperFactory _entityFactory;

		public int CreateHero(Vector2 pos, Quaternion rot, Transform parent)
		{
			var prefab = _kit.AssetProvider.Hero();
			var instance = _kit.InstantiateService
				.Instantiate(prefab, pos, rot, parent);
			var entity = _kit.EntityBehaviourFactory
				.InitEntityBehaviour(instance);
			var hero = _entityFactory.CreateWrapper(entity);

			AddBombCollection(instance, hero);

			hero
				.Add<HeroComponent>()
				.Add<InputReader>()
				.Add<CharacterInput>()
				.Add<MovementDirection>()
				.Add<BombCarrier>()
				.AddMoveSpeed(_heroData.MovementSpeed)
				.AddLifePoints(_heroData.LifePointsOnStart)
				.AddBombStackSize(c_bombStackSizeAtStart)
				;

			AddTakenDamageEffectComponents(hero);

			return entity;
		}

		public int CreateHeroSpawnPoint(Vector2 pos)
		{
			var prefab = _kit.AssetProvider.HeroSpawnPoint();
			var instance = _kit.InstantiateService.Instantiate(prefab, pos);
			var e = _kit.EntityBehaviourFactory.InitEntityBehaviour(instance);
			return e;
		}

		void AddBombCollection(GameObject instance, EntityWrapper hero)
		{
			if (!instance.TryGetComponent<IBombCollection>(out var bombCollection))
			{
				Debug.LogError($"Cannot to get a {nameof(bombCollection)}");
				return;
			}

			hero.AddBombCollectionComponent(bombCollection);

			bombCollection.AddBomb(BombType.Usual);
		}

		void AddTakenDamageEffectComponents(EntityWrapper heroEntity)
		{
			var name = Constant.ObjectName.c_DamageAudioEffect;
			if (_audioService.TryCreateAdditionalAudioSource(heroEntity.Enity,
				    out var takenDamageEffectAudioSource, name: name) == false)
			{
				CastCannotInitCorrectlyErrorMessage();
				return;
			}

			heroEntity
				.AddTakenDamageAudioEffectId(Constant.AudioClipId.c_HeroTakenDamage)
				.AddTakenDamageEffectAudioSource(takenDamageEffectAudioSource)
				;
		}

		static void CastCannotInitCorrectlyErrorMessage() =>
			Debug.LogError("The hero cannot be initialized correctly.");
	}
}