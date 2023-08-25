using UnityEngine;

/// <summary>
/// D�finit un objet comme �tant un FragmentItem : quand le joueur le ramasse, le joueur est associ� en BDD avec un nouveau fragment disponible.
/// </summary>
public class FragmentItem : MonoBehaviour
{
    [Header("UI display au ramassage")]
    [SerializeField] GameObject fragmentUI;

    private SQLiteSessionDataQuery db;
    private IRunningGameService runningGameService;
    [SerializeField] private AudioSource audioSource;


    private void Start()
    {
        runningGameService = ServiceLocator.Instance.GetService<IRunningGameService>();
        db = ServiceLocator.Instance.GetService<ISessionService>().Query;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterAutoRunner c))
        {
            Disable();
            audioSource.Play();
            fragmentUI.SetActive(true);
            db.InsertNewFragmentForPlayer((int)runningGameService.TimeID); // Demande � la db d'associer le joueur � un fragment disponible. 
        } 
    }

    private void Disable()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        
    }

    private void Enable()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        
    }

    /// <summary>
    /// Le reset consiste � le mettre visible de nouveau, comme s'il n'avait pas �t� ramass�.
    /// </summary>
    public void Reset()
    {
        Enable();
    }

}
