using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Script "raccourci" permettant d'obtenir les informations de ScoreTime de l'utilisateur.
/// </summary>
[RequireComponent(typeof(PortalDBContainer))]
public class BestScoreTimeFromDB : MonoBehaviour
{
   public DBUsers_TimeJoinTime GetBestTimeInformations()
    {
       int time = (int)gameObject.GetComponent<PortalDBContainer>().IdFromDb;
       DBUsers_TimeJoinTime dbInfos = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllInformationsForUserByTime(time);
       return dbInfos;
    }

    public List<int> GetFragmentsStats()
    {
        int time = (int)gameObject.GetComponent<PortalDBContainer>().IdFromDb;
        List<int> dbInfos = ServiceLocator.Instance.GetService<ISessionService>().Query.GetAllFoundFragmentsForTime(time).ToList();
        return dbInfos;
    }
}
