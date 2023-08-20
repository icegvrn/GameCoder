using UnityEngine;

/// <summary>
/// Permet de définir un objet comme obstacle en indiquant une fréquence d'apparition de celui-ci.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float frequency;
    public float Frequency { get { return frequency; } }
}
