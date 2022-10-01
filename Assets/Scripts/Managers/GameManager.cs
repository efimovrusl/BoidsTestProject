using System;
using UnityEngine;

namespace Managers
{
public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    public void RunSimulation( int frontFlockSize, int backFlockSize, int durationSeconds, Action<float> callback )
    {
        sceneLoader.LoadLevelScene( levelManager =>
        {
            levelManager.RunSimulation( frontFlockSize, backFlockSize, durationSeconds, frameRate =>
                sceneLoader.UnloadLevelScene( () => callback?.Invoke( frameRate ) ) );
        } );
    }
}
}