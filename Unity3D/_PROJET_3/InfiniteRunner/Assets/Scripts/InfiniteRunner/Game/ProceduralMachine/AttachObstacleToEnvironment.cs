using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObstacleToEnvironment : MonoBehaviour
{

    [SerializeField] ObjectPool obstaclePool;
    private int previousChildrenNb = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ObstacleSpawner[] generators = GetComponentsInChildren<ObstacleSpawner>();

        if (previousChildrenNb < generators.Length)
        {
            foreach (ObstacleSpawner generator in generators)
            {
                if (generator.ObstaclePool == null)
                {
                    generator.ObstaclePool = obstaclePool;
                }
            }
        }
    }
}

