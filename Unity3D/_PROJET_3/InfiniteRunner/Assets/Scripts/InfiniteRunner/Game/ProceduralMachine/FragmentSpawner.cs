using UnityEngine;

/// <summary>
/// Fait spawn un fragment � la demande (dans le cas pr�sent, du FragmentManager)
/// </summary>
public class FragmentSpawner : MonoBehaviour
{
    [SerializeField] FragmentItem fragmentItem;

    public void SpawnFragment()
    {
        fragmentItem.gameObject.SetActive(true);
    }
}
