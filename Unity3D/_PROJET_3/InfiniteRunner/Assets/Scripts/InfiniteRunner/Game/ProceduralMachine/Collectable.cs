using UnityEngine;


public class Collectable : MonoBehaviour
{
    [SerializeField] private int pointsValue;

   void OnTriggerEnter(Collider other)
    {
    gameObject.SetActive(false);
        if (other.gameObject.TryGetComponent(out PointCollector pCollector))
        {
        
            pCollector.AddPoints(pointsValue);
        }
    }
}
