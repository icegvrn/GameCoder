using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PoolAssetsGenerator : MonoBehaviour
{

    [SerializeField] private GameObject[] decorElements;
    [SerializeField] private float decorNbWanted;
    [SerializeField] private GameObject floor;
    [SerializeField]  private List<GameObject> decorsOnPlay = new List<GameObject>();
   

  
    void Start()
    {
      
      
    }

    void Update()
    {
        if (ObjectPool.SharedInstance.PoolIsRead)
        {
            Generate();
        }
    }

    void Generate()
    {
        Vector3 floorHalfSize = floor.GetComponent<Collider>().bounds.extents;

        while (decorsOnPlay.Count < decorNbWanted)
        {
           
            int wantedObjectIndex = Random.Range(0, ObjectPool.SharedInstance.ObjectList.Count);
            GameObject newItem = ObjectPool.SharedInstance.GetPooledObject(wantedObjectIndex);

            if (newItem != null)
            {
                newItem.transform.position = new Vector3(transform.position.x + Random.Range(-floorHalfSize.x, floorHalfSize.x), transform.position.y + 1, transform.position.z + Random.Range(-floorHalfSize.z, floorHalfSize.z));

                bool ok = true;

                Bounds newItemBound = newItem.GetComponentInChildren<Renderer>().bounds;

                Debug.Log($"{newItem.name} center : {newItemBound.center}");

                foreach (var childWithRenderer in newItem.GetComponentsInChildren<Renderer>())
                {
                    newItemBound.Encapsulate(childWithRenderer.bounds);
                }

                foreach (GameObject item in decorsOnPlay)
                {
                    Bounds objBound = item.GetComponentInChildren<Renderer>().bounds; // A mettre dans un dictionnaire


                    float distance = Vector3.Distance(newItem.transform.position, item.transform.position);

                    if (newItemBound.Intersects(objBound))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    ClampObject(wantedObjectIndex, newItem);
                }
                else
                {
                    Debug.Log($"Destory Item : {newItem.name}");
                    ObjectPool.SharedInstance.ReleasedPooledObject(wantedObjectIndex, newItem);
                }
            }
            else
            {
                Debug.LogError("Impossible de récupérer un objet supplémentaire, vérifier la taille de la pool.");
                break;
            }
            
        }
    }


    void ClampObject(int index, GameObject obj)
    {
        Bounds objBound = obj.GetComponent<Collider>().bounds;
        Bounds floorBound = floor.GetComponent<Collider>().bounds;

        if (obj.transform.position.x + objBound.extents.x > floor.transform.position.x + floorBound.extents.x ||
            obj.transform.position.x - objBound.extents.x < floor.transform.position.x - floorBound.extents.x ||
            obj.transform.position.z + objBound.extents.z > floor.transform.position.z + floorBound.extents.z ||
            obj.transform.position.z - objBound.extents.z < floor.transform.position.z - floorBound.extents.z)
        {
            ObjectPool.SharedInstance.ReleasedPooledObject(index, obj);
        }
        else
        {
            decorsOnPlay.Add(obj);
        }
    }
}
