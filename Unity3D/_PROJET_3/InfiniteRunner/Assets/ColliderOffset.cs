using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOffset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().contactOffset = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
