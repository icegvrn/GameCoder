using UnityEngine;

/// <summary>
/// Permet de d�finir un objet comme obstacle en indiquant une fr�quence d'apparition de celui-ci.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float frequency;
    public float Frequency { get { return frequency; } }
}
