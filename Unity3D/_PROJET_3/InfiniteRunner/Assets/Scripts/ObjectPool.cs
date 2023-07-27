using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    [SerializeField] private List<GameObject> objectsList = new List<GameObject>();
    public List<GameObject> ObjectList { get { return objectsList; } private set { objectsList = value; } }

    [SerializeField] private int poolBatchNumber;
    [SerializeField] private int batchSize;
    [SerializeField] private int totalMaxPoolNumber;
  
    public int PoolNumber { get { return poolBatchNumber; } }

    [SerializeField] private Dictionary<GameObject, List<GameObject>> pool;
    [SerializeField] private int currentTotalPool;

    public bool PoolIsRead = false;
    void Awake()
    {
        SharedInstance = this;
    }

    public void Start()
    {

        pool = new Dictionary<GameObject, List<GameObject>>();
        currentTotalPool = 0;

        for (int n = 0; n < objectsList.Count; n++)
        {
            List<GameObject> list = new List<GameObject>();

            for (int i = 0; i < poolBatchNumber; i++)
            {
                GameObject instantiateObj = Instantiate(objectsList[n]);
                objectsList[n].SetActive(false);
                currentTotalPool++;
                list.Add(instantiateObj);
            }
            pool.Add(objectsList[n], list);
        }

        PoolIsRead = true;
    }

    public GameObject GetPooledObject(int wantedObject)
    {
        foreach (GameObject obj in pool[objectsList[wantedObject]])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if (currentTotalPool >= totalMaxPoolNumber)
        {
            Debug.LogError("Pool oversize !");
            return null;
        }
        else
        {
            for (int n = 0; n < objectsList.Count; n++)
            {
                for (int i = 0; i < batchSize; i++)
                {
                    GameObject newObj = Instantiate(objectsList[n]);
                    objectsList[n].SetActive(false);
                    pool[objectsList[n]].Add(newObj);
                    currentTotalPool++;

                    if (currentTotalPool >= totalMaxPoolNumber)
                    {
                        Debug.LogError("Pool oversize !");
                        return null;
                    }
                }
            }
        }

        return GetPooledObject(wantedObject);
    }

    public void ReleasedPooledObject(int poolIndex, GameObject objToRelease)
    {
        if (poolIndex >= 0 && poolIndex < objectsList.Count)
        {
            if (pool[ObjectList[poolIndex]].Contains(objToRelease))
            {
                objToRelease.SetActive(false);
                objToRelease.transform.position = transform.position;
                objToRelease.transform.rotation = transform.rotation;
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

}
