using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet d'activer ou désactiver les portails accessibles dans le Hub à partir des données utilisateurs dans la BDD (a-t-il accès ou non ?)
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
    /// Si un nouveau portail est débloquable pour l'utilisateur, on le débloque et on affiche un message à l'écran
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
    /// Active les portails auxquels l'utilisateur a accès en envoyant un bool à son PortalBehavior
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
