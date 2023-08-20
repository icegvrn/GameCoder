using UnityEngine;
using static InputService;
public class ClassementConsultationSpot : MonoBehaviour
{
    // Composition du canvas
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject consultationPanel;
    [SerializeField] GameObject QuitButton;
   
    // Données du canvas
    private RectTransform canvasTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    // State du canvas
    private bool isTrigger;
    private bool isConsultationOpen;

    private IInputService input;
   

    private void Start()
    {
        input = ServiceLocator.Instance.GetService<IInputService>();
        InitCanvas();
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
            if (isConsultationOpen)
            {
                CloseState();
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController cc))
        {
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
            instructionsPanel.SetActive(false);
        }
    }

    void InitCanvas()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        canvasTransform = canvas.GetComponent<RectTransform>();
        originalPosition = canvasTransform.localPosition;
        originalRotation = canvasTransform.localRotation;
        originalScale = canvasTransform.localScale;
        QuitButton.SetActive(false);
    }

    void CloseState()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        QuitButton.SetActive(false);
        canvasTransform.localPosition = originalPosition;
        canvasTransform.localRotation = originalRotation;
        canvasTransform.localScale = originalScale;
        isConsultationOpen = false;
    }

    void OpenState()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        isConsultationOpen = true;
        consultationPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        QuitButton.SetActive(true);
    }

}
