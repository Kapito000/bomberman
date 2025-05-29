using Gameplay.Audio;

namespace MetaSystem.GameSettings.Audio
{
	public interface IAudioSetting
	{
		void SetVolume(VolumeType volumeType, float value);
		bool TryGetVolumeValue(VolumeType volumeType, out float value);
	}
}