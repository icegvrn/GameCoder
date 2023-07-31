using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCollector : MonoBehaviour
{
    [SerializeField] private int points;
    public int Points { get { return points; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints(int p_points)
    {
        points += p_points;
    }
    public void RemovePoints(int p_points)
    {
        points -= p_points;
    }

}
