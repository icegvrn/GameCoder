using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Machine a �tat qui donne des instructions au Game Manager en fonction de l'�tat actuel de la course.
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
    /// M�thode initiant un RunningGameService.
    /// </summary>
    void InitialisationRun()
    {
        runStatsService = gameObject.GetComponent<IRunningGameService>();
        runStatsService.Init();
        gameManager.Init();
        StartRunIfSessionReady();
    }
  

    /// <summary>
    /// M�thode effectuant des v�rifications aupr�s du IRunningGameService pour les changements d'�tats li�es � des donn�es de la course en cours (mort ou victoire du joueur).
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
    /// M�thode au coeur de la machine et choisissant les actions � effectuer selon l'�tat actuel de la run.
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
    /// M�thode permettant de passer la run � la state suivante depuis l'ext�rieur.
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
    /// M�thode permettant de d�marrer le niveau si la session utilisateur est bien reconnue.
    /// </summary>
    private void StartRunIfSessionReady()
    {
        if (session.IsSessionReady())
        {
            currentRunState = RunLevelState.levelStart; 
        }
    }

    /// <summary>
    /// M�thode appelant le Game Manager pour d�marrer le niveau.
    /// </summary>
    private void StartLevel()
    {
        gameManager.StartGame();
    }

    /// <summary>
    /// M�thode appelant le Game Manager pour d�marrer la course.
    /// </summary>
    public void StartRun()
    {
        gameManager.StartRun(); 
    }

    /// <summary>
    /// M�thode appelant le Game Manager pour enclancher les effets de d�faite.
    /// </summary>
    private void Defeat()
    {
        gameManager.UserDeath();
        currentRunState = RunLevelState.levelEnd;
    }

    /// <summary>
    /// M�thode d�clanch�e quand l'utilisateur a atteint le nombre maximum d'essences ( = points). 
    /// Appel de la m�thode  de DBRunSession permettant d'enregistrer le meilleur temps du joueur si record battu 
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
