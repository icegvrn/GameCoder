using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Obstacle obstacle)
    {
       GameObject spObstacle = Instantiate(obstacle.gameObject);
        spObstacle.transform.parent = transform;   
        spObstacle.transform.localPosition = new Vector3(0, spObstacle.transform.position.y, 0);
    }
}
