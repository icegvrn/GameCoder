
using System.Collections.Generic;
using UnityEngine;

public class FragmentManager : MonoBehaviour
{
    [SerializeField] private PathGenerator mapGenerator;
    private bool fragmentDetected;
    public bool FragmentDetected { get { return fragmentDetected; } set { fragmentDetected = value; } }
    // Il faut rajouter une vérif de si oui ou non y'a des fragments en BDD avant d'en faire apparaitre... 

    private int totalPointsNeeded; // Points totaux nécessaires pour terminer la course
    private bool triedToInvokeFragments;
    private List<int> fragmentScoreThresholds;
    private int fragmentsNbUserCanHave;
    private int alreadySpawnedFragment;
    private void Start()
    {
        totalPointsNeeded = ServiceLocator.Instance.GetService<RunStatsService>().RunGoal;
        fragmentScoreThresholds = new List<int>();
        alreadySpawnedFragment = 0;

     }

    private void Update()
    {
        int playerScore = ServiceLocator.Instance.GetService<RunStatsService>().UserEssences;

        if (!triedToInvokeFragments)
        {
            if (playerScore >= totalPointsNeeded / 3)
            {
                if (ShouldSpawnFragment())
                {
                    fragmentsNbUserCanHave = CalculateNumFragments();
                    for (int i = 0; i < fragmentsNbUserCanHave; i++)
                    {
                        int score = CalculateScoreTarget(playerScore);
                        fragmentScoreThresholds.Add(score);  
                    }  
                   
                }
              triedToInvokeFragments = true;
            }
          
        }

        else
        {
            if (playerScore >= totalPointsNeeded / 3)
            {
                Debug.Log("FRAGMENT MANAGER : Je suis censé passer dans le test i avec un i commence à  " + (fragmentScoreThresholds.Count));
             
                if (fragmentScoreThresholds.Count > 0)
                {
                    for (int i = fragmentScoreThresholds.Count - 1; i >= 0; i--)
                    {
                        Debug.Log("FRAGMENT MANAGER : Mon score joueur est de " + playerScore + " et pour déclancher je dois atteindre " + fragmentScoreThresholds[i]);
                        if (playerScore >= fragmentScoreThresholds[i])
                        {
                            if (IsAFragmentAvailableOverInt(alreadySpawnedFragment))
                            {
                                SpawnFragment();
                                fragmentScoreThresholds.RemoveAt(i);
                            }
                        }
                    }
                }
               
            }
        }



        if (Input.GetKeyUp(KeyCode.M) && IsAFragmentAvailableOverInt(alreadySpawnedFragment))
        {
            SpawnFragment();
        }
    }

    private bool ShouldSpawnFragment()
    {
        float nb = Random.Range(0f, 1f);
        //bool rand = nb <= 0.05f;(5%)
        bool rand = nb <= 0.5f;
        Debug.Log("FRAGMENTMANAGER : JE REGARDE SI JE DOIS FAIRE POP DES FRAGMENTS ET JE DIS : " + rand + " car j'ai tiré " + nb);
        return rand; // 50% de chance d'apparition d'un fragment
    }

    private int CalculateNumFragments()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= 0.01f)
        {
            Debug.Log(" FRAGMENTMANAGER : TU AURAS LE DROIT A 2 FRAG");
            return 2; // 1% de chance d'obtenir deux fragments
        }
        else if (randomValue <= 0.005f)
        {
            Debug.Log("FRAGMENTMANAGER : TU AURAS LE DROIT A 3 FRAG");
            return 3; // 0.5% de chance d'obtenir trois fragments
        }
        else
        {
            Debug.Log("FRAGMENTMANAGER : TU AURAS LE DROIT A 1 FRAG");
            return 1;
        }
    }

    private int CalculateScoreTarget(int playerScore)
    {
        int minScore = playerScore + 10;
        int maxScore = totalPointsNeeded - 10;
        int result = Random.Range(minScore, maxScore);
        Debug.Log("FRAGMENTMANAGER: TU DOIS FAIRE UN SCORE DE " + result);
        return result;
    }

    private void SpawnFragment()
    {
        FragmentDetected = true;
        alreadySpawnedFragment++;
        ObstacleSpawner obSpawner = mapGenerator.prefabOnPlay[1].GetComponentInChildren<ObstacleSpawner>();
        FragmentSpawner fragSpawner = obSpawner.GetFirstObstacle().GetComponentInChildren<FragmentSpawner>();
        fragSpawner.SpawnFragment();
    }

    private bool IsAFragmentAvailableOverInt(int nb)
    {
      bool result = ServiceLocator.Instance.GetService<UserSessionData>().AreFragmentsAvailableOverInt((int)ServiceLocator.Instance.GetService<RunStatsService>().TimeID, nb);
        return result;
    }

}

