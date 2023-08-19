using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSceneLoader : MonoBehaviour
{
    [SerializeField] ApplicationManager.Scene sceneToLoad;

    public void LoadScene()
    {
        ServiceLocator.Instance.GetService<ApplicationManager>().LoadScene(sceneToLoad);
    }
}
