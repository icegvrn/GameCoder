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
        int rand = Random.Range(minObstaclesPerTerrain, maxObstaclesPerTerrain+1);
        for (int i=0; i<rand; i++) 
        {
            Obstacle obstacle = ChooseRandomObstacle();
            Spawn(obstacle);
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

        public void Reset()
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
