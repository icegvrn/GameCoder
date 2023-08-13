using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentSpawner : MonoBehaviour
{
    [SerializeField] FragmentItem fragmentItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFragment()
    {
        fragmentItem.gameObject.SetActive(true);
    }
}
