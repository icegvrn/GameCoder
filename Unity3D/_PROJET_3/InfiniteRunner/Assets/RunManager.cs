using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RunStatsService))]
public class RunManager : MonoBehaviour
{
    public enum RunLevelState { none, levelInit, levelStart,  runReady, run, userWin, userDead, levelEnd }
    private RunLevelState lastRunState;
    public RunLevelState currentRunState;


    [Header("Session informations")]
    [SerializeField] UsingSession session;
    [SerializeField] DBRunSession dbRunSession;
    RunStatsService runStatsService;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        lastRunState = RunLevelState.none;
        currentRunState = RunLevelState.levelInit;
        session.Init();
        runStatsService = gameObject.GetComponent<RunStatsService>();
        runStatsService.Init();
        StartRunIfSessionReady();
    }

    // Update is called once per frame
    void Update()
    {
        if (runStatsService.UserLife <= 0)
        {
            currentRunState = RunLevelState.userDead;
        }

        if (runStatsService.UserEssences >= runStatsService.RunGoal)
        {
            currentRunState = RunLevelState.userWin;
        }
     

        if (currentRunState != lastRunState)
        {
            switch (currentRunState)
            {
                case RunLevelState.levelInit:
                   // OnLevelInit.Invoke();
                    break;
                case RunLevelState.levelStart:
                    StartLevel();
                break;
                case RunLevelState.levelEnd:
                   // OnLevelEnd.Invoke();
                    break;
                case RunLevelState.runReady:
                    StartRun();
                    break;
                case RunLevelState.run:
                    // OnRun.Invoke();
                    break;
                case RunLevelState.userWin:
                    Win();
                    break;
                case RunLevelState.userDead:
                    Defeat();
                    break;
            }
        }
     

        lastRunState = currentRunState;
    }

    public void NextState()
    {
        lastRunState = currentRunState;
        Debug.Log("Je passe dans le next step et je regarde si " + (int)currentRunState  + " est inférieur à " +(Enum.GetValues(typeof(RunLevelState)).Length - 1) + "");
        if ((int)currentRunState < Enum.GetValues(typeof(RunLevelState)).Length-1)
        {
            Debug.Log("C'est bon donc je mets le current State sur " + ((int)currentRunState + 1));
            currentRunState = (RunLevelState)((int) currentRunState + 1);
        }   
    }

    private void StartRunIfSessionReady()
    {
        if (session.SessionReady)
        {
            currentRunState = RunLevelState.levelStart; 
        }
    }

    public void StartRun()
    {
        gameManager.StartRun();
    }

    private void StartLevel()
    {
        gameManager.StartGame();
    }

    private void Defeat()
    {
        gameManager.UserDeath();
        currentRunState = RunLevelState.run;
    }

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
