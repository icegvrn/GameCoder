using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    private List<GameObject> listTerrains;
    public List<GameObject> ListTerrains { get { return listTerrains; } }

    private float totalFrequency;

    [SerializeField] private int minObstaclesPerTerrain;
    [SerializeField] private int maxObstaclesPerTerrain;

    [SerializeField] private ObjectPool obstaclePool;

    void Start()
    {
        listTerrains = new List<GameObject>();
        CalcTotalFrequency();
    }


    void Update()
    {
        
    }

    //public void AddSpawnableTerrain(GameObject terrain)
    //{
    //    listTerrains.Add(terrain);
    //    Obstacle obstacle = ChooseRandomObstacle();
    //    Spawn(terrain, obstacle);
    //}

    //public void AddSpawnableTerrain(List<GameObject> terrains)
    //{
    //    foreach (GameObject terrain in listTerrains) { 
    //        listTerrains.Add(terrain);
    //        Obstacle obstacle = ChooseRandomObstacle();
    //        Spawn(terrain, obstacle);
    //    }
    //}

    public void RemoveSpawnableTerrain(GameObject terrain)
    {
        listTerrains.Remove(terrain);
    }

    public void RemoveSpawnableTerrain(List<GameObject> terrains)
    {
        foreach (GameObject terrain in terrains) { listTerrains.Remove(terrain); }
    }

    public void RemoveAllSpawnableTerrain()
    {
        for (int i = listTerrains.Count; i > 0; i--) { listTerrains.RemoveAt(i); }
    }

    public void SpawnObstacleOnTerrain(GameObject terrain)
    {
        int rand = Random.Range(minObstaclesPerTerrain, maxObstaclesPerTerrain+1);
        for (int i=0; i<rand; i++) 
        {
            Obstacle obstacle = ChooseRandomObstacle();
            Spawn(terrain, obstacle);
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

    void Spawn(GameObject terrain, Obstacle obstacle)
    {
        //Rajouter un script pour éviter les collisions

        foreach (GameObject obj in obstaclePool.ObjectList)
        {
            if (obj == obstacle.gameObject)
            {
                GameObject spObstacle = obstaclePool.GetPooledObject(obstaclePool.ObjectList.IndexOf(obj));
                spObstacle.transform.parent = terrain.transform;
                Vector3 obstaclePosition = new Vector3(0, spObstacle.transform.position.y, 0);
                float randX = Random.Range(-terrain.GetComponent<Renderer>().bounds.extents.x, terrain.GetComponent<Renderer>().bounds.extents.x);
                obstaclePosition.x = randX;
                spObstacle.transform.localPosition = obstaclePosition;

                break;
            }
        }
      
    }

}
