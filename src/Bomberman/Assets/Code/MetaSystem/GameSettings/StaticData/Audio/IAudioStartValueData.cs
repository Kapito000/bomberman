using Gameplay.Audio;
using Gameplay.StaticData;

namespace MetaSystem.GameSettings.StaticData.Audio
{
	public interface IAudioStartValueData : IStaticData
	{
		bool TryGetVolume(VolumeType volumeType, out float value);
	}
}