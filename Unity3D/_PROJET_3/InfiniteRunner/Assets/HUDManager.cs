using UnityEngine;

[RequireComponent(typeof(HUDDecomposer))]
public class HUDManager : MonoBehaviour
{
    private HUDDecomposer decomposer;
    private RunStatsService runStatsService;
    private bool initialized;
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("J'initialise le HUD manager");
        decomposer = GetComponent<HUDDecomposer>();
        runStatsService = ServiceLocator.Instance.GetService<RunStatsService>();
        decomposer.Username.text = ServiceLocator.Instance.GetService<ISessionService>().UserData.CurrentUser;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(initialized)
        {
            decomposer.Timer.text = runStatsService.RunTime.ToString() + " s";
            decomposer.Essences.text = runStatsService.UserEssences.ToString() + "/" + runStatsService.RunGoal.ToString();
            decomposer.BestTimeAnimator.textsToAnimate[0] = runStatsService.RunTime + " s";
            UpdateUserLife();
        }
      
    }

    void UpdateUserLife()
    {
        for (int i = 0; i < runStatsService.MaxUserLife; i++)
        {
            decomposer.LifeIcons[i].enabled = runStatsService.UserLife >= (runStatsService.MaxUserLife - i);
        }
    }
}
