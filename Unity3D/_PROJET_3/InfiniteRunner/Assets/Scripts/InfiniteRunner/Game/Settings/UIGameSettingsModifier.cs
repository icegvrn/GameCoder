using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameSettingsModifier : MonoBehaviour
{
    private IGameSettingsService gameSettings;
    private GameSettingsContainer gameSettingsContainer;

    [SerializeField] List<Toggle> qualityToggles;
    [SerializeField] TMP_Dropdown controlDropDown;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider effectVolume;

    private void OnEnable()
    {
        GameSettingsContainer gs = Initialize();
        UpdateUIValues(gs);

       
    }

    private GameSettingsContainer Initialize()
    {
        gameSettings = ServiceLocator.Instance.GetService<IGameSettingsService>();
        GameSettingsContainer gs = gameSettings.LoadSettings();
        return gs;
    }

    private void UpdateUIValues(GameSettingsContainer gs)
    {
        gameSettingsContainer = gs;
        musicVolume.value = gs.musicVolume;
        effectVolume.value = gs.soundVolume;
        SetControlToAzerty(gs.isAzerty);
        SetQualityLevel(gs.qualityLevel);
    }

    private void SetControlToAzerty(bool b)
    {
        controlDropDown.value = b ? 0 : 1;
    }

    private void SetQualityLevel(int quality)
    {
        for (int i = 0; i < qualityToggles.Count; i++)
        {
            qualityToggles[i].isOn = i == quality;
        }

    }

    private int GetQualityLevel(List<Toggle> qualityToggles)
    {
        for (int i = 0; i < qualityToggles.Count; i++)
        {
            if (qualityToggles[i].isOn)
            {
                return i;
            }
        }
        return 0;
    }

    public void SaveSettings()
    {
        gameSettingsContainer.qualityLevel = GetQualityLevel(qualityToggles);
        gameSettingsContainer.isAzerty = controlDropDown.value == 0;
        gameSettingsContainer.musicVolume = musicVolume.value;
        gameSettingsContainer.soundVolume = effectVolume.value;
        gameSettings.SaveSettings(gameSettingsContainer);
    }
    
}
