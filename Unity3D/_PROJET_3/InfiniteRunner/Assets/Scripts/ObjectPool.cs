using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> ObjectList { get { return objectsList; } private set { objectsList = value; } }
    [SerializeField] private int poolBatchNumber;
    [SerializeField] private int totalMaxPoolNumber;
    [SerializeField] private List<GameObject> objectsList = new List<GameObject>();


    public int PoolNumber { get { return poolBatchNumber; } }
    private int batchSize = 2;
    private Dictionary<GameObject, List<GameObject>> pool;
    private int currentTotalPool;
    private bool poolIsReady;
    public bool PoolIsReady { get { return poolIsReady; } }

    public void Start()
    {

        pool = new Dictionary<GameObject, List<GameObject>>();
        currentTotalPool = 0;

        for (int n = 0; n < objectsList.Count; n++)
        {
            List<GameObject> list = new List<GameObject>();

            for (int i = 0; i < poolBatchNumber; i++)
            {
                GameObject instantiateObj = Instantiate(objectsList[n], transform);
                instantiateObj.SetActive(false);
                currentTotalPool++;
                list.Add(instantiateObj);
            }
            pool.Add(objectsList[n], list);
        }

        poolIsReady = true;
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
                    GameObject newObj = Instantiate(objectsList[n], transform);
                    newObj.SetActive(false);
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

    public void ReleasedPooledObject(GameObject objToRelease)
    {
                objToRelease.SetActive(false);
                objToRelease.transform.position = transform.position;
                objToRelease.transform.rotation = transform.rotation;       
    }

    public void Reset()
    {
        poolIsReady = false;
        pool.Clear();

        currentTotalPool = 0;

        for (int n = 0; n < objectsList.Count; n++)
        {
            List<GameObject> list = new List<GameObject>();

            for (int i = 0; i < poolBatchNumber; i++)
            {
                GameObject instantiateObj = Instantiate(objectsList[n], transform);
                instantiateObj.SetActive(false);
                currentTotalPool++;
                list.Add(instantiateObj);
            }
            pool.Add(objectsList[n], list);
        }

        poolIsReady = true;

    }

}
