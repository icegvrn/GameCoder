using UnityEngine;

public class CameraControlByMouse : MonoBehaviour
{
    // Cible de la cam�ra
    [SerializeField] Transform target;
    // D�calage vertical par rapport � la cible
    [SerializeField] float offsetY = 1.0f;

    // Angles maximaux o� elle peut aller verticalement
    [SerializeField] float minYAngle = -30.0f;
    [SerializeField] float maxYAngle = 30.0f;

    // Sensibilit� de d�placement
    [SerializeField] float sensitivityX = 2.0f;
    [SerializeField] float sensitivityY = 2.0f; 


    private float rotationX = 0.0f;
    private float initialDistance;

    void Start()
    {
        if (target != null)
        {
            CameraInitialization();
        }
    }

    void CameraInitialization()
    {
        // Enregistrement de la distance initiale entre la cam�ra et la cible
        initialDistance = Vector3.Distance(transform.position, target.position);

        // Ajout de l'offset vertical voulu
        Vector3 desiredPosition = target.position - transform.forward * initialDistance + Vector3.up * offsetY;
        transform.position = desiredPosition;
    }

    void Update()
    {
        if (target != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Gestion de la rotation horizontale
            target.Rotate(Vector3.up * mouseX * sensitivityX);

            // Gestion de la rotation verticale
            rotationX -= mouseY * sensitivityY;
            rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);
            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Follow de la cam�ra par rapport � la cible et sa position initiale
            Vector3 desiredPosition = target.position - transform.forward * initialDistance + Vector3.up * offsetY;
            transform.position = desiredPosition;
        }
        else
        {
            Debug.LogError("Le composant " + gameObject.name + "a un script de contr�le cam�ra avec cible mais aucune cible n'a �t� assign�e.");
        }
    }
}
