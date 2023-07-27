using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private List<Obstacle> listObstacles;
    public List<Obstacle> ListObstacles { get { return listObstacles; } }

    private List<GameObject> listTerrains;
    public List<GameObject> ListTerrains { get { return listTerrains; } }

    private float totalFrequency;


    void Start()
    {
        listTerrains = new List<GameObject>();
        CalcTotalFrequency();
    }


    void Update()
    {
        
    }

    public void AddSpawnableTerrain(GameObject terrain)
    {
        listTerrains.Add(terrain);
        terrain.GetComponent<ObstacleSpawner>().Spawn(listObstacles[0]);    
    }

    public void AddSpawnableTerrain(List<GameObject> terrains)
    {
        foreach (GameObject terrain in listTerrains) { 
            listTerrains.Add(terrain);
            terrain.GetComponent<ObstacleSpawner>().Spawn(listObstacles[0]);
        }
    }

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
        Obstacle obstacle = ChooseRandomObstacle();
        terrain.GetComponent<ObstacleSpawner>().Spawn(obstacle);
    }

   
    private void CalcTotalFrequency()
    {

        totalFrequency = 0f;
        foreach (Obstacle obstacle in listObstacles)
        {
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
        foreach (Obstacle obstacle in listObstacles)
        {
            cumulativeFrequency += obstacle.Frequency;
            if (randomValue <= cumulativeFrequency)
            {
                spObstacle = obstacle;
                break;
            }
        }

        return spObstacle;
    }

}
