using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        
        obstacleInPlay = new List<GameObject>();
        
    }



    void Update()
    {
        if (obstaclePool.PoolIsReady && !obstaclesGenerated)
        {
            CalcTotalFrequency();
            SpawnObstacleOnTerrain();
            obstaclesGenerated = true;
           
        }
    }

    public void SpawnObstacleOnTerrain()
    {
       foreach (GameObject gameObject in obstacleSlots)
        {
            Obstacle obstacle = ChooseRandomObstacle();
            Spawn(obstacle, gameObject);
        }
    }

   
    private void CalcTotalFrequency()
    {

        totalFrequency = 0f;
        foreach (GameObject obj in obstaclePool.ObjectList)
        {
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            totalFrequency += obstacle.Frequency;
        }
    }

    private Obstacle ChooseRandomObstacle()
    {
        // Générer un nombre aléatoire entre 0 et la somme totale des fréquences
        float randomValue = Random.Range(0f, totalFrequency);
        float cumulativeFrequency = 0f;
        Obstacle spObstacle = null;

        // Parcourir les obstacles et choisir celui dont la probabilité cumulée est supérieure au nombre aléatoire
        foreach (GameObject obj in obstaclePool.ObjectList)
        {
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            cumulativeFrequency += obstacle.Frequency;
            if (randomValue <= cumulativeFrequency)
            {
                spObstacle = obstacle;
                break;
            }
        }

        return spObstacle;
    }

    void Spawn(Obstacle obstacle)
    {
        //Rajouter un script pour éviter les collisions


        foreach (GameObject obj in obstaclePool.ObjectList)
        {
            if (obj == obstacle.gameObject)
            {
                GameObject spObstacle = obstaclePool.GetPooledObject(obstaclePool.ObjectList.IndexOf(obj));
                Vector3 parentExtents = gameObject.GetComponent<Renderer>().localBounds.extents;
                spObstacle.transform.parent = gameObject.transform;
                Vector3 obstacleLocalPosition = Vector3.zero;
                float randX = Random.Range(-parentExtents.x, parentExtents.x);
                obstacleLocalPosition.x += randX;
                spObstacle.transform.localPosition = obstacleLocalPosition;
                obstacleInPlay.Add(spObstacle);

                break;
            }
        }
    }

    void Spawn(Obstacle obstacle, GameObject gameObject)
    {
        //Rajouter un script pour éviter les collisions

                GameObject spObstacle = obstaclePool.GetPooledObject(obstaclePool.ObjectList.IndexOf(obstacle.gameObject));
                spObstacle.transform.parent = gameObject.transform;
                spObstacle.transform.localPosition = Vector3.zero;    
        
        // Rajouter un script ici si contact avec le joueur, destruction, sinon... 

                obstacleInPlay.Add(spObstacle);
           
            
  
    }

    public void Reset()
    {
        foreach (GameObject obstacleSlot in obstacleSlots)
        {
            ResetCollectables(obstacleSlot);
            ResetAnimation(obstacleSlot);
        }
        ResetObstacle();

        
    }

    void ResetCollectables(GameObject obstacleSlot)
    {

        ObstacleCollectableManager[] collectablesManagers = obstacleSlot.GetComponentsInChildren<ObstacleCollectableManager>();

        foreach (ObstacleCollectableManager collectablesManager in collectablesManagers)

        {
            collectablesManager.Reset();

        }
    }

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

    public GameObject GetFirstObstacle()
    {
        return obstacleInPlay[0];
    }

}
