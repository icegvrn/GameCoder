using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class PathGenerator : MonoBehaviour
{
    public Transform player;
    public List<GameObject> prefabOnPlay = new List<GameObject>();
    public int pathLength = 5;
    public float curveSegmentLength = 30f; // Length of each curved segment
    public float curveStrength = 10f; // Strength of the curve (higher values result in sharper curves)
    public float curveFrequency = 0.1f; // Lower values make curves less frequent


    [SerializeField] private int pathDespawnDistance;
    private bool pathGenerated = false;
    [SerializeField] private ObjectPool pathPool;


    void Update()
    {
        if (pathPool.PoolIsReady && !pathGenerated)
        {
            for (int i = 0; i < pathLength; i++)
            {
                SpawnPath();
                pathGenerated = true;
            }
        }

        // Get the position of the player.
        float playerZ = player.position.z;

        // Loop through each prefab in prefabOnPlay.
        for (int i = 0; i < prefabOnPlay.Count; i++)
        {
            GameObject prefab = prefabOnPlay[i];

            // Check if the prefab is behind the player (use a buffer of 5 units).
            if (prefab.transform.position.z + CalculateCombinedBounds(prefab.transform).extents.z + pathDespawnDistance < playerZ)
            {
                RepositionPath(prefab);

                // Move the index back by one to avoid skipping a prefab.
                i--;
            }
        }


    }

    void SpawnPath()
    {
        // Instantiate a new prefab.
        GameObject newPrefab = pathPool.GetPooledObject(0);

        // Position the new prefab based on the previous prefab or the player's position if it's the first one.
        if (prefabOnPlay.Count == 0)
        {
            newPrefab.transform.position = player.position;
        }
        else
        {
                // Position the new prefab straight ahead if no curve is added.
                Vector3 newPosition = prefabOnPlay[prefabOnPlay.Count - 1].transform.position;
                newPosition.z += CalculateCombinedBounds(newPrefab.transform).size.z;
                newPrefab.transform.position = newPosition;
        }

        // Add the new prefab to the list.
        prefabOnPlay.Add(newPrefab);
      
     
    }

    void RepositionPath(GameObject prefab)
    {
        // Move the prefab to the end of the path.
        Vector3 newPosition = prefabOnPlay[prefabOnPlay.Count - 1].transform.position;
        newPosition.z += CalculateCombinedBounds(prefab.transform).size.z;
        prefab.transform.position = newPosition;

        PoolAssetsGenerator[] generators = prefab.GetComponentsInChildren<PoolAssetsGenerator>();
       

        if (generators.Length > 0)
        {
            foreach (PoolAssetsGenerator generator in generators)
            {
                generator.Reset();
            
            }
        }

        ObstacleSpawner[] obstacleGenerators = prefab.GetComponentsInChildren<ObstacleSpawner>();

        if (obstacleGenerators.Length > 0)
        {
            foreach (ObstacleSpawner obstacleGenerator in obstacleGenerators)
            {
                obstacleGenerator.Reset();

            }
        }

        prefabOnPlay.Remove(prefab);
        prefabOnPlay.Add(prefab);
    }


    Bounds CalculateCombinedBounds(Transform objTransform)
    {
        Renderer[] renderers = objTransform.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                if (renderers[i].tag == "Path")
                {
                    combinedBounds.Encapsulate(renderers[i].bounds);
                }
                
            }

            return combinedBounds;
        }

        return new Bounds(objTransform.position, Vector3.zero);
    }
}