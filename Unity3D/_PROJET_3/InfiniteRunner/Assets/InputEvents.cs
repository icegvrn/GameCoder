using UnityEngine.Events;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    [SerializeField]UnityEvent onEscape;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            onEscape.Invoke();
        }
    }
}
