
using System.Collections.Generic;
using UnityEngine;
using static InputService;


/// <summary>
/// Permet le spawn d'un fragment durant une course : grâce à un système de seuil et de pourcentage, le joueur peut avoir un, deux, trois ou zéro fragments à apparaitre sur son chemin si un fragment est disponible.
/// </summary>
[RequireComponent(typeof(MapGenerator))]
public class FragmentManager : MonoBehaviour
{
  
    private bool triedToInvokeFragments;
    private List<int> fragmentScoreThresholds;
    private int fragmentsNbUserCanHave;
    private int alreadySpawnedFragment;

    // Composant dont le manager a besoin : la map, une connexion à la bdd, les stats de jeu
    private MapGenerator map;
    private RunStatsService runStatsService;
    private SQLiteSessionDataQuery db;

    // Pour faire apparaitre le fragment en version "cheat"
    private IInputService input;

    // Indique si un fragment a été détecté ou non
    private bool fragmentDetected;
    public bool FragmentDetected { get { return fragmentDetected; } set { fragmentDetected = value; } }


    private void Start()
    {
        InitFragmentDependancies();
        InitManager();
     }

    private void Update()
    {
        int playerScore = runStatsService.UserEssences;

        // Si on a pas encore tenté de faire apparaitre des fragments
        if (!triedToInvokeFragments) 
        {
            // Si le joueur a dépassé un tier de son score, on essaie de faire spawn un fragment
            if (playerScore >= runStatsService.RunGoal / 3) 
            {
                TryInvokeFragments(playerScore);   
            }
        }

        else 
        {
            // Si on a déjà déterminé que des fragments devaient spawn, on regarde si le score est atteint pour le faire spawn
            CheckIfFragmentWillSpawn(playerScore); 
        }

        // (pour démo) Cheat pour pouvoir faire apparaitre un fragment si dispo à l'aide d'une touche 
        if (input.GetKeyUp(ActionKey.cheatGemme) && IsAFragmentAvailableOverInt(alreadySpawnedFragment))
        {
            SpawnFragment();
        }
    }

    void TryInvokeFragments(int playerScore)
    {
        // On tire au sort pour savoir si l'user doit avoir des fragments
        if (ShouldSpawnFragment()) 
        {
            // Si oui on tire au sort si c'est 1, 2 ou 3 fragments
            fragmentsNbUserCanHave = CalculateNumFragments(); 

            for (int i = 0; i < fragmentsNbUserCanHave; i++)
            {
                // Pour chaque fragment, on définit un score auquel on va le faire apparaitre (pour pas qu'ils apparaissent tous en même temps)
                int score = CalculateScoreTarget(playerScore);
                // On enregistre ce score à atteindre dans une liste
                fragmentScoreThresholds.Add(score); 
            }
        }
        triedToInvokeFragments = true;
    }

    void CheckIfFragmentWillSpawn(int playerScore)
    {
        if (playerScore >= runStatsService.RunGoal / 3)
        {
            // Si on a déjà déterminé que des fragments devaient apparaitre et qu'ils ont un seuil à atteindre pour se déclancher
            if (fragmentScoreThresholds.Count > 0) 
            {
                for (int i = fragmentScoreThresholds.Count - 1; i >= 0; i--)
                {
                    // Quand le score est atteint
                    if (playerScore >= fragmentScoreThresholds[i]) 
                    {
                        // On vérifie s'il y a encore suffisamment de fragments en bdd
                        if (IsAFragmentAvailableOverInt(alreadySpawnedFragment)) 
                        {
                            //Si oui on spawnet on retire du tableau 
                            SpawnFragment(); 
                            fragmentScoreThresholds.RemoveAt(i); 
                        }
                    }
                }
            }
        }
    }

    void InitFragmentDependancies()
    {
        map = GetComponent<MapGenerator>();
        input = ServiceLocator.Instance.GetService<IInputService>();
        runStatsService = ServiceLocator.Instance.GetService<RunStatsService>();
        db = ServiceLocator.Instance.GetService<ISessionService>().Query;
    }

    void InitManager()
    {
        fragmentScoreThresholds = new List<int>();
        alreadySpawnedFragment = 0;
    }

    /// <summary>
    /// Détermine si un fragment doit apparaitre ou pas selon un pourcentage de chance
    /// </summary>
    /// <returns></returns>
    private bool ShouldSpawnFragment()
    {
        float nb = Random.Range(0f, 1f);
        bool rand = nb <= 0.5f;

        // 50% de chance d'apparition d'un fragment pour la démo ; normalement réglé à 5%
        //bool rand = nb <= 0.05f;(5%)

        return rand; 
    }

    /// <summary>
    /// Détermine combien de fragments doivent apparaitre, selon un pourcentage de chance
    /// </summary>
    /// <returns></returns>
    private int CalculateNumFragments()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= 0.01f)
        {
         // 1% de chance d'obtenir deux fragments
            return 2; 
        }
        else if (randomValue <= 0.005f)
        {
          // 0.5% de chance d'obtenir trois fragments
            return 3; 
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// Calcul de façon aléatoire combien le joueur doit avoir de score pour déclancher le fragment. Aléatoire basé sur une fourchette entre le score actuel et le score max
    /// </summary>
    /// <param name="playerScore"></param>
    /// <returns></returns>
    private int CalculateScoreTarget(int playerScore)
    {
        int minScore = playerScore + 10;
        int maxScore = runStatsService.RunGoal - 10;
        int result = Random.Range(minScore, maxScore);
        return result;
    }

    /// <summary>
    /// Méthode qui appelle l'instanciation du fragment en récupérant le premier fragment spawner du prochain chunk de map
    /// </summary>
    private void SpawnFragment()
    {
        FragmentDetected = true;
        alreadySpawnedFragment++;

        ObstacleSpawner obSpawner = map.PrefabOnPlay[1].GetComponentInChildren<ObstacleSpawner>();
        FragmentSpawner fragSpawner = obSpawner.GetFirstObstacle().GetComponentInChildren<FragmentSpawner>();
       
        fragSpawner.SpawnFragment();
    }

    /// <summary>
    /// Méthode permettant d'interroger la BDD pour savoir s'il y a un certain nombre de fragments encore disponsible pour une époque donnée.
    /// </summary>
    /// <param name="nb">Nombre de fragments que l'on veut disponible</param>
    /// <returns></returns>
    private bool IsAFragmentAvailableOverInt(int nb)
    {
      bool result = db.AreFragmentsAvailableOverInt((int)runStatsService.TimeID, nb);
        return result;
    }

}

