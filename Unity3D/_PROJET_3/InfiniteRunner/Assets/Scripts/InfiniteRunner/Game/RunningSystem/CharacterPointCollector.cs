using UnityEngine;
using static InputService;

/// <summary>
/// Classe héritante de PointCollector. Elle communique avec le Running Game Service pour lui indiquer le nombre de point collecté.
/// </summary>
public class CharacterPointCollector : PointCollector
{
    IRunningGameService runStatsService;
    IInputService input;

    private void Start()
    {
        InitCollector();
    }

    void InitCollector()
    {
        runStatsService = ServiceLocator.Instance.GetService<IRunningGameService>();
        input = ServiceLocator.Instance.GetService<IInputService>();
        Points = 0;
    }

    /// <summary>
    /// Envoi constant des points cumulés au RunningGameService.
    /// </summary>
    void Update()
    {
        runStatsService.UserEssences = Points;

        //(demo) Cheat pour gagner des points plus rapidement
        if (input.GetKeyDown(ActionKey.cheatPoints))
        {
            Points += 10;
        }
    }
}

