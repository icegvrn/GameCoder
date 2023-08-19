
    public interface IGameSettingsService
    {
            void SaveSettings(GameSettingsContainer settings);
            GameSettingsContainer LoadSettings();
            void SetSettings();
    }

