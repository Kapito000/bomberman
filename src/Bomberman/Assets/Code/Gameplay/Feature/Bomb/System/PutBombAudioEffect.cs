using Common.Component;
using Gameplay.Audio.Service;
using Gameplay.Feature.Bomb.Component;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.Bomb.System
{
	public sealed class PutBombAudioEffect : IEcsRunSystem
	{
		[Inject] EntityWrapper _reqest;
		[Inject] IAudioService _audioService;

		readonly EcsFilterInject<
				Inc<BombComponent, FirstBreath, TransformComponent>>
			_requestFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var e in _requestFilter.Value)
			{
				_reqest.SetEntity(e);
				var pos = _reqest.TransformPos();
				var clipId = Constant.AudioClipId.c_PutBomb;
				_audioService.Player.PlaySfxClipAtPoint(clipId, pos);
			}
		}
	}
}