using UnityEngine;

public class DBRunSession : MonoBehaviour
{
    private DBConstant.Time timeID;
    private bool newBestTime;
  public void WriteTimeIfBestime(int time)
    {
        timeID = ServiceLocator.Instance.GetService<RunStatsService>().TimeID;
        DBUsers_TimeJoinTime bestTimeData = ServiceLocator.Instance.GetService<ISessionService>().Query.GetCurrentUserBestScoreTimeFromTime((int)timeID);
       
        if (bestTimeData == null)
        {
            ServiceLocator.Instance.GetService<ISessionService>().Query.RegisterNewBestScoreTimeForUserFromNull(time, (int)timeID);
            newBestTime = true;
        }
        else if (time < bestTimeData.best_time && time != 0)
        {
            ServiceLocator.Instance.GetService<ISessionService>().Query.RegisterNewBestScoreTimeForUser(time, (int)timeID);
            newBestTime = true;
        } 
    }

    public bool IsNewBestime()
    {
        return newBestTime;
    }
}
