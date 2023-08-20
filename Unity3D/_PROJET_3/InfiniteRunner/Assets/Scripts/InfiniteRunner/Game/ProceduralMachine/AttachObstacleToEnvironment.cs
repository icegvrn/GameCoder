using UnityEngine;

/// <summary>
/// Permet l'attache d'une nouvelle pool d'obstacle choisie d�s qu'un nouveau g�n�rateur d'obstacle est instanci� dans la sc�ne.
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

