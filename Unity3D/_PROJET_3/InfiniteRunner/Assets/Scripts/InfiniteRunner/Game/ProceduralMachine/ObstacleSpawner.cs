using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de faire spawn un obstacle al�atoire parmis la pool d'obstacle dans chaque slot d'obstacle qui lui indiqu�.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{

    private float totalFrequency;

    [SerializeField] private int minObstaclesPerTerrain;
    [SerializeField] private int maxObstaclesPerTerrain;

    [SerializeField] private ObjectPool obstaclePool;
    public ObjectPool ObstaclePool { get { return obstaclePool; }  set { obstaclePool = value; } }

    [SerializeField] private List<GameObject> obstacleInPlay;

    [SerializeField] private List<GameObject> obstacleSlots;

    private bool obstaclesGenerated = false;

    void Start()
    {
        InitObstacleSpawner();
    }

    void Update()
    {
        if (obstaclePool.PoolIsReady && !obstaclesGenerated)
        {
           InitObstacles();
        }
    }

    void InitObstacleSpawner()
    {
        obstacleInPlay = new List<GameObject>();
    }

    /// <summary>
    /// Initie les obstacles pour la premi�re fois en calculant une fr�quence totale (utilis�e pour d�terminer la fr�quence d'apparition de chaucun en pourcentage) et fait spawn un obstacle dans chaque slot.
    /// </summary>
    void InitObstacles()
    {
        CalcTotalFrequency();
        SpawnObstacleOnTerrain();
        obstaclesGenerated = true;
    }

/// <summary>
/// M�thode permettant d'instancier un nouvel obstacle au hasard � partir d'une liste d'obstacle dans chaque slot d'obstacle.
/// </summary>
    public void SpawnObstacleOnTerrain()
    {
       foreach (GameObject gameObject in obstacleSlots)
        {
            Obstacle obstacle = ChooseRandomObstacle();
            Spawn(obstacle, gameObject);
        }
    }

   
    /// <summary>
    /// M�thode prenant en compte la fr�quence d'apparition de chaque obstacle pour calculer une fr�quence totale permettant de faire un pourcentage d'apparition pour chacun.
    /// </summary>
    private void CalcTotalFrequency()
    {
        totalFrequency = 0f;

        foreach (GameObject obj in obstaclePool.ObjectList)
        {
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            totalFrequency += obstacle.Frequency;
        }
    }

    /// <summary>
    /// M�thode permettant de d�terminer l'obstacle � faire spawn en tenant compte de sa fr�quence d'apparition configur�e.
    /// </summary>
    /// <returns></returns>
    private Obstacle ChooseRandomObstacle()
    {
        while (true)
        {
            float randomPercentage = Random.Range(0f, 1f);
            Obstacle randomObstacle = GetRandomObstacleWithPercentageGreaterOrEqual(randomPercentage);

            if (randomObstacle != null)
            {
                return randomObstacle;
            }
        }
    }

    /// <summary>
    /// M�thode qui s�lectionne un obstacle au hasard selon son pourcentage de chance de sortir.
    /// </summary>
    /// <param name="targetPercentage"></param>
    /// <returns></returns>
    private Obstacle GetRandomObstacleWithPercentageGreaterOrEqual(float targetPercentage)
    {
        // On fait d'abord une liste de tous les candidats potentiels : ceux avec le pourcentage sup�rieur ou �gal � celui tir�
        List<Obstacle> wantedObstacles = new List<Obstacle>();

        foreach (GameObject obj in obstaclePool.ObjectList)
        {
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            float obstaclePercentage = (obstacle.Frequency / totalFrequency);

            if (obstaclePercentage >= targetPercentage)
            {
                wantedObstacles.Add(obstacle);
            }
        }

        // Puis on tire un obstacle au hasard dans la liste de ceux qui correspondent
        if (wantedObstacles.Count > 0)
        {
            int randomIndex = Random.Range(0, wantedObstacles.Count);
            return wantedObstacles[randomIndex];
        }

        return null;
    }

    /// <summary>
    /// M�thode faisant spawn un obstacle dans un slot donn�.
    /// </summary>
    /// <param name="obstacle"></param>
    /// <param name="slot"></param>
    void Spawn(Obstacle obstacle, GameObject slot)
    {
                GameObject newObstacle = obstaclePool.GetPooledObject(obstaclePool.ObjectList.IndexOf(obstacle.gameObject));
                newObstacle.transform.parent = slot.transform;
                newObstacle.transform.localPosition = Vector3.zero;    
                obstacleInPlay.Add(newObstacle);     
    }

    /// <summary>
    /// Retourne le premier obstacle de la liste d'obstacle. Utilis� dans la g�n�ration des fragments notamment.
    /// </summary>
    /// <returns></returns>
    public GameObject GetFirstObstacle()
    {
        return obstacleInPlay[0];
    }

    /// <summary>
    /// Reset qui r�initialise non seulement les obstacles, mais appel aussi le reset de leurs animations et leurs collectables.
    /// </summary>
    public void Reset()
    {
        foreach (GameObject obstacleSlot in obstacleSlots)
        {
            ResetCollectables(obstacleSlot);
            ResetAnimation(obstacleSlot);
        }
        ResetObstacle(); 
    }

 
    /// <summary>
    /// M�thode appelant pour chaque CollectableManager le reset de celui-ci.
    /// </summary>
    /// <param name="obstacleSlot"></param>
    void ResetCollectables(GameObject obstacleSlot)
    {
        ObstacleCollectableManager[] collectablesManagers = obstacleSlot.GetComponentsInChildren<ObstacleCollectableManager>();

        foreach (ObstacleCollectableManager collectablesManager in collectablesManagers)

        {
            collectablesManager.Reset();
        }
    }

    /// <summary>
    /// M�thode appelant pour chaque AnimationManager le reset de celui-ci.
    /// </summary>
    void ResetAnimation(GameObject obstacleSlot)
    {
        ObstacleAnimationManager[] animationsManagers = obstacleSlot.GetComponentsInChildren<ObstacleAnimationManager>();

        foreach (ObstacleAnimationManager animationsManager in animationsManagers)

        {
            animationsManager.Reset();

        }

    }


    void ResetObstacle()
    {
        foreach (GameObject obj in obstacleInPlay)
        {
            obstaclePool.ReleasedPooledObject(obj);
            obj.transform.parent = obstaclePool.gameObject.transform;
        }

        obstacleInPlay.Clear();
        obstaclesGenerated = false;
    }
    
  

}
