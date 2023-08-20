using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int indexSceneToLoad;

    public void Load()
    {
        SceneManager.LoadScene(indexSceneToLoad);
    }
}
