using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    private ISessionService sessionService;
    private IGameSettingsService gameSettingsService;
    private IInputService inputService;
    private ISoundService soundLevelService;

    public enum Scene { initialization, homeMenu, gameHub, lvl_eypt, lvl_war, lvl_antiquity, lvl_japan, lvl_farwest, lvl_prehistoric, lvl_medieval, lvl_pirats, lvl_future }

    private Scene currentScene;
    public Scene sceneToLoadAfterInitialization;

    public ApplicationManager()
    {
        if (ServiceLocator.Instance.GetService<ApplicationManager>() == null)
        {
            ServiceLocator.Instance.RegisterService(this);
        }
        SetCurrentScene(Scene.initialization);
        InitBaseServices();
        RegisterServices();
       
    }

    private void Start()
    {
        LoadScene(sceneToLoadAfterInitialization);
    }
    private void InitBaseServices()
    {
        sessionService = new SessionManager();
        soundLevelService = new SoundLevelService();
        inputService = new InputService();
        gameSettingsService = new GameSettingsService(inputService, soundLevelService);
    }

    private void RegisterServices()
    {
        ServiceLocator.Instance.RegisterService(sessionService);
        ServiceLocator.Instance.RegisterService(gameSettingsService);
        ServiceLocator.Instance.RegisterService(inputService);
        ServiceLocator.Instance.RegisterService(soundLevelService);
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    private void SetCurrentScene(Scene scene)
    {
        currentScene = scene;
    }

    private void OnApplicationQuit()
    {
       sessionService.EndSession();
    }


}
