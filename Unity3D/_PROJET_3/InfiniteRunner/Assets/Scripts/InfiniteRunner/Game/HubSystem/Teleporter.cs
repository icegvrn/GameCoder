using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Gère l'action du coeur du portail : soit afficher un message si portail inactif, soit effectuer une action à partir de l'UnityEvent (ici changer de scène)
/// </summary>
public class Teleporter : MonoBehaviour
{
    [Header("Etat")]
    [SerializeField] private bool isEnable;

    [Header("A afficher quand indisponible")]
    [SerializeField] GameObject unavailableMessage;

    [Header("Action du portail")]
    [SerializeField] private UnityEvent OnTeleport;

    public bool IsEnable { get { return isEnable; } set { isEnable = value; } }
    private bool isTrigger;


    void Start()
    {
        HideMessages();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc) && !isTrigger)
        {
            isTrigger = true;
            EnableTeleporter(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {
            isTrigger = false;
            DisableTeleporter();
        }
    }

    void HideMessages()
    {
        unavailableMessage.SetActive(false);
    }

    void ShowMessages()
    {
        unavailableMessage.SetActive(true);
    }

    void EnableTeleporter()
    {
        if (isEnable)
        {
            OnTeleport.Invoke();
        }

        else
        {
            if (unavailableMessage != null)
            {
                ShowMessages();
            }
        }
    }

    void DisableTeleporter()
    {
        if (!isEnable && unavailableMessage != null)
        {
            HideMessages();
        }
    }
}
