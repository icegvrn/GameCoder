using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public enum ObjectDirection
{
    Face,
    Left,
    Right
}


public class PoolAssetsGenerator : MonoBehaviour
{

    [SerializeField] private GameObject[] decorElements;
    [SerializeField] private float decorNbWanted;
    [SerializeField] private GameObject floor;
    [SerializeField] private List<GameObject> decorsOnPlay = new List<GameObject>();
    [SerializeField] private ObjectDirection objDirection = new ObjectDirection();
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
        Vector3 floorHalfSize = floor.GetComponent<Renderer>().bounds.extents;

        while (decorsOnPlay.Count < decorNbWanted)
        {
           
            int wantedObjectIndex = Random.Range(0, ObjectPool.SharedInstance.ObjectList.Count);
            GameObject newItem = ObjectPool.SharedInstance.GetPooledObject(wantedObjectIndex);

            if (newItem != null)
            {
                newItem.transform.position = new Vector3(transform.position.x + Random.Range(-floorHalfSize.x, floorHalfSize.x), transform.position.y, transform.position.z + Random.Range(-floorHalfSize.z, floorHalfSize.z));

               ChangeDirection(newItem);

                bool ok = true;

                Bounds newItemBound = CalculateCombinedBounds(newItem.transform);

                Debug.Log($"{newItem.name} center : {newItemBound.center}");

                foreach (Renderer childWithRenderer in newItem.GetComponentsInChildren<Renderer>())
                {
                    newItemBound.Encapsulate(childWithRenderer.bounds);
                }

                foreach (GameObject item in decorsOnPlay)
                {
                    Bounds objBound = CalculateCombinedBounds(item.transform); // A mettre dans un dictionnaire

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
        Bounds objBound = CalculateCombinedBounds(obj.transform);
        Bounds floorBound = floor.GetComponent<Renderer>().bounds;

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

    void ChangeDirection(GameObject newItem)
    {
        if (objDirection == ObjectDirection.Right)
        {
            newItem.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
        else if (objDirection == ObjectDirection.Left)
        {
            newItem.transform.rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
        }

        int RandomiseDirection = Random.Range(0, 6);

        if (RandomiseDirection >= 5)
        {
            newItem.transform.rotation = Quaternion.LookRotation(-Vector3.left, Vector3.up);
        }

    }

    Bounds CalculateCombinedBounds(Transform objTransform)
    {
        Renderer[] renderers = objTransform.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }

            return combinedBounds;
        }

        return new Bounds(objTransform.position, Vector3.zero);
    }
}
