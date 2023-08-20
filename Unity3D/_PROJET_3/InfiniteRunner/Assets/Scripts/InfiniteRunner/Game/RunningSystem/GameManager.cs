using UnityEngine;

/// <summary>
/// G�re la communication avec les diff�rents composants en fonction des �tats de la course (donn� par le RunStateMachine)
/// </summary>
public class GameManager : MonoBehaviour
{
    // Elements avec lesquels le Game Manager va communiquer selon les phases du jeu
    [SerializeField] RunUIManager UIManager;
    [SerializeField] MapGenerator gameEnvironment;
    [SerializeField] FragmentManager fragmentManager;
    [SerializeField] CharacterAutoRunner character;
    [SerializeField] Camera mainCamera;
  
    // Propri�t�s
    bool initialized; 


    /// <summary>
    /// Initialise les composants � leur �tat initial : tout est d�sactiv� par d�faut, car r�activ� selon la RunStateMachine.
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
    /// L'Update v�rifie � tout instant si un fragment est apparu pour afficher un message le cas �ch�ant.
    /// </summary>
    private void Update()
    {
        if (initialized)
        {
          DisplayMessageIfFragmentDetected();
        }
    }

    /// <summary>
    /// M�thode appelant l'UIManager pour lui faire afficher un message quand un fragment est d�tect�.
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
    /// M�thode d�marrant le jeu en activant tous les composants sauf le controlleur automatique du personnage joueur.
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
    /// M�thode qui lance la course : active le controlleur automatique du personnage joueur et d�marre le timer.
    /// </summary>
    public void StartRun()
    {
        UIManager.SetStartCountdownVisible(false);
        ServiceLocator.Instance.GetService<IRunningGameService>().StartTimer();
        character.enabled = true;
    }

/// <summary>
/// M�thode appel�e lorsque le jeu s'arr�te pour d�sactiver le controlleur automatique du joueur et le HUD. Reset des donn�es de course.
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
    /// M�thode appel�e lorsque le jeu s'arr�te : d�sactive le controlleur joueur, le HUD et l'environnement.
    /// </summary>
    public void StopGameAndEnvironment()
    {
        StopGame();
        gameEnvironment.gameObject.SetActive(false);

    }

    /// <summary>
    /// M�thode appelant l'UIManager pour passer l'HUD en visible.
    /// </summary>
    public void EnableHUD()
    {
        UIManager.SetHUDVisible(true);
    }

    /// <summary>
    /// M�thode appelant l'UIManager pour passer l'HUD en invisible.
    /// </summary>
    public void DisableHUD()
    {
        UIManager.SetHUDVisible(false);
    }

    /// <summary>
    /// M�thode appelant la m�thode StopGame qui masque les �l�ments qui ne doivent pas �tre affich�s hors jeu  et appel l'UIManager pour afficher le message de mort.
    /// </summary>
    public void UserDeath()
    {
        StopGame();
        UIManager.SetPlayerDeadVisible(true);
    }

    /// <summary>
    /// M�thode appelant la m�thode StopGame qui masque les �l�ments qui ne doivent pas �tre affich�s hors jeu  et appel l'UIManager pour afficher le message de victoire.
    /// </summary>
    public void Win()
    {
        StopGame();
        UIManager.SetPlayerWinVisible(true);
    }

    /// <summary>
    /// M�thode appelant la m�thode StopGame qui masque les �l�ments qui ne doivent pas �tre affich�s hors jeu  et appel l'UIManager pour afficher le message de victoire et de meilleur temps.
    /// </summary>
    public void WinAndBestTime()
    {
        StopGame();
        UIManager.SetPlayerBestTimeVisible(true);
        UIManager.SetPlayerWinVisible(true);
    }

}
