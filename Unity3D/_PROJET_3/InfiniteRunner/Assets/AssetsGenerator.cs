using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class AssetsGenerator : MonoBehaviour
{

    [SerializeField] private GameObject[] decorElements;
    [SerializeField] private float decorNbWanted;
    [SerializeField] private GameObject floor;
    private List<GameObject> decorsOnPlay = new List<GameObject>();


  
    void Start()
    {
        Generate();
    }

    void Update()
    {

    }

    void Generate()
    {
        Vector3 floorHalfSize = floor.GetComponent<Collider>().bounds.extents;

        while (decorsOnPlay.Count < decorNbWanted)
        {
            GameObject decorPrefab = decorElements[Random.Range(0, decorElements.Length)];
          

            GameObject newItem = Instantiate(decorPrefab);

            newItem.transform.position = new Vector3(transform.position.x + Random.Range(-floorHalfSize.x, floorHalfSize.x), transform.position.y + 1, transform.position.z + Random.Range(-floorHalfSize.z, floorHalfSize.z));
           
            bool ok = true;

          Bounds newItemBound = newItem.GetComponentInChildren<Renderer>().bounds;
           // newItemBound.center = newItem.transform.TransformPoint(newItemBound.center);
            Debug.Log($"{newItem.name} center : {newItemBound.center}");

            foreach (var childWithRenderer in newItem.GetComponentsInChildren<Renderer>())
            {
                newItemBound.Encapsulate(childWithRenderer.bounds);
            }

            foreach (GameObject item in decorsOnPlay)
            {
                Bounds objBound = item.GetComponentInChildren<Renderer>().bounds; // A mettre dans un dictionnaire
              //   objBound.center = item.transform.TransformPoint(objBound.center);


                //  float combinedSize = newItem.GetComponentInChildren<Collider>().bounds.extents.x + item.GetComponentInChildren<Collider>().bounds.extents.x;

                float distance = Vector3.Distance(newItem.transform.position, item.transform.position);

                if (newItemBound.Intersects(objBound))
                {
                    ok = false;
                    break;
                }
            }
            if (ok)
            {
                ClampObject(newItem);

            }
            else
            {
                Debug.Log($"Destory Item : {newItem.name}");
                Destroy(newItem);
            }
        }
    }

    void ClampObject(GameObject obj)
    {
        Bounds objBound = obj.GetComponent<Collider>().bounds;
        Bounds floorBound = floor.GetComponent<Collider>().bounds;  

        if (obj.transform.position.x + objBound.extents.x > floor.transform.position.x + floorBound.extents.x ||
            obj.transform.position.x - objBound.extents.x < floor.transform.position.x - floorBound.extents.x ||
            obj.transform.position.z + objBound.extents.z > floor.transform.position.z + floorBound.extents.z ||
            obj.transform.position.z - objBound.extents.z < floor.transform.position.z - floorBound.extents.z) 
        {
            Destroy(obj);
        }
        else
        {
            decorsOnPlay.Add(obj);
        }
    }
}
