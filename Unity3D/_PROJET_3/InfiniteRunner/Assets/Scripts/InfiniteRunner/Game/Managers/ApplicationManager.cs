using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    private ISessionService sessionService;
    private IGameSettingsService gameSettingsService;
    private IInputService inputService;
    private ISoundService soundLevelService;

    public enum Scene { Initialization, HomeMenu, GameHub, Lvl_egypt, Lvl_war, Lvl_antiquity, Lvl_japan, Lvl_farwest, Lvl_prehistoric, Lvl_medieval, Lvl_pirats, LvlFuture }

    private Scene currentScene;
    public Scene sceneToLoadAfterInitialization;


   void Awake()
    {
      
        if (ServiceLocator.Instance.IsServiceRegistered<ApplicationManager>())
        {
            ServiceLocator.Instance.UnregisterService<ApplicationManager>();
        }

        ServiceLocator.Instance.RegisterService(this);

        SetCurrentScene(Scene.Initialization);
        InitBaseServices();
        RegisterServices();    
    }

    private void Start()
    {
        gameSettingsService.SetSettings();
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
        SetCurrentScene(scene);
    }

    private void SetCurrentScene(Scene scene)
    {
        if (scene != currentScene)
        {
            currentScene = scene;
        }
    }

    private void OnApplicationQuit()
    {
        if (sessionService != null)
        {
            sessionService.EndSession();
        }
    }

}
