using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField] int sceneToLoad;
    [SerializeField] bool isEnable;
    bool isTrigger;
    public bool IsEnable { get { return isEnable; } set { isEnable = value; } }
    [SerializeField] GameObject unavailableMessage;
    // Start is called before the first frame update
    void Start()
    {
        unavailableMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.gameObject.TryGetComponent(out CharacterController cc) && !isTrigger)
        {   
            Debug.Log("Je rentre pour " + other);
            isTrigger = true;
            if (isEnable)
            {
                SceneManager.LoadScene(sceneToLoad);
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
