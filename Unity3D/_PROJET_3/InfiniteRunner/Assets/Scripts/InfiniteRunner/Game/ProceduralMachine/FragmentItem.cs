using UnityEngine;

/// <summary>
/// Définit un objet comme étant un FragmentItem : quand le joueur le ramasse, le joueur est associé en BDD avec un nouveau fragment disponible.
/// </summary>
public class FragmentItem : MonoBehaviour
{
    [Header("UI display au ramassage")]
    [SerializeField] GameObject fragmentUI;

    private SQLiteSessionDataQuery db;
    private IRunningGameService runningGameService;

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
            fragmentUI.SetActive(true);
            db.InsertNewFragmentForPlayer((int)runningGameService.TimeID); // Demande à la db d'associer le joueur à un fragment disponible. 
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
    /// Le reset consiste à le mettre visible de nouveau, comme s'il n'avait pas été ramassé.
    /// </summary>
    public void Reset()
    {
        Enable();
    }

}
