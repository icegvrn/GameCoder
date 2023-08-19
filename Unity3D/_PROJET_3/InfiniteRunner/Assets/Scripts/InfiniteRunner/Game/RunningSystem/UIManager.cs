using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject startCountdown;
    [SerializeField] GameObject fragmentDetected;
    [SerializeField] GameObject playerDead;
    [SerializeField] GameObject playerWin;
    [SerializeField] GameObject playerBestTime;

    public void SetHUDVisible(bool boolean)
    {
        HUD.SetActive(boolean);
    }

    public void SetStartCountdownVisible(bool boolean)
    {
        startCountdown.SetActive(boolean);
    }

    public void SetFragmentDetectedVisible(bool boolean)
    {
        fragmentDetected.SetActive(boolean);
    }

    public void SetPlayerDeadVisible(bool boolean)
    {
        playerDead.SetActive(boolean);
    }

    public void SetPlayerWinVisible(bool boolean)
    {
        playerWin.SetActive(boolean);
    }

    public void SetPlayerBestTimeVisible(bool boolean)
    {
        playerBestTime.SetActive(boolean);
    }

    public void DisableAll()
    {
        HUD.SetActive(false);
        startCountdown.SetActive(false);
        playerDead.SetActive(false);
        fragmentDetected.SetActive(false);
    }
}
