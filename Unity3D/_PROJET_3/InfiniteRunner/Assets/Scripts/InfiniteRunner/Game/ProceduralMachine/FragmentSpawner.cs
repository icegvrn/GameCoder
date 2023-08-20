using UnityEngine;

/// <summary>
/// Fait spawn un fragment à la demande (dans le cas présent, du FragmentManager)
/// </summary>
public class FragmentSpawner : MonoBehaviour
{
    [SerializeField] FragmentItem fragmentItem;

    public void SpawnFragment()
    {
        fragmentItem.gameObject.SetActive(true);
    }
}
