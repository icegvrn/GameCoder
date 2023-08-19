using UnityEngine;

public class ColliderOffset : MonoBehaviour
{
    [SerializeField] float offset = 0.01f;

    void Start()
    {
        GetComponent<BoxCollider>().contactOffset = offset;
    }
}
