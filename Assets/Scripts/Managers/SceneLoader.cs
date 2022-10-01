using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string rootScene;
    [SerializeField] private string levelScene;

    public void LoadLevelScene( Action<LevelManager> callback = null )
    {
        StartCoroutine( _LoadSceneAsync( levelScene,
            () =>
            {
                SceneManager.SetActiveScene( SceneManager.GetSceneByName( levelScene ) );
                callback?.Invoke( FindObjectOfType<LevelManager>() );
            } ) );
    }

    public void UnloadLevelScene( Action callback = null )
    {
        SceneManager.SetActiveScene( SceneManager.GetSceneByName( rootScene ) );
        StartCoroutine( _UnloadSceneAsync( levelScene, () => callback?.Invoke() ) );
    }

    // Load single scene
    private IEnumerator _LoadSceneAsync( string sceneName, Action callback = null )
    {
        if ( _TryLoadSceneAsync( sceneName, out var unloadingOperation ) )
        {
            yield return new WaitUntil( () => unloadingOperation.isDone );
            // yield return new Wait
            callback?.Invoke();
        }
    }

    // Unload single scene
    private IEnumerator _UnloadSceneAsync( string sceneName, Action callback = null )
    {
        if ( _TryUnloadSceneAsync( sceneName, out var unloadingOperation ) )
        {
            yield return new WaitUntil( () => unloadingOperation.isDone );
            callback?.Invoke();
        }
    }

    private bool _TryLoadSceneAsync( string sceneName, out AsyncOperation loadingOperation )
    {
        var sceneIsLoaded = false;
        for ( var i = 0; i < SceneManager.sceneCount; i++ )
        {
            sceneIsLoaded = sceneName.Equals( SceneManager.GetSceneAt( i ).name );
            if ( sceneIsLoaded ) break;
        }

        if ( !sceneIsLoaded )
        {
            loadingOperation = SceneManager.LoadSceneAsync( sceneName, LoadSceneMode.Additive );
            return true;
        }

        loadingOperation = null;
        return false;
    }

    private bool _TryUnloadSceneAsync( string sceneName, out AsyncOperation unloadingOperation )
    {
        for ( var i = 0; i < SceneManager.sceneCount; i++ )
        {
            if ( !sceneName.Equals( SceneManager.GetSceneAt( i ).name ) ) continue;
            unloadingOperation = SceneManager.UnloadSceneAsync( sceneName );
            return true;
        }

        unloadingOperation = null;
        return false;
    }


    private IEnumerator QueryCoroutines( IEnumerable<IEnumerator> coroutines )
    {
        yield return coroutines.Select( StartCoroutine ).GetEnumerator();
    }
}
}