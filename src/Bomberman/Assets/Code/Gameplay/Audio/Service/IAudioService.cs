﻿using Gameplay.Audio.Player;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Audio.Service
{
	public interface IAudioService : IService
	{
		IAudioPlayer Player { get; }
		void AssignMusicClip(AmbientMusic key, AudioSource audioSource);
		void AssignUiSfxClip(UiSfx key, AudioSource audioSource);
		void AssignMixerGroup(MixerGroup key, AudioSource audioSource);
		void EstablishCommonSettings(AudioSource audioSource);
		AudioSource ReplaceAudioSource(GameObject instance);
		bool TryCreateAdditionalAudioSource(int forEntity, out AudioSource audioSource,
			string name = nameof(AudioSource));
	}
}