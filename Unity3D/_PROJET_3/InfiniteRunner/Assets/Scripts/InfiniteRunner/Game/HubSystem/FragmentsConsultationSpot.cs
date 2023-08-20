using UnityEngine;
using static InputService;

/// <summary>
/// Gère la navigation dans les fragments d'histoires : permet d'ouvrir un spot de consultat, de passer de l'histoire suivante ou précédente et de quitter.
/// </summary>
public class FragmentsConsultationSpot : MonoBehaviour
{
    // Elements serialises
    [SerializeField] GameObject Gemme;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject consultationPanel;
    
    private FragmentContentFromDB container;

    // States
    private bool isTrigger;
    private bool isConsultationOpen;
    private int currentPanelIndex;
    
    //Systeme custom d'input
    private IInputService input;
    

    void Start()
    {
        input = ServiceLocator.Instance.GetService<IInputService>();
        InitFragmentContainer();
        CloseState();
    }

    private void Update()
    {
        if (input.GetKeyUp(ActionKey.interact))
        {
            if (isTrigger && !isConsultationOpen)
            {
                OpenState();
            }
        }

        if (input.GetKeyUp(ActionKey.quit))
        {
            if ((isTrigger && isConsultationOpen))
            {
                CloseState();
            }
        }

        if (input.GetKeyUp(ActionKey.previous))
        {
            if ((isTrigger && isConsultationOpen))
            {
                PreviousPage();
            }
        }

        if (input.GetKeyUp(ActionKey.next))
        {
            if ((isTrigger && isConsultationOpen))
            {
                NextPage();
            }
        }
     
    }

    void InitFragmentContainer()
    {
        container = consultationPanel.GetComponent<FragmentContentFromDB>();
    }

    private void OpenState()
    {
        isConsultationOpen = true;
        Gemme.SetActive(false);
        consultationPanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }
    private void CloseState()
    {
        instructionsPanel.SetActive(false);
        consultationPanel.SetActive(false);
        Gemme.SetActive(true);
        isConsultationOpen = false;
    }

    void NextPage()
    {
        if (currentPanelIndex < container.FragmentsPanels.Count-1)
        {
            container.FragmentsPanels[currentPanelIndex].gameObject.SetActive(false);
            currentPanelIndex++;
            container.FragmentsPanels[currentPanelIndex].gameObject.SetActive(true);
        }

        else
        {
            container.FragmentsPanels[currentPanelIndex].gameObject.SetActive(false);
            container.LoadData();
            container.FragmentsPanels[0].gameObject.SetActive(false);
            currentPanelIndex++;
            container.FragmentsPanels[currentPanelIndex].gameObject.SetActive(true);
        }
    }

    void PreviousPage()
    {
        if (currentPanelIndex > 0)
        {
            container.FragmentsPanels[currentPanelIndex].gameObject.SetActive(false);
            currentPanelIndex--;
            container.FragmentsPanels[currentPanelIndex].gameObject.SetActive(true);
        }
    }

    void ShowInstructionPanel()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {
            isTrigger = true;
            ShowInstructionPanel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {
            isTrigger = false;
            CloseState();
        }
    }
}
