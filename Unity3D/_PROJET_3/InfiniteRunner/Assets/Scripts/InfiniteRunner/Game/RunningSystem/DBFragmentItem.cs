using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBFragmentItem : MonoBehaviour
{
    public void AddNewFragmentForPlayer()
    {
       
        int timeID = (int)ServiceLocator.Instance.GetService<RunStatsService>().TimeID;
        ServiceLocator.Instance.GetService<UserSessionData>().InsertNewFragmentForPlayer(timeID);
 
    }
}
