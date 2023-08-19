using System.IO;
using UnityEngine;

public class GameSettingsService : IGameSettingsService
{
    private string settingsFilePath;
    private ISoundService soundLevelService;
    private IInputService inputService;

    public GameSettingsService(IInputService input, ISoundService sound)
    {
        soundLevelService = sound;
        inputService = input;
    }


    public void SaveSettings(GameSettingsContainer settings)
    {
        settingsFilePath = Application.persistentDataPath + "/settings.json";
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(settingsFilePath, json);
        SetSettings();
    }

    public GameSettingsContainer LoadSettings()
    {
         settingsFilePath = Application.persistentDataPath + "/settings.json";

        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            return JsonUtility.FromJson<GameSettingsContainer>(json);
        }
        else
        {
            return new GameSettingsContainer();
        }
    }

    public void SetSettings()
    {
        GameSettingsContainer gc = LoadSettings();
        SetQuality(gc);
        soundLevelService.MusicLevel = gc.musicVolume;
        soundLevelService.VFXLevel = gc.soundVolume;
        inputService.SetAzertyModeTo(gc.isAzerty);
    }   

    private void SetQuality(GameSettingsContainer gc)
    {
        if (gc.qualityLevel == 0)
        {
            QualitySettings.SetQualityLevel(0, true);
        }

        else if (gc.qualityLevel == 1)
        {
            QualitySettings.SetQualityLevel(3, true);
        }

        else if (gc.qualityLevel == 2)
        {
            QualitySettings.SetQualityLevel(5, true);
        }

    }
}
