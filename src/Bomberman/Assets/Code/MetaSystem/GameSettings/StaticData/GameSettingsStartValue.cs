using MetaSystem.GameSettings.StaticData.Audio;

namespace MetaSystem.GameSettings.StaticData
{
	public sealed class GameSettingsStartValue : IGameSettingsStartValueData
	{
		public IAudioStartValueData Audio { get; } = new AudioStartValueData();
	}
}