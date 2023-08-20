using UnityEngine;


/// <summary>
/// G�re l'affichage des textes durant la course du personnage : compte � rebours, message de mort ou de fragments... Il est appel� par le GameManager.
/// </summary>
public class RunUIManager : MonoBehaviour
{
    [SerializeField] GameObject HUD; // HUD du joueur avec PV, Essences (points) collect�es etc, temps et username
    [SerializeField] GameObject startCountdown; // Compte � rebours avant de d�marrer la course
    [SerializeField] GameObject fragmentDetected; // Message indiquant qu'un fragment a �t� d�tect�
    [SerializeField] GameObject playerDead; // Message indiquant la mort du joueur
    [SerializeField] GameObject playerWin; // Message indiquant la victoire du joueur
    [SerializeField] GameObject playerBestTime; // Message indiquant qu'un nouveau record de temps a �t� �tabli pour le joueur

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
