using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesContainer : MonoBehaviour
{
    [SerializeField] private List<Collectable> collectables;
    public List<Collectable> Collectables { get { return collectables; } }

    // Start is called before the first frame update
    public void Init()
    {
        foreach (Collectable collectable in GetComponentsInChildren<Collectable>())
        {
            collectables.Add(collectable);
        }
    }

    public void EnableGroup()
    {
     
        foreach (Collectable c in collectables)
        {
            c.gameObject.SetActive(true);
        }
    }

    public void DisableGroup()
    {
      
        foreach (Collectable c in collectables)
        {
            c.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        foreach (Collectable c in collectables)
        {
            c.gameObject.SetActive(true);
        }
    }
}
