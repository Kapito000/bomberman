using Gameplay.Audio;

namespace MetaSystem.GameSettings.StaticData.Audio
{
	public sealed class AudioStartValueData : IAudioStartValueData
	{
		public bool TryGetVolume(VolumeType volumeType, out float value)
		{
			value = 1;
			return true;
		}
	}
}