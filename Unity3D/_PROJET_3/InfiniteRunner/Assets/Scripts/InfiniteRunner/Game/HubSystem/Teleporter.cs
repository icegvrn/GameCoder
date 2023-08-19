using UnityEngine;
using UnityEngine.Events;


public class Teleporter : MonoBehaviour
{
    [SerializeField] UnityEvent OnTeleport;

    [SerializeField] bool isEnable;
    bool isTrigger;
    public bool IsEnable { get { return isEnable; } set { isEnable = value; } }
    [SerializeField] GameObject unavailableMessage;
    // Start is called before the first frame update
    void Start()
    {
        unavailableMessage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.gameObject.TryGetComponent(out CharacterController cc) && !isTrigger)
        {   
            isTrigger = true;
            if (isEnable)
            {
                OnTeleport.Invoke();
            }
            else
            {
                if (unavailableMessage != null)
                {
                    unavailableMessage.SetActive(true);
                }
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
            {
            isTrigger = false;
            if (!isEnable)
                {
                    if (unavailableMessage != null)
                    {
                        unavailableMessage.SetActive(false);
                    }
                }
            }
     
    }
}
