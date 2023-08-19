using UnityEngine;


public class DisplayChildOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {

            transform.GetChild(0).gameObject.SetActive(true); 
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {

            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
