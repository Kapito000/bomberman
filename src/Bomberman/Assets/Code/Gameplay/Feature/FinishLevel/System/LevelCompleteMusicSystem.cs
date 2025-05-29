﻿using Common.Component;
using Gameplay.Audio;
using Gameplay.Audio.Service;
using Gameplay.Feature.FinishLevel.Component;
using Infrastructure.ECS;
using Infrastructure.ECS.Wrapper;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Zenject;

namespace Gameplay.Feature.FinishLevel.System
{
	public sealed class LevelCompleteMusicSystem : IEcsRunSystem
	{
		[Inject] EntityWrapper _music;
		[Inject] IAudioService _audioService;

		readonly EcsFilterInject<
				Inc<FinishLevelMusic, AudioSourceComponent>,
				Exc<Launched>>
			_musicFilter;
		readonly EcsFilterInject<
			Inc<FinishLevelObserver, LevelFinished, LevelComplete>> _observerFilter;

		public void Run(IEcsSystems systems)
		{
			foreach (var music in _musicFilter.Value)
			foreach (var observer in _observerFilter.Value)
			{
				_music.SetEntity(music);
				var audioSource = _music.AudioSource();
				_audioService.Player.Play(ShortMusic.Victory, audioSource);
				_music.Add<Launched>();
			}
		}
	}
}