using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Machine a état qui donne des instructions au Game Manager en fonction de l'état actuel de la course.
/// </summary>
[RequireComponent(typeof(IRunningGameService))]
public class RunStateMachine : MonoBehaviour
{
    [Header("Session informations")]
    [SerializeField] UsingSession session;
    [SerializeField] DBRunSession dbRunSession;
    IRunningGameService runStatsService;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    // States de la course
    public enum RunLevelState { init, levelStart, runReady, userWin, userDead, levelEnd }
    private RunLevelState lastRunState;
    public RunLevelState currentRunState;


    void Start()
    {
        InitializationStateMachine();
        InitialisationRun();
    }

    void Update()
    {
        CheckGameStateTransitions();
        ExecuteCurrentStateAction();
    }

    void InitializationStateMachine()
    {
        lastRunState = RunLevelState.init;
        currentRunState = RunLevelState.init;
    }

    /// <summary>
    /// Méthode initiant un RunningGameService.
    /// </summary>
    void InitialisationRun()
    {
        runStatsService = gameObject.GetComponent<IRunningGameService>();
        runStatsService.Init();
        gameManager.Init();
        StartRunIfSessionReady();
    }
  

    /// <summary>
    /// Méthode effectuant des vérifications auprès du IRunningGameService pour les changements d'états liées à des données de la course en cours (mort ou victoire du joueur).
    /// </summary>
    private void CheckGameStateTransitions()
    {
        if (runStatsService.UserLife <= 0)
        {
            currentRunState = RunLevelState.userDead;
        }
        else if (runStatsService.UserEssences >= runStatsService.RunGoal)
        {
            currentRunState = RunLevelState.userWin;
        }
    }

    /// <summary>
    /// Méthode au coeur de la machine et choisissant les actions à effectuer selon l'état actuel de la run.
    /// </summary>
    private void ExecuteCurrentStateAction()
    {
        if (currentRunState != lastRunState)
        {
            switch (currentRunState)
            {
                case RunLevelState.levelStart:
                    StartLevel();
                    break;
                case RunLevelState.runReady:
                    StartRun();
                    break;
                case RunLevelState.userWin:
                    Win();
                    break;
                case RunLevelState.userDead:
                    Defeat();
                    break;
                case RunLevelState.levelEnd:
                    break;
            }
        }
        lastRunState = currentRunState;
    }

    /// <summary>
    /// Méthode permettant de passer la run à la state suivante depuis l'extérieur.
    /// </summary>
    public void NextState()
    {
        lastRunState = currentRunState;
  
        if ((int)currentRunState < Enum.GetValues(typeof(RunLevelState)).Length-1)
        {
            currentRunState = (RunLevelState)((int) currentRunState + 1);
        }   
    }

    /// <summary>
    /// Méthode permettant de démarrer le niveau si la session utilisateur est bien reconnue.
    /// </summary>
    private void StartRunIfSessionReady()
    {
        if (session.IsSessionReady())
        {
            currentRunState = RunLevelState.levelStart; 
        }
    }

    /// <summary>
    /// Méthode appelant le Game Manager pour démarrer le niveau.
    /// </summary>
    private void StartLevel()
    {
        gameManager.StartGame();
    }

    /// <summary>
    /// Méthode appelant le Game Manager pour démarrer la course.
    /// </summary>
    public void StartRun()
    {
        gameManager.StartRun(); 
    }

    /// <summary>
    /// Méthode appelant le Game Manager pour enclancher les effets de défaite.
    /// </summary>
    private void Defeat()
    {
        gameManager.UserDeath();
        currentRunState = RunLevelState.levelEnd;
    }

    /// <summary>
    /// Méthode déclanchée quand l'utilisateur a atteint le nombre maximum d'essences ( = points). 
    /// Appel de la méthode  de DBRunSession permettant d'enregistrer le meilleur temps du joueur si record battu 
    /// et appelant le Game Manager pour enclancher les effets de victoire.
    /// </summary>
    private void Win()
    {
        dbRunSession.WriteTimeIfBestime(runStatsService.RunTime);

        if (dbRunSession.IsNewBestime())
        {
            gameManager.WinAndBestTime();
        }
        else
        {
            gameManager.Win();
        }
    }

}
