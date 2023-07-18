using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float theSpeed = speed*Time.deltaTime;
        Vector3 movement = transform.position;
        if (Input.GetKey(KeyCode.Z))
        {
            movement.z += theSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            movement.x -= theSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.z -= theSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x += theSpeed;
        }
        transform.position = movement;
    }
}
