using UnityEngine;


/// <summary>
/// Permet l'attache d'une nouvelle pool d'objet choisie d�s qu'un nouveau g�n�rateur d'asset est instanci� dans la sc�ne.
/// </summary>
public class AttachObjectsToEnvironnement : MonoBehaviour
{
    [SerializeField] ObjectPool objectPool;
    private int previousChildrenNb = 0;

    private void Update()
    {
        AttachGeneratorsToObjectPool();
    }

    private void AttachGeneratorsToObjectPool()
    {
        AssetsGeneratorByPool[] generators = GetComponentsInChildren<AssetsGeneratorByPool>();

        if (previousChildrenNb != generators.Length)
        {
            foreach (AssetsGeneratorByPool generator in generators)
            {
                if (generator.ObjectPool == null)
                {
                    generator.ObjectPool = objectPool;
                }
            }   
            previousChildrenNb = generators.Length;
        }
    }
}
