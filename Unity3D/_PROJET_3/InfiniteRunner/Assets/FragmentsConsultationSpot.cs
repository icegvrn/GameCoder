using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FragmentsConsultationSpot : MonoBehaviour
{
 
    bool isTrigger;
    bool isConsultationOpen;
    private int currentPanelIndex;
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

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if ((isTrigger && isConsultationOpen))
            {
                Reset();
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

        if (currentPanelIndex < consultationPanel.GetComponent<FragmentsDBContainer>().Fragments.Count-1)
        {
            consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[currentPanelIndex].gameObject.SetActive(false);
            currentPanelIndex++;
            consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[currentPanelIndex].gameObject.SetActive(true);
        }

        else
        {   consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[currentPanelIndex].gameObject.SetActive(false);
            consultationPanel.GetComponent<FragmentsDBContainer>().GetAllUsersRandomFragments();
            consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[0].gameObject.SetActive(false);
            currentPanelIndex++;
            consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[currentPanelIndex].gameObject.SetActive(true);
        }


    }

    void PreviousPage()
    {
        Debug.Log("PAGE PRECEDENTE");
        if (currentPanelIndex > 0)
        {
            consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[currentPanelIndex].gameObject.SetActive(false);
            currentPanelIndex--;
            consultationPanel.GetComponent<FragmentsDBContainer>().Fragments[currentPanelIndex].gameObject.SetActive(true);
        }
    }
}
