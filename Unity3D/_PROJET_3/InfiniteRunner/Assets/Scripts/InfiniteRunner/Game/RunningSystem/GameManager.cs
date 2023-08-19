using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager UIManager;
    [SerializeField] EnvironmentManager gameEnvironment;
    [SerializeField] FragmentManager fragmentManager;
    [SerializeField] CharacterAutoRunner character;
    [SerializeField] Camera mainCamera;

    Vector3 mainCameraInitialLocalPosition;
    void Start()
    {
        gameEnvironment.gameObject.SetActive(false);
        character.enabled = false;
        character.gameObject.SetActive(false);
        UIManager.DisableAll();
    }

    private void Update()
    {
        if (fragmentManager.FragmentDetected)
        {
            UIManager.SetFragmentDetectedVisible(true);
            fragmentManager.FragmentDetected = false;
        }
    }

    public void StartGame()
    {
        gameEnvironment.gameObject.SetActive(true);
        character.gameObject.SetActive(true);
        character.enabled = false;
        UIManager.SetHUDVisible(true);
        mainCamera.transform.parent = character.gameObject.transform;
        mainCameraInitialLocalPosition = mainCamera.transform.localPosition;   
        UIManager.SetStartCountdownVisible(true);
    }

 
    public void StopGame()
    {
        character.enabled = false;
        character.gameObject.SetActive(false);
        UIManager.SetHUDVisible(false);
        mainCamera.transform.parent = transform;
        ServiceLocator.Instance.GetService<RunStatsService>().ResetData();
    }

    public void StopGameAndEnvironment()
    {
        StopGame();
        gameEnvironment.gameObject.SetActive(false);

    }

    public void EnableHUD()
    {
        UIManager.SetHUDVisible(true);
    }

    public void DisableHUD()
    {
        UIManager.SetHUDVisible(false);
    }
 
    public void StartRun()
    {
        UIManager.SetStartCountdownVisible(false);
        ServiceLocator.Instance.GetService<RunStatsService>().StartTimer();
        character.enabled = true;
    }

    public void UserDeath()
    {
        StopGame();
        UIManager.SetPlayerDeadVisible(true);
    }

    public void RestartGame()
    {
       
        character.Reset();
        gameEnvironment.Reset();
        gameEnvironment.gameObject.SetActive(true);
        UIManager.SetHUDVisible(true);
        UIManager.SetPlayerDeadVisible(false); 

        mainCamera.transform.parent = character.gameObject.transform;
        mainCamera.transform.localPosition = mainCameraInitialLocalPosition;
        ServiceLocator.Instance.GetService<RunStatsService>().StartTimer();

    }

    public void Win()
    {
        StopGame();
        UIManager.SetPlayerWinVisible(true);
    }

    public void WinAndBestTime()
    {
        StopGame();
        UIManager.SetPlayerBestTimeVisible(true);
        UIManager.SetPlayerWinVisible(true);
    }

    public void GoToTheHub()
    {
        SceneManager.LoadScene(1);
    }

}
