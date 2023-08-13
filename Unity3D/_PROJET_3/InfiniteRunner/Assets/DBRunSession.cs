using UnityEngine;

public class DBRunSession : MonoBehaviour
{
    [SerializeField] private DBConstant.Time timeID;
    private bool newBestTime;
  public void WriteTimeIfBestime(int time)
    {

        Debug.Log("JE vais enregistrer le temps " + time);
        DBUsers_TimeJoinTime bestTimeData = ServiceLocator.Instance.GetService<UserSessionData>().GetBestTimeFromTime((int)timeID);
       
        if (bestTimeData == null)
        {
            ServiceLocator.Instance.GetService<UserSessionData>().RegisterNewBestTimeForUserFromNull(time, (int)timeID);
            newBestTime = true;
        }
        else if (time < bestTimeData.best_time && time != 0)
        {
            ServiceLocator.Instance.GetService<UserSessionData>().RegisterNewBestTimeForUser(time, (int)timeID);
            newBestTime = true;
        } 
    }

    public bool IsNewBestime()
    {
        return newBestTime;
    }
}
