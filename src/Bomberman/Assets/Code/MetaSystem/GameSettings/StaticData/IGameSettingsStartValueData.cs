using Gameplay.StaticData;
using MetaSystem.GameSettings.StaticData.Audio;

namespace MetaSystem.GameSettings.StaticData
{
	public interface IGameSettingsStartValueData : IStaticData
	{
		IAudioStartValueData Audio { get; }
	}
}