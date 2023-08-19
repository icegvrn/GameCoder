using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassementConsultationSpot : MonoBehaviour
{

    bool isTrigger;
    bool isConsultationOpen;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject consultationPanel;
    [SerializeField] Canvas canvas;
    private RectTransform canvasTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private Canvas canvasComponent;

    [SerializeField] GameObject QuitButton;

    private void Start()
    {
        // Récupérer les références nécessaires
        canvasTransform = canvas.GetComponent<RectTransform>();
        canvasComponent = canvas.GetComponent<Canvas>();
         
        // Sauvegarder l'état d'origine du Canvas
        originalPosition = canvasTransform.localPosition;
        originalRotation = canvasTransform.localRotation;
        originalScale = canvasTransform.localScale;
        QuitButton.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (isTrigger && !isConsultationOpen)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                isConsultationOpen = true;
                consultationPanel.SetActive(true);
                instructionsPanel.SetActive(false);
                QuitButton.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if ((isTrigger && isConsultationOpen))
            {
                canvas.renderMode = RenderMode.WorldSpace;
                QuitButton.SetActive(false);
                canvasTransform.localPosition = originalPosition;
                canvasTransform.localRotation = originalRotation;
                canvasTransform.localScale = originalScale;
                isConsultationOpen = false;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent(out CharacterController cc) && !isTrigger)
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

}
