using MetaSystem.GameSettings.Audio;
using Zenject;

namespace MetaSystem.GameSettings
{
	public sealed class GameSettingsService : IGameSettings
	{
		[Inject] public IAudioSetting Audio { get; }
	}
}