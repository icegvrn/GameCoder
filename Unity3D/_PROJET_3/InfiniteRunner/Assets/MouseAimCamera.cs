using UnityEngine;

public class MouseAimCamera : MonoBehaviour
{
    public Transform target; // Référence au personnage à suivre
    public float sensitivityX = 2.0f; // Sensibilité du mouvement horizontal de la caméra
    public float sensitivityY = 2.0f; // Sensibilité du mouvement vertical de la caméra
    public float minYAngle = -30.0f; // Angle minimal en Y
    public float maxYAngle = 30.0f; // Angle maximal en Y
    public float offsetY = 1.0f; // Décalage vertical par rapport au personnage

    private float rotationX = 0.0f;
    private float initialDistance; // Distance initiale entre la caméra et le personnage

    void Start()
    {
        if (target != null)
        {
            // Calculer la distance initiale entre la caméra et le personnage
            initialDistance = Vector3.Distance(transform.position, target.position);

            // Ajuster la position de la caméra en fonction du décalage vertical
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

        // Appliquer la rotation verticale à la caméra (autour du personnage)
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Positionner la caméra à une distance initiale du personnage
        Vector3 desiredPosition = target.position - transform.forward * initialDistance + Vector3.up * offsetY;
        transform.position = desiredPosition;
    }
}
