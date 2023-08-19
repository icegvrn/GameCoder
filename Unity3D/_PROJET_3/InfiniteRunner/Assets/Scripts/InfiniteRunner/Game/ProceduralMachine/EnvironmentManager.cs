using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] PathGenerator pathGenerator;

    public void Reset()
    {
        pathGenerator.Reset();  
    }
}
