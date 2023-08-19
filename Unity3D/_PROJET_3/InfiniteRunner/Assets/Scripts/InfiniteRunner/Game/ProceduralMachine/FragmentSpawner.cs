using UnityEngine;

public class FragmentSpawner : MonoBehaviour
{
    [SerializeField] FragmentItem fragmentItem;

    public void SpawnFragment()
    {
        fragmentItem.gameObject.SetActive(true);
    }
}
