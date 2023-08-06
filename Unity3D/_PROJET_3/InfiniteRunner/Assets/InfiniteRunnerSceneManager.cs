using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiniteRunnerSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
