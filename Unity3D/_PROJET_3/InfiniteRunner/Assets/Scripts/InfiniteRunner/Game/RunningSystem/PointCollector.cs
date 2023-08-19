using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCollector : MonoBehaviour
{
    [SerializeField] private int points;
    public int Points { get { return points; } protected set { points = value; } }

    public void AddPoints(int p_points)
    {
        points += p_points;
    }
    public void RemovePoints(int p_points)
    {
        points -= p_points;
    }

}
