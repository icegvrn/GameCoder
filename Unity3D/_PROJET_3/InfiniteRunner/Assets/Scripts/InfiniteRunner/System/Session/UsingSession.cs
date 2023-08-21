using System.Collections.Generic;
using UnityEngine;

public class UsingSession : MonoBehaviour
{

    [SerializeField] GameObject errorConnection;
    [SerializeField] List<GameObject> ObjectsToEnableOnSessionReady;

    public void Start()
    {
          ResetMessages();
     
        if (IsSessionReady())
        {
            EnableScene();
        }
    }

   public bool IsSessionReady()
    {
        try
        {
            DBUserData data = ServiceLocator.Instance.GetService<ISessionService>().Query.GetUserSessionData();

            if (data != null)
            {
               return true;
            }
            else
            {
                EnableError();
                return false;
            }
          
        }
        catch
        {
            EnableError();
            Debug.LogError("UsingSession : un probl�me a �t� rencontr� lors de la tentative de lecture de session.");
            return false;
        }
    }

    void EnableScene()
    {
        foreach (GameObject obj in ObjectsToEnableOnSessionReady) { obj.SetActive(true); }
    }

    void DisableScene()
    {
        foreach (GameObject obj in ObjectsToEnableOnSessionReady) { obj.SetActive(false); }
    }

    void EnableError()
    {
        DisableScene();
        Cursor.visible = true;
        errorConnection.SetActive(true);
        Debug.LogError("Impossible d'acc�der aux donn�es joueur. Veuillez vous rediriger vers l'�cran d'accueil.");
    }

    void ResetMessages()
    {
        errorConnection.SetActive(false);
    }

}
