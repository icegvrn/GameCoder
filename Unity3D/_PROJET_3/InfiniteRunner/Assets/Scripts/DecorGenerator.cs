using UnityEngine;
using System.Collections.Generic;
using System.Drawing;

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

    // Layer sur lequel se trouve les objets de décor qui ne doivent pas collisionner entre eux
    public LayerMask collisionMask;

    // Layer sur lequel se trouve le chemin tracé, pour ne pas faire pop des décors dessus
    public LayerMask pathMask;

    // Reference au PathGenerator (voir comment l'obtenir différemment)
    public PathGenerator pathGenerator;

    // Liste des objets actuellement en cours dans la scène
    private List<GameObject> decorObjects = new List<GameObject>();

    public bool isInit = false;

    private void Start()
    {

    }

    /// <summary>
    /// Initialise la possition de départ du joueur et génère des premiers décors.
    /// </summary>
    public void Init()
    {
        GenerateInitialDecorWithoutPath();
        isInit = true;
    }

    private void Update()
    {
      //  GenerateDecors();
    }

    public void ProceduralUpdate()
    {
        GenerateDecors();
    }

        /// <summary>
        /// Génère des décors devant le joueur dès que le joueur passe devant un décor
        /// </summary>
        private void GenerateDecors()
{

    Vector3 playerPos = player.position;
    int n = 0;
    for (int i = decorObjects.Count - 1; i >= 0; i--)
    {
        GameObject obj = decorObjects[i];
        float distanceToObject = Vector3.Distance(playerPos, obj.transform.position);

      //  Debug.Log(obj.name + " est à " + distanceToObject + " par rapport à une distance " + despawnDistance + " et sur les z on me dit que l'objet est à " + obj.transform.position.z + " comparé pour être plus petit que player est à " + playerPos.z);

        if (distanceToObject >= despawnDistance && obj.transform.position.z < playerPos.z)
        {
            Destroy(obj);
            decorObjects.RemoveAt(i);
            n++;
        }
    }
    
 
    for (int j = 0; j < n; j++)
    {
        GenerateDecorAreaWithoutPath(playerPos);
    }
}

    private void GenerateInitialDecorWithoutPath()
    {
        int numDecorPrefabs = decorPrefabs.Length;

        for (int i = 0; i < 50; i++) 
        {
            Vector3 randomPos = Random.insideUnitSphere * spawnDistance;
            randomPos.y = 0f;
            Vector3 spawnPosition = player.transform.position + randomPos;


            if (Vector3.Distance(spawnPosition, player.transform.position) >= minDistanceToPlayer)
            {
                GameObject decorPrefab = decorPrefabs[Random.Range(0, numDecorPrefabs)];
                GameObject decorObject = null;

                while (true)
                {
                    decorObject = SpawnDecor(decorPrefab, spawnPosition);

                    if (decorObject == null)
                    {
                        // L'objet entre en collision avec quelque chose, essayer une autre position
                        randomPos = Random.insideUnitSphere * spawnDistance;
                        randomPos.y = 0f;
                        spawnPosition = player.transform.position + randomPos;
                    }
                    else
                    {
                        // L'objet a été généré avec succès, sortir de la boucle
                        break;
                    }
                }

                if (decorObject != null)
                {

                    decorObjects.Add(decorObject);
                    // Calculate the direction vector from the path to the spawned object
                    Vector3 lookDirection = player.transform.position - spawnPosition;
                    lookDirection.y = 0f;

                    // Calculate the angle between the lookDirection and player's forward direction
                    float angle = Vector3.SignedAngle(lookDirection.normalized, player.forward, Vector3.up);

                    // Determine the rotation based on the angle
                    Quaternion rotation;
                    if (angle > 0f)
                    {
                        // If the angle is positive, the object is to the left of the path and should face right
                        rotation = Quaternion.LookRotation(Vector3.left);
                    }
                    else
                    {
                        // If the angle is non-positive, the object is to the right of the path and should face left
                        rotation = Quaternion.LookRotation(Vector3.right);
                    }

                    // Randomly choose the rotation for some objects
                    if (Random.value < 0.5f)
                    {
                        // 50% chance to face the player
                        rotation = Quaternion.LookRotation(lookDirection);
                    }

                    // Apply the rotation to the spawned object
                    decorObject.transform.rotation = rotation;
                }


            }
        }
    }

    private void GenerateDecorAreaWithoutPath(Vector3 center)
    {
        int numDecorPrefabs = decorPrefabs.Length;
        Vector3 randomPos = Random.insideUnitSphere * spawnDistance;
        randomPos.y = 0f;
        Vector3 spawnPosition = center + randomPos;
        spawnPosition.z = player.position.z + spawnDistance;

        GameObject decorPrefab = decorPrefabs[Random.Range(0, numDecorPrefabs)];
        GameObject decorObject = null;

        // Essayer de générer l'objet sans limite d'essais
        while (true)
        {
            decorObject = SpawnDecor(decorPrefab, spawnPosition);

            if (decorObject == null)
            {
                // L'objet entre en collision avec quelque chose, essayer une autre position
                randomPos = Random.insideUnitSphere * spawnDistance;
                randomPos.y = 0f;
                spawnPosition = center + randomPos;
                spawnPosition.z = player.position.z + spawnDistance;
            }
            else
            {
                // L'objet a été généré avec succès, sortir de la boucle
                break;
            }
        }

        if (decorObject != null)
        {
            decorObjects.Add(decorObject);
            // Calculate the direction vector from the path to the spawned object
            Vector3 lookDirection = center - spawnPosition;
            lookDirection.y = 0f;

            // Calculate the angle between the lookDirection and player's forward direction
            float angle = Vector3.SignedAngle(lookDirection.normalized, player.forward, Vector3.up);

            // Determine the rotation based on the angle
            Quaternion rotation;
            if (angle > 0f)
            {
                // If the angle is positive, the object is to the left of the path and should face right
                rotation = Quaternion.LookRotation(Vector3.left);
            }
            else
            {
                // If the angle is non-positive, the object is to the right of the path and should face left
                rotation = Quaternion.LookRotation(Vector3.right);
            }

            // Randomly choose the rotation for some objects
            if (Random.value < 0.5f)
            {
                // 50% chance to face the player
                rotation = Quaternion.LookRotation(lookDirection);
            }

            // Apply the rotation to the spawned object
            decorObject.transform.rotation = rotation;
        }
    }


  
    private GameObject SpawnDecor(GameObject decorPrefab, Vector3 position)
    {
        if (OverlapsWithPath(position, decorPrefab))
        {
            Debug.Log("OVERLAP");
            //Si l'objet entre en collision avec le chemin, ne pas l'instancier et retourner null
            return null;
        }
        else
        {
            if (OverlapsWithOtherElements(position, decorPrefab))
            {
                Debug.Log("OVERLAP");
                //Si l'objet entre en collision avec le chemin, ne pas l'instancier et retourner null
                return null;
            }
            else
            {
                //float scaleMultiplier = Random.Range(0.8f, 1.2f);
                //Vector3 scale = decorPrefab.transform.localScale * scaleMultiplier;
                //position.y = 0f;
                GameObject newDecor = Instantiate(decorPrefab, position, Quaternion.identity);
                //newDecor.transform.localScale = scale;
                return newDecor;
            }
          
        }
    }


    private bool OverlapsWithPath(Vector3 position, GameObject decorPrefab)
    {
        BoxCollider boxCollider = decorPrefab.GetComponent<BoxCollider>();


        // Vérifier les collisions avec le chemin
        Collider[] pathColliders = Physics.OverlapBox(position + boxCollider.center, boxCollider.size, Quaternion.identity, pathMask);

        if (pathColliders.Length > 0)
        {
            Debug.Log("Collision detected between object " + decorPrefab.name + " and the path.");
            return true;
        }

        return false;
    }

    private bool OverlapsWithOtherElements(Vector3 position, GameObject decorPrefab)
    {
        BoxCollider boxCollider = decorPrefab.GetComponent<BoxCollider>();


        // Vérifier les collisions avec le chemin
        Collider[] pathColliders = Physics.OverlapBox(position + boxCollider.center, boxCollider.size, Quaternion.identity, collisionMask);

        if (pathColliders.Length > 0)
        {
            Debug.Log("Collision detected between object " + decorPrefab.name + " and the path.");
            return true;
        }

        return false;
    }








}