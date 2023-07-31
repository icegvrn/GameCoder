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
    this.gameObject.SetActive(false);
        if (other.gameObject.TryGetComponent(out PointCollector pCollector))
        {
            pCollector.AddPoints(pointsValue);
            Debug.Log("C'est le joueur qui touche et qui a désormais " + pCollector.Points);
        }
    }
}
