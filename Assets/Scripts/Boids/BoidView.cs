using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoidView : MonoBehaviour
{
    #region InspectorValues
    
    [Range( 0, 10 ), OnValueChanged( "OnMaxSpeedChangeInvocator" )]
    public float maxSpeed;
    public event Action<float> OnMaxSpeedChange;
    private void OnMaxSpeedChangeInvocator() => OnMaxSpeedChange?.Invoke( maxSpeed );

    [Range( .1f, .5f ), OnValueChanged( "OnMaxForceChangeInvocator" )]
    public float maxForce = .03f;
    public event Action<float> OnMaxForceChange;
    private void OnMaxForceChangeInvocator() => OnMaxForceChange?.Invoke( maxForce );
    
    [Range( 1, 10 ), OnValueChanged( "OnNeighborhoodRadiusChangeInvocator" )]
    public float neighborhoodRadius = 3f;
    public event Action<float> OnNeighborhoodRadiusChange;
    private void OnNeighborhoodRadiusChangeInvocator() => OnNeighborhoodRadiusChange?.Invoke( neighborhoodRadius );

    [Range( 0, 3 ), OnValueChanged( "OnSeparationAmountChangeInvocator" )] 
    public float separationAmount = 1f;
    public event Action<float> OnSeparationAmountChange;
    private void OnSeparationAmountChangeInvocator() => OnSeparationAmountChange?.Invoke( separationAmount );

    [Range( 0, 3 ), OnValueChanged( "OnCohesionAmountChangeInvocator" )] 
    public float cohesionAmount = 1f;
    public event Action<float> OnCohesionAmountChange;
    private void OnCohesionAmountChangeInvocator() => OnCohesionAmountChange?.Invoke( cohesionAmount );
    
    [Range( 0, 3 ), OnValueChanged( "OnAlignmentAmountChangeInvocator" )] 
    public float alignmentAmount = 1f;
    public event Action<float> OnAlignmentAmountChange;
    private void OnAlignmentAmountChangeInvocator() => OnAlignmentAmountChange?.Invoke( alignmentAmount );

    #endregion
    
    public void UpdateTransform( Vector2 velocity )
    {
        // Updating position
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
        
        // Updating rotation
        var angle = Mathf.Atan2( velocity.y, velocity.x ) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler( new Vector3( 0, 0, angle ) );
    }

}