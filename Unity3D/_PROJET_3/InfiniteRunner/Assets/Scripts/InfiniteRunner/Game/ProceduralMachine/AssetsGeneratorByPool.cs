using System.Collections.Generic;
using UnityEngine;

public enum ObjectDirection
{
    Face,
    Left,
    Right
}

/// <summary>
/// Permet de générer des objets aléatoirement sur une surface plane sans qu'ils se rentrent dedans et avec une orientation aléatoire.
/// </summary>
public class AssetsGeneratorByPool : MonoBehaviour
{
    [Header("Configuration espace de génération")]
    [SerializeField] private float nbAssetsWanted;
    [SerializeField] private ObjectDirection defaultAssetsDirection;
    [SerializeField] private GameObject spawnSurface;

    // Stock la pool d'objets qu'il peut faire spawn, récupéré par un script AttachOjectsToEnvironnement.cs
    private ObjectPool objectPool;
    public ObjectPool ObjectPool { get { return objectPool; } set { objectPool = value; } }
      
    // Liste de tous les objets actuellement en jeu
    private List<GameObject> assetsOnPlay = new List<GameObject>();
    public List<GameObject> AssetsOnPlay { get { return assetsOnPlay; } set {  assetsOnPlay = value; } }

    

    void Update()
    {
        // Génération des objets en continue dès que la pool est prête
        if (objectPool.PoolIsReady)
        {
            Generate();
        }
    }

    /// <summary>
    ///  Méthode qui apelle la génération d'assets tant qu'on en a pas le bon nombre
    /// </summary>
    void Generate()
    {
        while (AssetsOnPlay.Count < nbAssetsWanted)
        {
            bool generation = TryGenerateAsset();   
            if (!generation) { break; }
        }
    }

    /// <summary>
    /// Méthode qui tente la génération d'assets en vérifiant les conditions de collision
    /// </summary>
    /// <returns></returns>
    bool TryGenerateAsset()
    {
        // Instancie un asset au hasard de la liste d'assets
        int wantedObjectIndex = Random.Range(0, objectPool.ObjectList.Count);
        GameObject newItem = objectPool.GetPooledObject(wantedObjectIndex);

        if (newItem != null)
        {
            PositionAsset(newItem);
            ChangeDirection(newItem);

            // Si l'asset ne collide pas avec un autre et est entièrement sur la spawnSurface on le garde en jeu
            if (IsAssetCollideWithAnOther(newItem) && IsAssetAllOnTheSurface(newItem))
            {
                AssetsOnPlay.Add(newItem);
            }

            else
            {
                // Sinon il retourne dans la pool
                objectPool.ReleasedPooledObject(wantedObjectIndex, newItem);
            }
            return true;
        }

        else
        {
            Debug.LogWarning("Impossible de récupérer un objet supplémentaire, vérifier la taille de la pool.");
            return false;
        }
    }

    /// <summary>
    /// Retourne un booléan correspondant à la collision ou non de l'asset avec un autre
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    bool IsAssetCollideWithAnOther(GameObject newItem)
    {
        bool assetIsOk = true;

        Bounds newItemBound = CalculateCombinedBounds(newItem.transform);

        foreach (GameObject item in AssetsOnPlay)
        {
            Bounds objBound = CalculateCombinedBounds(item.transform);

            float distance = Vector3.Distance(newItem.transform.position, item.transform.position);

            if (newItemBound.Intersects(objBound))
            {
                assetIsOk = false;
                break;
            }
        }
        return assetIsOk;
    }

    /// <summary>
    /// Méthode simple de positionnement d'un objet de façon aléatoire sur une surface spawnSurface
    /// </summary>
    /// <param name="newItem"></param>
    void PositionAsset(GameObject newItem)
    {
        Vector3 floorHalfSize = spawnSurface.GetComponent<Renderer>().bounds.extents;
        newItem.transform.position = new Vector3(transform.position.x + Random.Range(-floorHalfSize.x, floorHalfSize.x), transform.position.y, transform.position.z + Random.Range(-floorHalfSize.z, floorHalfSize.z));
    }


    /// <summary>
    /// Méthode permettant de vérifier si l'objet dépasse ou non de la surface.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="obj"></param>
    bool IsAssetAllOnTheSurface(GameObject obj)
    {
        Bounds objBound = CalculateCombinedBounds(obj.transform);
        Bounds floorBound = spawnSurface.GetComponent<Renderer>().bounds;

        if (obj.transform.position.x + objBound.extents.x > spawnSurface.transform.position.x + floorBound.extents.x ||
            obj.transform.position.x - objBound.extents.x < spawnSurface.transform.position.x - floorBound.extents.x ||
            obj.transform.position.z + objBound.extents.z > spawnSurface.transform.position.z + floorBound.extents.z ||
            obj.transform.position.z - objBound.extents.z < spawnSurface.transform.position.z - floorBound.extents.z)
        {
            return false; // S'il dépasse de la surface return false
        }
        else
        {
            return true; // Sinon retourne true
        }
    }

    /// <summary>
    /// Modifie la rotation de l'objet en fonction de l'orientation par défaut, puis ajoute un random pour le faire varier de 90° et avoir de temps en temps des objets différents
    /// </summary>
    /// <param name="newItem"></param>
    void ChangeDirection(GameObject newItem)
    {
        if (defaultAssetsDirection == ObjectDirection.Right)
        {
            newItem.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
        else if (defaultAssetsDirection == ObjectDirection.Left)
        {
            newItem.transform.rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
        }

        int RandomiseDirection = Random.Range(0, 6);

        if (RandomiseDirection >= 5)
        {
            newItem.transform.rotation = Quaternion.LookRotation(-Vector3.left, Vector3.up);
        }

    }

    /// <summary>
    /// Méthode permettant de prendre en compte les bounds des objets enfants pour prendre en compte tout l'asset dans les calculs
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
                combinedBounds.Encapsulate(renderers[i].bounds); // Méthode Unity qui "additionne" les bounds
            }

            return combinedBounds;
        }

        return new Bounds(objTransform.position, Vector3.zero);
    }

    /// <summary>
    /// Méthode qui nettoie tout : libère tous les assets et nettoie la liste des assets en jeu
    /// </summary>
    public void Reset()
    {
        foreach (GameObject obj in AssetsOnPlay)
        {
            objectPool.ReleasedPooledObject(obj);
          
        }
        AssetsOnPlay.Clear();
    }
}
