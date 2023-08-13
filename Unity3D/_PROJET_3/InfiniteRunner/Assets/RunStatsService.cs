using UnityEngine;


public class RunStatsService : MonoBehaviour
{

    [SerializeField] private DBConstant.Time timeID;
    public DBConstant.Time TimeID { get { return timeID; } }

    [SerializeField] int runGoal;
    public int RunGoal { get { return runGoal; } set {  runGoal = value; } }
    int userLife;
    public int UserLife { get {  return userLife; } set {  userLife = value; } }

    int maxUserLife = 4;
    public int MaxUserLife { get { return maxUserLife; } set { maxUserLife = value; } }

    int userEssences;
    public int UserEssences { get {  return userEssences; } set {  userEssences = value; } }

    int runTime;
    public int RunTime { get { return runTime; } }

    private CustomTimer timer;

    bool initialized;
    

    public void Init()
    {
        RegisterMe();
        SetTimer();
        ResetUserLife();
        initialized = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
           timer.Update();
           runTime = timer.GetValue();
        }


    }

    public void RegisterMe()
    {
        if (ServiceLocator.Instance.GetService<RunStatsService>() == null)
        {
            ServiceLocator.Instance.RegisterService(this);
            Debug.Log("Service enregistré");
        }
      
    }

    public void UnregisterMe()
    {
        ServiceLocator.Instance.UnregisterService<RunStatsService>();
    }

    void SetTimer()
    {
        timer = new CustomTimer();
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
        SetTimer();
    }

    public void StartTimer()
    {
        timer.Start();
    }

    public void StopTimer()
    {
        timer.Stop();
    }

}
