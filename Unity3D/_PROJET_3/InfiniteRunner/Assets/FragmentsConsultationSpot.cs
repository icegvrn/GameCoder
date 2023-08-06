using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FragmentsConsultationSpot : MonoBehaviour
{
 
    bool isTrigger;
    bool isConsultationOpen;
    [SerializeField] GameObject Gemme;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject consultationPanel;
    
    void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A)) 
        {
            if (isTrigger && !isConsultationOpen)
            {
                isConsultationOpen = true;
                Gemme.SetActive(false);
                consultationPanel.SetActive(true);
                instructionsPanel.SetActive(false);
            }
            else if ((isTrigger && isConsultationOpen)) 
            {
                PreviousPage();
            }


        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if ((isTrigger && isConsultationOpen))
            {
                NextPage();
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent(out CharacterController cc) && !isTrigger)
        {
            Debug.Log("Je rentre pour " + other);
            isTrigger = true;
     
         
                if (instructionsPanel != null)
                {
                    instructionsPanel.SetActive(true);
                }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {
            isTrigger = false;
            Reset();
        }

    }

    private void Reset()
    {
        instructionsPanel.SetActive(false);
        consultationPanel.SetActive(false);
        Gemme.SetActive(true);
        isConsultationOpen = false;
    }

    void NextPage()
    {
        Debug.Log("PAGE SUIVANTE");
    }

    void PreviousPage()
    {
        Debug.Log("PAGE PRECEDENTE");
    }
}
