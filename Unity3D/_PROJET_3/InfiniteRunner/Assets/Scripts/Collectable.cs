using UnityEngine;


public class Collectable : MonoBehaviour
{
    [SerializeField] private int pointsValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

        if (other.gameObject.TryGetComponent(out PointCollector pCollector))
        {
            Debug.Log("JE PASSE DEDANS");
            pCollector.AddPoints(pointsValue);
        }
    }
}
