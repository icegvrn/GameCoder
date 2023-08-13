

using UnityEngine;

public class CharacterPointCollector : PointCollector
    {
    RunStatsService runStatsService;

    private void Start()
    {
        runStatsService = ServiceLocator.Instance.GetService<RunStatsService>();
    }

    void Update()
    {
        if (runStatsService == null)
        {
            runStatsService = ServiceLocator.Instance.GetService<RunStatsService>();
        }
        runStatsService.UserEssences = Points;

        if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("P " + Points);
            Points += 100;
        }
    }
}

