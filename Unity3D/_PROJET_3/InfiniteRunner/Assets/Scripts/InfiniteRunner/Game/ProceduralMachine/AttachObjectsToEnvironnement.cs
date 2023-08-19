using UnityEngine;

public class AttachObjectsToEnvironnement : MonoBehaviour
{
    [SerializeField] ObjectPool objectPool;
    private int previousChildrenNb = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PoolAssetsGenerator[] generators = GetComponentsInChildren<PoolAssetsGenerator>();

       if (previousChildrenNb < generators.Length)
        {
            foreach (PoolAssetsGenerator generator in generators)
            {
                if (generator.ObjectPool == null)
                {
                    generator.ObjectPool = objectPool;
                }
            }
        }
    }
}
