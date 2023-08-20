using UnityEngine;

/// <summary>
/// D�finit un objet comme �tant un FragmentItem : quand le joueur le ramasse, le joueur est associ� en BDD avec un nouveau fragment disponible.
/// </summary>
[RequireComponent(typeof(DBFragmentItem))]
public class FragmentItem : MonoBehaviour
{
    [Header("UI display au ramassage")]
    [SerializeField] GameObject fragmentUI;

    private DBFragmentItem DBFragmentItem;
    

    private void Start()
    {
        DBFragmentItem = GetComponent<DBFragmentItem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterAutoRunner c))
        {
            Disable();
            fragmentUI.SetActive(true);
            DBFragmentItem.AddNewFragmentForPlayer(); // Appel � DBFragmentItem pour demander � la db d'associer le joueur � un fragment disponible. 
        } 
    }

    private void Disable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void Enable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// Le reset consiste � le mettre visible de nouveau, comme s'il n'avait pas �t� ramass�.
    /// </summary>
    public void Reset()
    {
        Enable();
    }

}
