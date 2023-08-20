using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Syst�me de pool d'objets, utilis� pour le proc�dural : les obstacles, les d�cors, l'environnement...
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("Configuration de la pool")]
    [SerializeField] private int totalMaxPoolNumber;
    [SerializeField] private int poolBatchNumber;
   

    [Header("Contenu de la pool")]
    [SerializeField] private List<GameObject> objectsList = new List<GameObject>();

    // Propri�t�s priv�es
    private bool poolIsReady;
    private int batchSize = 2;
    private int currentTotalPool;
    private Dictionary<GameObject, List<GameObject>> pool;

    // Acesseurs
    public int PoolBatchNumber { get { return poolBatchNumber; } }
    public List<GameObject> ObjectList { get { return objectsList; } private set { objectsList = value; } }
    public bool PoolIsReady { get { return poolIsReady; } }


    public void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        currentTotalPool = 0;
        pool = new Dictionary<GameObject, List<GameObject>>();
        InitializePoolObjects();
        poolIsReady = true;
    }

    /// <summary>
    /// Initialise les objets de la pool en les instanciants et les marquants comme inactifs.
    /// </summary>
    void InitializePoolObjects()
    {
        for (int n = 0; n < objectsList.Count; n++)
        {
            List<GameObject> list = new List<GameObject>();

            // On instancie un nombre d'objet �quivalent � la configuration poolBatchNumber
            for (int i = 0; i < poolBatchNumber; i++)
            {
                GameObject instantiateObj = Instantiate(objectsList[n], transform);
                instantiateObj.SetActive(false);
                list.Add(instantiateObj);
                currentTotalPool++;
            }

            pool.Add(objectsList[n], list);
        }
    }

    /// <summary>
    /// M�thode permettant de r�cup�rer un objet dans la pool, soit en retournant un objet disponible, soit en �largissant la pool avant de le faire.
    /// </summary>
    /// <param name="wantedObject">Index de l'objet voulu dans la pool</param>
    /// <returns></returns>
    public GameObject GetPooledObject(int wantedObject)
    {
        foreach (GameObject obj in pool[objectsList[wantedObject]])
        {
            // Si un objet est disponible dans la pool, on l'active
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Sinon, on �tend la pool
        GameObject poolObj = ExpandPoolAndGetObject(wantedObject);
        return poolObj;
    }

    /// <summary>
    /// M�thode permettant d'�tendre la pool jusqu'� sa taille maximal. On �tend par paquet correspondant au batchSize d�fini
    /// </summary>
    /// <param name="wantedObject"></param>
    /// <returns></returns>
    GameObject ExpandPoolAndGetObject(int wantedObject)
    {
        if (currentTotalPool >= totalMaxPoolNumber)
        {
            Debug.LogWarning("Pool oversize !");
            return null;
        }
        else
        {
            for (int n = 0; n < objectsList.Count; n++)
            {
                for (int i = 0; i < batchSize; i++)
                {
                    GameObject newObj = Instantiate(objectsList[n], transform);
                    newObj.SetActive(false);
                    pool[objectsList[n]].Add(newObj);
                    currentTotalPool++;

                    if (currentTotalPool >= totalMaxPoolNumber)
                    {
                        Debug.LogWarning("Pool oversize !");
                        return null;
                    }
                }
            }
            return GetPooledObject(wantedObject);
        }
    }

    /// <summary>
    /// Remet un objet dans la pool, en le cachant et le rendant de nouveau disponible
    /// </summary>
    /// <param name="objToRelease"></param>
    public void ReleasedPooledObject(GameObject objToRelease)
    {
                objToRelease.SetActive(false);
                objToRelease.transform.position = transform.position;
                objToRelease.transform.rotation = transform.rotation;       
    }

    /// <summary>
    /// Remet un objet dans la pool apr�s avoir v�rifi� son index, en le cachant et le rendant de nouveau disponible
    /// </summary>
    /// <param name="objToRelease"></param>
    public void ReleasedPooledObject(int poolIndex, GameObject objToRelease)
    {
        if (poolIndex >= 0 && poolIndex < objectsList.Count)
        {
            if (pool[ObjectList[poolIndex]].Contains(objToRelease))
            {
                ReleasedPooledObject(objToRelease);
            }
            else
            {
                Debug.LogError("The object to release is not from the pool.");
            }
        }
        else
        {
            Debug.LogError("Invalid pool index.");
        }
    }


    public void Reset()
    {
        ResetPool();
        InitializePool();
    }

    void ResetPool() 
    {
        poolIsReady = false;
        pool.Clear();
    }

}
