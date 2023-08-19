using UnityEngine;
using System.Collections.Generic;


public class DecorGenerator : MonoBehaviour
{
    //Elements du décor
    public GameObject[] decorPrefabs;

    //Distance de génération des objets
    public int spawnDistance = 50;

    // Distance derrière laquelle les objets sont désactivés
    public int despawnDistance = 100;

    // Distance minimum à maintenir entre les objets à spawn et le joueur
    public float minDistanceToPlayer = 10f;

    // Transform du joueur pour avoir sa position
    public Transform player;

    // Layer sur lequel se trouve le chemin tracé, pour ne pas faire pop des décors dessus
    public LayerMask pathMask;

    // Liste des objets actuellement en cours dans la scène
    private List<GameObject> decorObjects = new List<GameObject>();


    // Limiter la boucle while
    private int spawnAttempts = 0;
    private const int maxSpawnAttempts = 100;


    public bool isInit = false;

    // Nombre d'objets à générer
    public int startedObjectsNb;


    /// <summary>
    /// Initialise la possition de départ du joueur et génère des premiers décors.
    /// </summary>
    public void Init()
    {
        GenerateInitialDecorWithoutPath();
        isInit = true;
    }

     void Start()
    {
        Init();
    }

    public void ProceduralUpdate()
    {
    GenerateDecors();
    }

   void Update()
    {
        GenerateDecors();
    }

    /// <summary>
    /// Génère des décors devant le joueur dès que le joueur passe devant un décor
    /// </summary>
    private void GenerateDecors()
    {

        Vector3 playerPos = player.position;

        for (int i = decorObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = decorObjects[i];
            float distanceToObject = Vector3.Distance(playerPos, obj.transform.position);
      
            if (distanceToObject >= despawnDistance && obj.transform.position.z < playerPos.z)
            {
                Destroy(obj);
                decorObjects.RemoveAt(i);
            }
        }

        GenerateDecorAreaWithoutPath();
      
    }

    private void GenerateInitialDecorWithoutPath()
    {

        int numDecorPrefabs = decorPrefabs.Length;
        spawnAttempts = 0;

        while (decorObjects.Count < startedObjectsNb)
        {
            Vector3 randomPos = Random.insideUnitSphere * spawnDistance;
            Vector3 spawnPosition = player.transform.position + randomPos;
            spawnPosition.y = 0f;

            GameObject decorObject = null;
            if (Vector3.Distance(spawnPosition, player.transform.position) >= minDistanceToPlayer)
            {
                GameObject decorPrefab = decorPrefabs[Random.Range(0, numDecorPrefabs)];
                decorObject = null;

                // Incrémenter le compteur de tentatives
                spawnAttempts++;

                // Vérifier si le nombre maximum de tentatives est atteint, et sortir de la boucle si c'est le cas
                if (spawnAttempts >= maxSpawnAttempts)
                {
                    break;
                }

                decorObject = SpawnDecor(decorPrefab, spawnPosition);
            }

            if (decorObject != null)
            {

                decorObjects.Add(decorObject);
                spawnAttempts = 0;
            }
        }

    }

    private void GenerateDecorAreaWithoutPath()
    {
        int numDecorPrefabs = decorPrefabs.Length;
       
        GameObject decorObject = null;

        spawnAttempts = 0;
      
        while (decorObjects.Count < startedObjectsNb)
        {
            Vector3 randomPos = Random.insideUnitSphere * spawnDistance;
            Vector3 spawnPosition = player.position + randomPos;
            spawnPosition.y = 0f;
            spawnPosition.z = player.position.z + spawnDistance;

            GameObject decorPrefab = decorPrefabs[Random.Range(0, numDecorPrefabs)];

            // Incrémentation du compteur de tentatives
            spawnAttempts++;

            // Sort de la boucle si tentative max atteinte
            if (spawnAttempts >= maxSpawnAttempts)
            {
                break;
            }

            decorObject = SpawnDecor(decorPrefab, spawnPosition);
        }

        // L'objet a spawn sans collision, on réinitialise les tentatives
        if (decorObject != null)
        {
            decorObjects.Add(decorObject);
            spawnAttempts = 0;
        }
    }



    private GameObject SpawnDecor(GameObject decorPrefab, Vector3 position)
    {  
        GameObject newDecor = Instantiate(decorPrefab, position, Quaternion.identity);

        if (OverlapsWithPath(position, newDecor))
        {
            Destroy(newDecor);
            return null;
        }
        else
        {
          

            if (OverlapsWithOtherElements(newDecor))
            {
                Destroy(newDecor);
                return null;
            }

            return newDecor;
        }
    }


    private bool OverlapsWithPath(Vector3 position, GameObject decorPrefab)
    {
        //BoxCollider boxCollider = decorPrefab.GetComponent<BoxCollider>();


        //// Vérifier les collisions avec le chemin
        //Collider[] pathColliders = Physics.OverlapBox(decorPrefab.transform.position, boxCollider.size, decorPrefab.transform.rotation, pathMask);

        //if (pathColliders.Length > 0)
        //{
        //    return true;
        //}

        return false;
    }

    private bool OverlapsWithOtherElements(GameObject decorPrefab)
    {
        foreach (GameObject obj in decorObjects)
        {
            float distance = Vector3.Distance(decorPrefab.transform.position, obj.transform.position);

            if (distance < decorPrefab.GetComponent<Collider>().bounds.size.x)
            {
                return true;
            }
        }
        return false;
    }
}