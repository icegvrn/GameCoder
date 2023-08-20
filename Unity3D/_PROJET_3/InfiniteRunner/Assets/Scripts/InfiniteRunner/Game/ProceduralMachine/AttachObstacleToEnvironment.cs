using UnityEngine;

/// <summary>
/// Permet l'attache d'une nouvelle pool d'obstacle choisie dès qu'un nouveau générateur d'obstacle est instancié dans la scène.
/// </summary>
public class AttachObstacleToEnvironment : MonoBehaviour
{
    [SerializeField] ObjectPool obstaclePool;
    private int previousChildrenNb = 0;

    void Update()
    {
        ObstacleSpawner[] generators = GetComponentsInChildren<ObstacleSpawner>();

        if (previousChildrenNb != generators.Length)
        {
            foreach (ObstacleSpawner generator in generators)
            {
                if (generator.ObstaclePool == null)
                {
                    generator.ObstaclePool = obstaclePool;
                }
            }
            previousChildrenNb = generators.Length;
        }
    }
}

