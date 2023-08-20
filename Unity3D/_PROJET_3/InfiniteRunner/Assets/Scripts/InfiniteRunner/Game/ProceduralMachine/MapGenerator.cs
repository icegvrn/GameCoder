using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// MapGenerator est le coeur du d�cor proc�dural : il instancie un chunk d'environnement qui contient quant � lui de la g�n�ration d'obstacle ou d'objet
/// </summary>
public class MapGenerator : MonoBehaviour
{
    [Header("Cible de r�f�rence pour apparition")]
    [SerializeField] private Transform player;

    [Header("Pools de g�n�ration")]
    [SerializeField] private ObjectPool runPathPool;
    [SerializeField] private ObjectPool obstaclePool;
    [SerializeField] private ObjectPool assetPool;

    [Header("Configuration du run path")]
    [SerializeField] private int runPathLenght = 5;
    [SerializeField] private int runPathDespawnDistance;

    [SerializeField] private List<GameObject> prefabOnPlay = new List<GameObject>();
    public List<GameObject> PrefabOnPlay { get { return prefabOnPlay; } }

    private bool pathGenerated = false;
  
  
    void Update()
    {
        // Premi�re initialisation au d�marrage
        if (runPathPool.PoolIsReady && !pathGenerated){ InitPath(); }
        MoveChunkWhenItsBackUser();
    }

    void InitPath()
    {
        for (int i = 0; i < runPathLenght; i++)
        {
            SpawnPath();
            pathGenerated = true;
        }
    }

    /// <summary>
    /// M�thode qui appelle une m�thode de repositionnement du chunk d'environnement si n�cessaire. Pour chaque prefab en jeu, on le repositionne si le joueur l'a d�pass� et que la distance entre eux est plus grande que le runPathDespawnDistance.
    /// </summary>
    void MoveChunkWhenItsBackUser()
    {
        float playerZ = player.position.z;

        for (int i = 0; i < PrefabOnPlay.Count; i++)
        {
            if (PrefabOnPlay[i].transform.position.z + CalculateCombinedBounds(PrefabOnPlay[i].transform).extents.z + runPathDespawnDistance < playerZ)
            {
                RepositionPath(PrefabOnPlay[i]);
                i--; // Permet de rester sur le m�me index apr�s avoir retir� le prefab en cours
            }
        }
    }

    /// <summary>
    /// M�thode repositionnant un chunk environnement pass� au bout du parcours pour qu'il soit de nouveau utilis�
    /// </summary>
    /// <param name="prefab"></param>
    void RepositionPath(GameObject prefab)
    {
        prefab.transform.position = CalcNewPosition(prefab);
       
        ResetAssets(prefab);
        ResetObstacles(prefab);
        MovePathOnTheTopOfThePrefabList(prefab);
    }

    /// <summary>
    /// Calcul la nouvelle position que doit avoir le prefab pour se positionner au bout du chemin en prenant la position du dernier connu et en y ajoutant la taille Z de son bound
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    Vector3 CalcNewPosition(GameObject prefab)
    {
        Vector3 newPosition = PrefabOnPlay[PrefabOnPlay.Count - 1].transform.position;
        newPosition.z += CalculateCombinedBounds(prefab.transform).size.z;
        prefab.transform.position = newPosition;
        return newPosition;
    }

    /// <summary>
    /// Calcul le bound global du prefab en prenant en compte ses enfants ayant un tag path
    /// </summary>
    /// <param name="objTransform"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Appel le reset de chaque AssetsGenerator en enfant pour les rendre r�utilisable par la pool
    /// </summary>
    /// <param name="prefab"></param>
    void ResetAssets(GameObject prefab)
    {
        AssetsGeneratorByPool[] generators = prefab.GetComponentsInChildren<AssetsGeneratorByPool>();
        if (generators.Length > 0)
        {
            foreach (AssetsGeneratorByPool generator in generators)
            {
                generator.Reset();
            }
        }
    }

    /// <summary>
    /// Appel le reset de chaque ObstacleSpawner en enfant pour les rendre r�utilisable par la pool
    /// </summary>
    /// <param name="prefab"></param>
    void ResetObstacles(GameObject prefab) 
    {
        ObstacleSpawner[] obstacleGenerators = prefab.GetComponentsInChildren<ObstacleSpawner>();

        if (obstacleGenerators.Length > 0)
        {
            foreach (ObstacleSpawner obstacleGenerator in obstacleGenerators)
            {
                obstacleGenerator.Reset();
            }
        }
    }

    /// <summary>
    /// Repositionne le prefab en haut de la liste pour que cela corresponde toujours au bout du chemin connu
    /// </summary>
    /// <param name="prefab"></param>
    void MovePathOnTheTopOfThePrefabList(GameObject prefab)
    {
        PrefabOnPlay.Remove(prefab);
        PrefabOnPlay.Add(prefab);
    }

    /// <summary>
    /// M�thode faisant spawn un chunk
    /// </summary>
    void SpawnPath()
    {
        GameObject newPrefab = runPathPool.GetPooledObject(0); // Attention : uniquement parce qu'on a qu'un seul chunk environnement actuellement ; � revoir au besoin

        // Si c'est le premier, on fait spawn sous les pieds du joueur
        if (PrefabOnPlay.Count == 0)
        {
            newPrefab.transform.position = new Vector3(0, 0, player.position.z);
        }
        // Sinon, on le fait spawn au bout du parcours
        else
        {
                newPrefab.transform.position = CalcNewPosition(newPrefab);
        }

        // Ajout du chunk � la liste des prefabs en jeu
        PrefabOnPlay.Add(newPrefab);
     
    }

   
    /// <summary>
    /// M�thode qui reset l'int�gralit� des chunk
    /// </summary>
    public void Reset()
    {
        foreach (GameObject prefab in PrefabOnPlay)
        {
            Destroy(prefab);
        }

        PrefabOnPlay.Clear();
        assetPool.Reset();
        runPathPool.Reset();
        obstaclePool.Reset();

        pathGenerated = false;
    }

}