using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet d'activer ou d�sactiver les portails accessibles dans le Hub � partir des donn�es utilisateurs dans la BDD (a-t-il acc�s ou non ?)
/// </summary>
public class PortalEnablerFromDB : MonoBehaviour
{
    [SerializeField] GameObject newPortalUI;
    private SQLiteSessionDataQuery db;

    void Start()
    {
        db = ServiceLocator.Instance.GetService<ISessionService>().Query;

        CheckIfANewPortalIsAvailable();
        EnableAvailablePortalsForUser();  
    }

    /// <summary>
    /// Si un nouveau portail est d�bloquable pour l'utilisateur, on le d�bloque et on affiche un message � l'�cran
    /// </summary>
    void CheckIfANewPortalIsAvailable()
    {
        bool portalUnlocked = db.UnlockNewTimeIfAvailable();

        if (portalUnlocked)
        {
            newPortalUI.SetActive(true);
        }
    }

    /// <summary>
    /// Active les portails auxquels l'utilisateur a acc�s en envoyant un bool � son PortalBehavior
    /// </summary>
    void EnableAvailablePortalsForUser()
    {
        List<int> availablePortal = db.GetAvailableTimeForUser();

        foreach (PortalDBContainer pdbc in GetComponentsInChildren<PortalDBContainer>())
        {
            PortalBehavior pb = pdbc.gameObject.GetComponent<PortalBehavior>();
            if (pb != null)
            {
                pb.IsAvailableForPlayer = availablePortal.Contains((int)pdbc.IdFromDb);
                pb.Init();
            }
        }
    }

}
