using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;

namespace Managers
{
public class FlockManager : MonoBehaviour
{
    public GameObject boidPrefab;
    public Bounds bounds = new Bounds( Vector3.zero, new Vector3( 14, 8 ) );

    [SerializeField, Range( 0, 512 )] public int boidsAmount = 256;
    [SerializeField, Range( 0, 16 )] private int layer = 0;

    private List<BoidModel> _boidModels = new List<BoidModel>();

    public void Run()
    {
        Vector2 RandomPositionInBounds()
        {
            Vector2 pos = Vector2.zero;
            pos.x = Random.Range( bounds.min.x, bounds.max.x );
            pos.y = Random.Range( bounds.min.y, bounds.max.y );
            return pos;
        }

        for ( int i = 0; i < boidsAmount; i++ )
        {
            BoidView boidView = Instantiate( boidPrefab, RandomPositionInBounds(), Quaternion.identity )
                .GetComponent<BoidView>();
            boidView.transform.parent = transform;
            _boidModels.Add( new BoidModel( boidView, bounds, layer ) );
        }

        for ( int i = 0; i < boidsAmount; i++ )
        {
            var otherBoids = new List<BoidModel>( _boidModels );
            var currentBoid = otherBoids[i];
            otherBoids.RemoveAt( i );
            currentBoid.SetNeighbourBoids( otherBoids );
        }
    }

    // Updates all the boids asynchronously
    private void Update()
    {
        var tasks = new List<Task>();

        foreach ( var boid in _boidModels ) boid.PreUpdate();

        foreach ( var boid in _boidModels ) tasks.Add( Task.Factory.StartNew( boid.Update ) );
        Task.WaitAll( tasks.ToArray() );

        foreach ( var boid in _boidModels ) boid.PostUpdate();
    }
}
}