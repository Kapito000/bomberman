﻿using Gameplay.Audio.Library;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Audio.ClipProvider
{
	public interface IAudioClipProvider : IService
	{
		SfxLibrary Sfx { get; }
		UiSfxLibrary UiSfx { get; }
		MusicLibrary Musics { get; }
		ShortMusicLibrary ShortMusic { get; }
	}
}