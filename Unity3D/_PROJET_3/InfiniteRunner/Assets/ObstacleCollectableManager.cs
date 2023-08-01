using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Obstacle))]
public class ObstacleCollectableManager : MonoBehaviour
{
   private List<CollectablesContainer> collectablesGroups;
    // Start is called before the first frame update
    void Start()
    {
        collectablesGroups = new List<CollectablesContainer>();

        foreach (CollectablesContainer c_group in GetComponentsInChildren<CollectablesContainer>())
        {
            collectablesGroups.Add(c_group);
            c_group.Init();
        }

        EnableRandomCollectables();

    }

    public void EnableRandomCollectables()
    {
        int randNb = Random.Range(0, collectablesGroups.Count+1);

        foreach (CollectablesContainer collecContainer in collectablesGroups)
        {

            if (randNb > collectablesGroups.Count || collectablesGroups.IndexOf(collecContainer) != randNb)
            {
                collecContainer.DisableGroup();
            }

            else
            {
                collecContainer.EnableGroup();
            }

        }
    }

    public void Reset()
    {
        EnableRandomCollectables();
    }
}
