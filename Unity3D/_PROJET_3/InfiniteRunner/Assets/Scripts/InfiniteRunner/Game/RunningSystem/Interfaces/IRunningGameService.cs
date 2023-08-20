
    public interface IRunningGameService
    {
    DBConstant.Time TimeID { get; }
    int RunGoal { get; set; }
    int UserLife { get; set; }
    int MaxUserLife { get; set; }
    int UserEssences { get; set; }
    int RunTime { get; }

    void Init();
    void SetNewTimer();
    void StartTimer();
    void StopTimer();
    void ResetData();
    }

