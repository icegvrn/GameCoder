using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UsingSession : MonoBehaviour
{

    [SerializeField] GameObject errorConnection;
    [SerializeField] List<GameObject> ObjectsToEnableOnSessionReady;
    bool sessionReady;
    // Start is called before the first frame update
    void Start()
    {
        errorConnection.SetActive(false);
        CheckSession();

        if (sessionReady)
        {
            EnableScene();
        }
    }

    void CheckSession()
    {
        try
        {
            DBUserData data = ServiceLocator.Instance.GetService<SessionManager>().GetUserSessionData();

            if (data != null)
            {
                sessionReady = true;
            }
            else
            {
                EnableError();
            }
          
        }
        catch
        {
            EnableError();
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
        errorConnection.SetActive(true);
        Debug.LogError("Impossible d'accéder aux données joueur. Veuillez vous rediriger vers l'écran d'accueil.");
    }

    private void OnApplicationQuit()
    {
        ServiceLocator.Instance.GetService<SessionManager>().EndSession();
    }
}
