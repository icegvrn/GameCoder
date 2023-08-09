using System.Collections.Generic;
using UnityEngine;


public class PortalEnablerFromDB : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        List<int> availablePortal = ServiceLocator.Instance.GetService<UserSessionData>().GetAvailableTimeForUser();
     
        if (availablePortal != null )
        {
            foreach (PortalDBContainer pb in GetComponentsInChildren<PortalDBContainer>())
            {
                
                if (availablePortal.Contains((int)pb.IdFromDb))
                {
                    pb.gameObject.GetComponent<PortalBehavior>().IsAvailableForPlayer = true;
                }
            }
        }
        else
        {
            int rand = Random.Range(0, GetComponentsInChildren<PortalBehavior>().Length+1);
            ServiceLocator.Instance.GetService<UserSessionData>().RegisterTimeForUser(rand);
            GetComponentsInChildren<PortalBehavior>()[rand-1].IsAvailableForPlayer = true;
        }

            foreach (PortalBehavior pb in GetComponentsInChildren<PortalBehavior>())
            {
  
            pb.Init();
            }

    }

}
