using UnityEngine;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject[] prefabPaths;
    private List<GameObject> prefabOnPlay = new List<GameObject>();
    public int pathLength = 200;
    public float curveSegmentLength = 30f; // Length of each curved segment
    public float curveStrength = 10f; // Strength of the curve (higher values result in sharper curves)
    public float curveFrequency = 0.1f; // Lower values make curves less frequent
    public DecorGenerator decor;
    void Start()
    {
    
    }

    public List<Bounds> GetPathBounds()
    {
        List<Bounds> boundsList = new List<Bounds>();

        foreach (GameObject prefab  in prefabOnPlay)
        {
            BoxCollider boxCollider = prefab.GetComponent<BoxCollider>();
            boundsList.Add(boxCollider.bounds);
        }
        return boundsList;
    }

    void Update()
    {
    
    }

    public void ProceduralStart()
    {
        for (int i = 0; i < pathLength; i++)
        {
            SpawnPath();
        }

        // Déplacez la génération des objets de décor ici, après que le chemin ait été généré.
    
    }

    public void ProceduralUpdate()
    {
        // Get the position of the player.
        float playerZ = player.position.z;

        // Loop through each prefab in prefabOnPlay.
        for (int i = 0; i < prefabOnPlay.Count; i++)
        {
            GameObject prefab = prefabOnPlay[i];
            float prefabZ = prefab.transform.position.z;

            // Check if the prefab is behind the player (use a buffer of 5 units).
            if (prefabZ + curveSegmentLength < playerZ)
            {
                RepositionPath(prefab);

                // Move the index back by one to avoid skipping a prefab.
                i--;
            }
        }
        if (!decor.isInit)
        {
            decor.Init();
        }

        decor.ProceduralUpdate();
    }

    void SpawnPath()
    {
        // Instantiate a new prefab.
        GameObject newPrefab = Instantiate(prefabPaths[0]);

        // Position the new prefab based on the previous prefab or the player's position if it's the first one.
        if (prefabOnPlay.Count == 0)
        {
            newPrefab.transform.position = player.position;
        }
        else
        {
            // Check if a curve should be added.
            if (Random.value < curveFrequency)
            {
                // Calculate a random angle for the curve.
                float curveAngle = Random.Range(-curveStrength, curveStrength);

                // Calculate the control points for the Bezier curve.
                Vector3 lastPosition = prefabOnPlay[prefabOnPlay.Count - 1].transform.position;
                Vector3 controlPoint1 = lastPosition + Vector3.forward * curveSegmentLength / 2f;
                Vector3 controlPoint2 = lastPosition + Quaternion.Euler(0f, curveAngle, 0f) * Vector3.forward * curveSegmentLength / 2f;

                // Calculate the end position of the new prefab using the control points.
                Vector3 endPosition = lastPosition + Quaternion.Euler(0f, curveAngle, 0f) * Vector3.forward * curveSegmentLength;

                // Position the new prefab.
                newPrefab.transform.position = endPosition;

                // Rotate the new prefab to follow the curve.
                newPrefab.transform.LookAt(endPosition + (endPosition - lastPosition), Vector3.up);
            }
            else
            {
                // Position the new prefab straight ahead if no curve is added.
                newPrefab.transform.position = prefabOnPlay[prefabOnPlay.Count - 1].transform.position + Vector3.forward * curveSegmentLength;
            }
        }

        // Add the new prefab to the list.
        prefabOnPlay.Add(newPrefab);
    }

    void RepositionPath(GameObject prefab)
    {
        // Move the prefab to the end of the path.
        prefab.transform.position = prefabOnPlay[prefabOnPlay.Count - 1].transform.position + Vector3.forward * curveSegmentLength;

        // Remove the first prefab from the list and reposition it.
        prefabOnPlay.Remove(prefab);
        prefabOnPlay.Add(prefab);
    }

    public List<Vector3> GetPathPositions()
    {
        List<Vector3> pathPositions = new List<Vector3>();

        foreach (GameObject prefab in prefabOnPlay)
        {
            pathPositions.Add(prefab.transform.position);
        }

        return pathPositions;
    }
}