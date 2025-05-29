using Infrastructure;
using MetaSystem.GameSettings.Audio;

namespace MetaSystem.GameSettings
{
	public interface IGameSettings : IService
	{
		IAudioSetting Audio { get; }
	}
}