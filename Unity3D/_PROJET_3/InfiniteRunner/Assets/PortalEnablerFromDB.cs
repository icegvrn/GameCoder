using System.Collections.Generic;
using UnityEngine;


public class PortalEnablerFromDB : MonoBehaviour
{
    void Start()
    {
        List<int> availablePortal = ServiceLocator.Instance.GetService<UserSessionData>().GetAvailableTimeForUser();

        if (availablePortal == null)
        {
            int rand = Random.Range(1, GetComponentsInChildren<PortalBehavior>().Length + 1);
            ServiceLocator.Instance.GetService<UserSessionData>().RegisterTimeForUser(rand);
            GetComponentsInChildren<PortalBehavior>()[rand - 1].IsAvailableForPlayer = true;
            availablePortal = new List<int>();
            availablePortal.Add(rand);
        }

        foreach (PortalDBContainer pdbc in GetComponentsInChildren<PortalDBContainer>())
        {
            PortalBehavior pb = pdbc.gameObject.GetComponent<PortalBehavior>();
            if (availablePortal.Contains((int)pdbc.IdFromDb))
            {
                pb.IsAvailableForPlayer = true;
                pb.Init();
            }
            else
            {
                pb.IsAvailableForPlayer = false;
                pb.Init();
            }
        }
    }

}
