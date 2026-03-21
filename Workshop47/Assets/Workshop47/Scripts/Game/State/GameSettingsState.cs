using R3;

namespace Workshop47.Scripts.Game.State
{
    public class GameSettingsState
    {
        public ReactiveProperty<int> MusicVolume { get; }
        public ReactiveProperty<int> SFXVolume { get; }

        public GameSettingsState(GameSettingsStateData gameSettingsStateData)
        {
            MusicVolume = new ReactiveProperty<int>(gameSettingsStateData.MusicVolume);
            SFXVolume = new ReactiveProperty<int>(gameSettingsStateData.SFXVolume);
            
            MusicVolume.Skip(1).Subscribe(value => gameSettingsStateData.MusicVolume = value);
            SFXVolume.Skip(1).Subscribe(value => gameSettingsStateData.SFXVolume = value);
        }
    }
}