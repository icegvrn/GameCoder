using UnityEngine;

/// <summary>
/// Service qui permet de connaître l'état d'avancé de la course actuelle en trackant les informations : points de vie du joueur, essences collectée, temps passé à courir etc.
/// </summary>
public class RunStatsService : MonoBehaviour, IRunningGameService
{
    [Header("Configuration de la course")]
    [SerializeField] private DBConstant.Time timeID;
    [SerializeField] int runGoal;
    public DBConstant.Time TimeID { get { return timeID; } }
    public int RunGoal { get { return runGoal; } set {  runGoal = value; } }

    // Informations course
    int runTime;
    public int RunTime { get { return runTime; } }

    // Informations joueur
    int userLife;
    public int UserLife { get {  return userLife; } set {  userLife = value; } }

    int maxUserLife = 4;
    public int MaxUserLife { get { return maxUserLife; } set { maxUserLife = value; } }

    int userEssences;
    public int UserEssences { get {  return userEssences; } set {  userEssences = value; } }

    // Timer pour le temps de course
    private CustomTimer timer;
    bool initialized;

   
    public RunStatsService()
    {
        // On réécrit par-dessus l'éventuel IRunnerGameService qui existerait déjà, dans le cas d'une course relancée par exemple.
        ServiceLocator.Instance.UnregisterService<IRunningGameService>();
        ServiceLocator.Instance.RegisterService<IRunningGameService>(this);
    }
    
    /// <summary>
    /// Méthode initialisant le timer pour compter le temps de course et la vie de l'utilisateur.
    /// </summary>
    public void Init()
    {
        SetNewTimer();
        ResetUserLife();
        initialized = true;
    }


    void Update()
    {
        if (initialized)
        {
           timer.Update();
           runTime = timer.GetValue();
        }
    }

    public void SetNewTimer()
    {
        timer = new CustomTimer();
    }

    public void StartTimer()
    {
        timer.Start();
    }

    public void StopTimer()
    {
        timer.Stop();
    }

    void ResetUserLife()
    {
        userLife = maxUserLife;
    }

    void ResetUserEssences()
    {
        userEssences = 0;
    }

    public void ResetData()
    {
        ResetUserLife();
        ResetUserEssences();
        SetNewTimer();
    }

}
