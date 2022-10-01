using System;
using UnityEngine;

namespace Managers
{
public class LevelManager : MonoBehaviour
{
    [SerializeField] private FlockManager frontFlockManager;
    [SerializeField] private FlockManager backFlockManager;
    [SerializeField] private FpsCounter fpsCounter;

    public void RunSimulation( int frontFlockSize, int backFlockSize, int durationSeconds, Action<float> callback )
    {
        frontFlockManager.boidsAmount = frontFlockSize;
        backFlockManager.boidsAmount = backFlockSize;
        frontFlockManager.Run();
        backFlockManager.Run();
        fpsCounter.CollectStatisticsForNextTimePeriod( 1, durationSeconds, callback );
    }
}
}
