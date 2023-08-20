using UnityEngine;

/// <summary>
/// Gère la communication avec les différents composants en fonction des états de la course (donné par le RunStateMachine)
/// </summary>
public class GameManager : MonoBehaviour
{
    // Elements avec lesquels le Game Manager va communiquer selon les phases du jeu
    [SerializeField] RunUIManager UIManager;
    [SerializeField] MapGenerator gameEnvironment;
    [SerializeField] FragmentManager fragmentManager;
    [SerializeField] CharacterAutoRunner character;
    [SerializeField] Camera mainCamera;
  
    // Propriétés
    bool initialized; 


    /// <summary>
    /// Initialise les composants à leur état initial : tout est désactivé par défaut, car réactivé selon la RunStateMachine.
    /// </summary>
    public void Init()
    {
        gameEnvironment.gameObject.SetActive(false);
        character.enabled = false;
        character.gameObject.SetActive(false);
        UIManager.DisableAll();
        initialized = true;
    }

    /// <summary>
    /// L'Update vérifie à tout instant si un fragment est apparu pour afficher un message le cas échéant.
    /// </summary>
    private void Update()
    {
        if (initialized)
        {
          DisplayMessageIfFragmentDetected();
        }
    }

    /// <summary>
    /// Méthode appelant l'UIManager pour lui faire afficher un message quand un fragment est détecté.
    /// </summary>
    private void DisplayMessageIfFragmentDetected()
    {
        if (fragmentManager.FragmentDetected)
        {
            UIManager.SetFragmentDetectedVisible(true);
            fragmentManager.FragmentDetected = false;
        }
    }

    /// <summary>
    /// Méthode démarrant le jeu en activant tous les composants sauf le controlleur automatique du personnage joueur.
    /// </summary>
    public void StartGame()
    {
        gameEnvironment.gameObject.SetActive(true);
        character.gameObject.SetActive(true);
        UIManager.SetHUDVisible(true);
        mainCamera.transform.parent = character.gameObject.transform;
        UIManager.SetStartCountdownVisible(true);
        character.enabled = false;
    }

    /// <summary>
    /// Méthode qui lance la course : active le controlleur automatique du personnage joueur et démarre le timer.
    /// </summary>
    public void StartRun()
    {
        UIManager.SetStartCountdownVisible(false);
        ServiceLocator.Instance.GetService<IRunningGameService>().StartTimer();
        character.enabled = true;
    }

/// <summary>
/// Méthode appelée lorsque le jeu s'arrête pour désactiver le controlleur automatique du joueur et le HUD. Reset des données de course.
/// </summary>
    public void StopGame()
    {
        character.enabled = false;
        character.gameObject.SetActive(false);
        UIManager.SetHUDVisible(false);
        mainCamera.transform.parent = transform;
        ServiceLocator.Instance.GetService<IRunningGameService>().ResetData();
    }


    /// <summary>
    /// Méthode appelée lorsque le jeu s'arrête : désactive le controlleur joueur, le HUD et l'environnement.
    /// </summary>
    public void StopGameAndEnvironment()
    {
        StopGame();
        gameEnvironment.gameObject.SetActive(false);

    }

    /// <summary>
    /// Méthode appelant l'UIManager pour passer l'HUD en visible.
    /// </summary>
    public void EnableHUD()
    {
        UIManager.SetHUDVisible(true);
    }

    /// <summary>
    /// Méthode appelant l'UIManager pour passer l'HUD en invisible.
    /// </summary>
    public void DisableHUD()
    {
        UIManager.SetHUDVisible(false);
    }

    /// <summary>
    /// Méthode appelant la méthode StopGame qui masque les éléments qui ne doivent pas être affichés hors jeu  et appel l'UIManager pour afficher le message de mort.
    /// </summary>
    public void UserDeath()
    {
        StopGame();
        UIManager.SetPlayerDeadVisible(true);
    }

    /// <summary>
    /// Méthode appelant la méthode StopGame qui masque les éléments qui ne doivent pas être affichés hors jeu  et appel l'UIManager pour afficher le message de victoire.
    /// </summary>
    public void Win()
    {
        StopGame();
        UIManager.SetPlayerWinVisible(true);
    }

    /// <summary>
    /// Méthode appelant la méthode StopGame qui masque les éléments qui ne doivent pas être affichés hors jeu  et appel l'UIManager pour afficher le message de victoire et de meilleur temps.
    /// </summary>
    public void WinAndBestTime()
    {
        StopGame();
        UIManager.SetPlayerBestTimeVisible(true);
        UIManager.SetPlayerWinVisible(true);
    }

}
