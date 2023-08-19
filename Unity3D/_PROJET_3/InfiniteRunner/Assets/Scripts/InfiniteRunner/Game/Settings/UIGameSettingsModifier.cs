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
        gameSettings = ServiceLocator.Instance.GetService<IGameSettingsService>();
        GameSettingsContainer gs = gameSettings.LoadSettings();

        gameSettingsContainer = gs;


        if (gs.isAzerty)
        {
            controlDropDown.value = 0;
        }
        
        else
        {
            controlDropDown.value = 1;
        }

        musicVolume.value = gs.musicVolume;
        effectVolume.value = gs.soundVolume;
        SetControlToAzerty(gs.isAzerty);
        SetQualityLevel(gs.qualityLevel);
    }

    private void SetControlToAzerty(bool b)
    {
        if (b)
        {
            controlDropDown.value = 0;
        }

        else
        {
            controlDropDown.value = 1;
        }
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
