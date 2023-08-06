using UnityEngine;

public class MouseAimCamera : MonoBehaviour
{
    public Transform target; // R�f�rence au personnage � suivre
    public float sensitivityX = 2.0f; // Sensibilit� du mouvement horizontal de la cam�ra
    public float sensitivityY = 2.0f; // Sensibilit� du mouvement vertical de la cam�ra
    public float minYAngle = -30.0f; // Angle minimal en Y
    public float maxYAngle = 30.0f; // Angle maximal en Y
    public float offsetY = 1.0f; // D�calage vertical par rapport au personnage

    private float rotationX = 0.0f;
    private float initialDistance; // Distance initiale entre la cam�ra et le personnage

    void Start()
    {
        if (target != null)
        {
            // Calculer la distance initiale entre la cam�ra et le personnage
            initialDistance = Vector3.Distance(transform.position, target.position);

            // Ajuster la position de la cam�ra en fonction du d�calage vertical
            Vector3 desiredPosition = target.position - transform.forward * initialDistance + Vector3.up * offsetY;
            transform.position = desiredPosition;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        // Obtenir les mouvements de la souris
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculer la rotation horizontale en fonction du mouvement horizontal de la souris
        target.Rotate(Vector3.up * mouseX * sensitivityX);

        // Calculer la rotation verticale en fonction du mouvement vertical de la souris
        rotationX -= mouseY * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);

        // Appliquer la rotation verticale � la cam�ra (autour du personnage)
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Positionner la cam�ra � une distance initiale du personnage
        Vector3 desiredPosition = target.position - transform.forward * initialDistance + Vector3.up * offsetY;
        transform.position = desiredPosition;
    }
}
