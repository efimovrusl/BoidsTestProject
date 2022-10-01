using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidModel
{
    private readonly BoidView _boidView;
    private readonly Bounds _bounds;

    // can be updated in runtime
    private List<BoidModel> _otherBoids;

    // recalculated every frame
    private List<BoidModel> _boidsInRadius;
    private List<BoidModel> _boidsInHalfRadius;

    private float _maxSpeed;
    private float _maxForce;
    private float _neighborhoodRadius;
    private float _separationAmount;
    private float _cohesionAmount;
    private float _alignmentAmount;

    private int _layer;

    private Vector2 _position;

    private Vector2 Position
    {
        get => _boidView.transform.position;
        set => _boidView.transform.position = new Vector3( value.x, value.y, _layer );
    }

    private Vector2 _velocity;

    public BoidModel( BoidView boidView, Bounds bounds, int layer )
    {
        _boidView = boidView;
        _bounds = bounds;
        _boidsInRadius = new List<BoidModel>();
        _boidsInHalfRadius = new List<BoidModel>();

        #region Copying View's properties

        _maxSpeed = boidView.maxSpeed;
        _maxForce = boidView.maxForce;
        _neighborhoodRadius = boidView.neighborhoodRadius;
        _separationAmount = boidView.separationAmount;
        _cohesionAmount = boidView.cohesionAmount;
        _alignmentAmount = boidView.alignmentAmount;

        _position = boidView.transform.position;
        _layer = layer;

        #endregion

        #region Subscribing to View properties' changes

        boidView.OnMaxSpeedChange += value => _maxSpeed = value;
        boidView.OnMaxForceChange += value => _maxForce = value;
        boidView.OnNeighborhoodRadiusChange += value => _neighborhoodRadius = value;
        boidView.OnSeparationAmountChange += value => _separationAmount = value;
        boidView.OnCohesionAmountChange += value => _cohesionAmount = value;
        boidView.OnAlignmentAmountChange += value => _alignmentAmount = value;

        #endregion

        #region Randomizing initial values

        float angle = Random.Range( 0, 2 * Mathf.PI );
        _velocity = new Vector2( Mathf.Cos( angle ), Mathf.Sin( angle ) );
        _boidView.UpdateTransform( _velocity );

        #endregion
    }

    public void SetNeighbourBoids( List<BoidModel> otherBoids )
    {
        _otherBoids = otherBoids;
    }

    public void PreUpdate()
    {
        _position = LoopPositionInBounds( Position, _bounds );
        Position = _position;
    }

    public void PostUpdate()
    {
        _boidView.UpdateTransform( _velocity );
    }

    // Asynchronous model-only code (no Unity's code allowed)
    public void Update()
    {
        if ( _otherBoids == null ) throw new Exception( "Need to call SetNeighbourBoids() after constructor" );

        _boidsInRadius.Clear();
        _boidsInHalfRadius.Clear();

        foreach ( var boid in _otherBoids )
        {
            var distance = Vector2.Distance( _position, boid._position );
            
            if ( distance < _neighborhoodRadius )
            {
                _boidsInRadius.Add( boid );
                if ( distance < _neighborhoodRadius / 2 )
                {
                    _boidsInHalfRadius.Add( boid );
                }
            }
        }

        _velocity += GetAcceleration();
        _velocity = Vector2.ClampMagnitude( _velocity, _maxSpeed );
    }

    // Combines Alignment, Cohesion and Separation together for better performance
    private Vector2 GetAcceleration()
    {
        if ( _boidsInRadius.Count == 0 ) return Vector2.zero;

        var velocity = Vector2.zero;
        var sumPositions = Vector2.zero;
        var halfRadiusDir = Vector2.zero;

        // alignment & cohesion
        // TODO: Theoretically possible to delegate to compute shader
        // TODO: BUT probably needs to be done on a higher level (for all boids at the same time)
        foreach ( var boid in _boidsInRadius )
        {
            velocity += boid._velocity;
            sumPositions += boid._position;
        }

        // separation
        foreach ( var boid in _boidsInHalfRadius )
        {
            var difference = _position - boid._position;
            halfRadiusDir += difference.normalized / difference.magnitude;
        }

        // alignment, cohesion & separation
        velocity /= _boidsInRadius.Count;
        var radiusDir = sumPositions / _boidsInRadius.Count - _position;
        halfRadiusDir /= _boidsInHalfRadius.Count;

        // steering
        var alignment = Steer( velocity.normalized * _maxSpeed );
        var cohesion = Steer( radiusDir.normalized * _maxSpeed );
        var separation = Steer( halfRadiusDir.normalized * _maxSpeed );

        return _alignmentAmount * alignment +
               _cohesionAmount * cohesion +
               _separationAmount * separation;
    }

    private Vector2 Steer( Vector2 desired )
    {
        return Vector2.ClampMagnitude( desired - _velocity, _maxForce );
    }

    private static Vector2 LoopPositionInBounds( Vector2 pnt, Bounds bnd ) // pnt - point, bnd - bounds
    {
        if ( pnt.x > bnd.max.x ) pnt.x -= bnd.size.x;
        if ( pnt.x < bnd.min.x ) pnt.x += bnd.size.x;
        if ( pnt.y > bnd.max.y ) pnt.y -= bnd.size.y;
        if ( pnt.y < bnd.min.y ) pnt.y += bnd.size.y;
        return pnt;
    }
}
