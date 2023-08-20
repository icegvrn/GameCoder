using UnityEngine;

/// <summary>
/// D�finit un objet comme �tant collectable : il ajoute un nombre de "points" d�fini lorsqu'il est trigger par un objet ayant un "pointCollector".
/// </summary>
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
