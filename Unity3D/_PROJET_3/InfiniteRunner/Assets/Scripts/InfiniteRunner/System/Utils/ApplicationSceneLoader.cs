using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSceneLoader : MonoBehaviour
{
    [SerializeField] ApplicationManager.Scene sceneToLoad;

    public void LoadScene()
    {

        if (ServiceLocator.Instance.IsServiceRegistered<ApplicationManager>())
        {
            ServiceLocator.Instance.GetService<ApplicationManager>().LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
