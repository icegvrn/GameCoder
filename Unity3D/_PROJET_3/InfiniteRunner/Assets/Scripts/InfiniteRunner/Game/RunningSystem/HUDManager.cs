using UnityEngine;

/// <summary>
/// Manager qui met à jour l'UI durant le run pour informer l'utilisateur de son état en cours.
/// </summary>
[RequireComponent(typeof(HUDDecomposer))]
public class HUDManager : MonoBehaviour
{
    private HUDDecomposer decomposer;
    private IRunningGameService runStatsService;
    private bool initialized;

    public void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Initialisation des composants nécessaire au HUD : la composition de celui-ci, les stats du jeu en cours et le nom du joueur issu de la session en cours.
    /// </summary>
    void Initialization()
    {
        decomposer = GetComponent<HUDDecomposer>();
        runStatsService = ServiceLocator.Instance.GetService<IRunningGameService>();
        decomposer.Username.text = ServiceLocator.Instance.GetService<ISessionService>().UserData.CurrentUser;
        initialized = true;
    }

    void Update()
    {
        if(initialized)
        {
            UpdateInformations();
        }
      
    }

    /// <summary>
    /// Modifie le canvas de l'HUD en y ajoutant les informations de la partie en cours.
    /// </summary>
    void UpdateInformations()
    {
        decomposer.Timer.text = runStatsService.RunTime.ToString() + " s";
        decomposer.Essences.text = runStatsService.UserEssences.ToString() + "/" + runStatsService.RunGoal.ToString();
        decomposer.BestTimeAnimator.TextsToAnimate[0] = runStatsService.RunTime + " s";
        UpdateUserLife();
    }

    /// <summary>
    /// Détermine combien d'icônes de vie il faut afficher en fonction de la vie actuelle du joueur. Attention : présuppose que le nombre d'icônes et les PV correspondent. Si besoin de plus de flexibilité à l'avenir, revoir la méthode.
    /// </summary>
    void UpdateUserLife()
    {
        for (int i = 0; i < runStatsService.MaxUserLife; i++)
        {
            decomposer.LifeIcons[i].enabled = runStatsService.UserLife >= (runStatsService.MaxUserLife - i);
        }
    }
}
