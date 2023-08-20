using UnityEngine;


/// <summary>
/// Gère l'affichage des textes durant la course du personnage : compte à rebours, message de mort ou de fragments... Il est appelé par le GameManager.
/// </summary>
public class RunUIManager : MonoBehaviour
{
    [SerializeField] GameObject HUD; // HUD du joueur avec PV, Essences (points) collectées etc, temps et username
    [SerializeField] GameObject startCountdown; // Compte à rebours avant de démarrer la course
    [SerializeField] GameObject fragmentDetected; // Message indiquant qu'un fragment a été détecté
    [SerializeField] GameObject playerDead; // Message indiquant la mort du joueur
    [SerializeField] GameObject playerWin; // Message indiquant la victoire du joueur
    [SerializeField] GameObject playerBestTime; // Message indiquant qu'un nouveau record de temps a été établi pour le joueur

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
