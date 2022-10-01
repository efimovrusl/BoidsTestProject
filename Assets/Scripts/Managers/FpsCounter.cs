using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
public class FpsCounter : MonoBehaviour
{
    public void CollectStatisticsForNextTimePeriod( float startDelaySeconds, float durationSeconds,
        Action<float> framerateCallback )
    {
        StartCoroutine( StatisticsCollectorCoroutine( startDelaySeconds, durationSeconds, framerateCallback ) );
    }

    private IEnumerator StatisticsCollectorCoroutine( float startDelaySeconds, float durationSeconds,
        Action<float> framerateCallback )
    {
        yield return new WaitForSeconds( startDelaySeconds );
        var waitInstruction = new WaitForEndOfFrame();
        int framesCounted = 0;
        float timeSpent = 0;
        while ( timeSpent < durationSeconds )
        {
            yield return waitInstruction;
            timeSpent += Time.unscaledDeltaTime;
            framesCounted++;
        }
        framerateCallback( framesCounted / timeSpent );
    }
}
}