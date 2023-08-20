using UnityEngine;

/// <summary>
/// Classe permettant de définir un objet comme étant un collecteur de points. Il peut donc ajouter ou enlever des points à un nombre de points.
/// </summary>
public class PointCollector : MonoBehaviour
{
    [Header("Configuration des points")]
    [SerializeField] private int points;
    public int Points { get { return points; } protected set { points = value; } }


    /// <summary>
    /// Ajout des points en integer
    /// </summary>
    /// <param name="p_points"></param>
    public void AddPoints(int p_points)
    {
        points += p_points;
    }

    /// <summary>
    /// Suppression de points en integer
    /// </summary>
    /// <param name="p_points"></param>
    public void RemovePoints(int p_points)
    {
        if ((points - p_points) >= 0)
        {
            points -= p_points;
        }   
    }

}
