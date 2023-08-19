using UnityEngine;

[RequireComponent(typeof(DBFragmentItem))]
public class FragmentItem : MonoBehaviour
{
    private DBFragmentItem DBFragmentItem;
    [SerializeField] GameObject fragmentUI;
    private void Start()
    {
        DBFragmentItem = GetComponent<DBFragmentItem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterAutoRunner c))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("FRAGMENT IMPACT !");
            DBFragmentItem.AddNewFragmentForPlayer();
            fragmentUI.SetActive(true);
        } 
    }

    public void Reset()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

}
