using UnityEngine;

/// <summary>
/// Classe permettant de communiquer avec la database. Utilisé pour inscrire les meilleurs temps des utilisateurs.
/// </summary>
public class DBRunSession : MonoBehaviour
{
    private IRunningGameService runningGameService;
    private SQLiteSessionDataQuery db;
    private DBConstant.Time timeID;
    private bool newBestTime;

    public void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Initialisation des services nécessaires au fonctionnement de DBRunSession.
    /// </summary>
    void Initialization()
    {
        db = ServiceLocator.Instance.GetService<ISessionService>().Query;
        runningGameService = ServiceLocator.Instance.GetService<IRunningGameService>();
    }

    /// <summary>
    /// Méthode vérifiant si le temps effectué par le joueur est son meilleur temps et de l'inscrire dans la bdd si c'est le cas.
    /// </summary>
    /// <param name="scoreTime"></param>
    public void WriteTimeIfBestime(int scoreTime)
    {
        timeID = runningGameService.TimeID;
        DBUsers_TimeJoinTime bestTimeData = db.GetCurrentUserBestScoreTimeFromTime((int)timeID);

        if (bestTimeData == null)
        {
           db.RegisterNewBestScoreTimeForUserFromNull(scoreTime, (int)timeID);
            newBestTime = true;
        }
        else if (scoreTime < bestTimeData.best_time && scoreTime != 0)
        {
            db.RegisterNewBestScoreTimeForUser(scoreTime, (int)timeID);
            newBestTime = true;
        }
    }

/// <summary>
/// Méthode retournant un bool indiquant si l'utilisateur vient de faire un meilleur temps ou non;
/// </summary>
/// <returns></returns>
    public bool IsNewBestime()
    {
        return newBestTime;
    }
}
