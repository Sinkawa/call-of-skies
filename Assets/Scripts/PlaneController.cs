using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlaneController : MonoBehaviour
{
    [Header("General settings")] 
    [SerializeField] [Tooltip("Mass")] private float mass = 1f;
    [SerializeField] [Tooltip("Max speed")] private float maxVelocity = 1f;
    
    [Header("Engine settings")]
    [SerializeField] [Tooltip("Max engine power")] private float enginePower = 100f;
    [SerializeField] [Tooltip("Throttle change step")] private float throttleStep = 0.01f;
    [SerializeField] [Tooltip("Max throttle multiplier")] private float maxThrottle = 1f;
    [SerializeField] [Tooltip("Pitch change factor")] private float pitchFactor = 0.1f;

    [Header("PID Controller settings")]
    [SerializeField] [Range(-10, 10)] [Tooltip("Proportional")] private float p;
    [SerializeField] [Range(-10, 10)] [Tooltip("Integral")] private float i;
    [SerializeField] [Range(-10, 10)] [Tooltip("Derivative")] private float d;

    [Header("Physics settings")]
    [SerializeField] [Tooltip("Angle of Attack will be used in physics calculation if checked")] private bool useAngleOfAttack = false;
    [SerializeField] [Tooltip("Factor used in gravity force calculation")] private float gravityFactor = 0.1f;
    [SerializeField] [Tooltip("Factor used in drag force calculation")] private float dragFactor = 0.1f;
    [SerializeField] [Tooltip("Factor used in lift force calculation")] private float liftFactor = 0.1f;
    
    private float _thrustPercent = 0;
    private float _throttleDelta = 0;

    private PID _zAxisPIDController;
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private Mouse _mouse;
    private Camera _mainCamera;

    [SerializeField] private Text thrustText;
    
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _mouse = Mouse.current;
        _mainCamera = Camera.main;
        
        _zAxisPIDController = new PID(p, i, d);
    }

    private void FixedUpdate()
    {
        ChangeThrottle();
        
        var velocity = _rigidbody2D.velocity.magnitude;
        var angleOfAttack = GetAngleOfAttack(); 
        
        var gravityForce = FlightPhysicsCalculator.GetGravityForce(mass, gravityFactor);
        var dragForce = FlightPhysicsCalculator.GetDragForce(velocity, dragFactor, useAngleOfAttack, angleOfAttack);
        var liftForce = FlightPhysicsCalculator.GetLiftForce(velocity, liftFactor, useAngleOfAttack, angleOfAttack);
        
        ApplyThrust(velocity, dragForce);
        ApplyTurnTest();
    }

    private void Update()
    {
        _zAxisPIDController.Kp = p;
        _zAxisPIDController.Ki = i;
        _zAxisPIDController.Kd = d;
    }

    private float GetAngleOfAttack()
    {
        return _transform.eulerAngles.z;
    }
    
    private string DebugString<T>(string nameOf, T value)
    {
        return $"{nameOf}: {value}";
    }
    
    private void ApplyTurnTest()
    {
        var destinationVector = GetDestinationVector();
        var headingVector = _transform.right;
        
        var angle = Vector2.SignedAngle(headingVector, destinationVector);
        
        //float zAngleError = Mathf.DeltaAngle(_transform.rotation.eulerAngles.z, destinationVector.eulerAngles.z);
        float zTorqueCorrection = _zAxisPIDController.GetOutput(angle, Time.fixedDeltaTime);

        zTorqueCorrection = Mathf.Clamp(zTorqueCorrection, -1f, 1f);
        
        
        Debug.Log($"{DebugString(nameof(destinationVector), destinationVector)} | {DebugString(nameof(headingVector), headingVector)} | {DebugString(nameof(angle), angle)} | {DebugString(nameof(zTorqueCorrection), zTorqueCorrection)}");
        
        _rigidbody2D.AddTorque(zTorqueCorrection);
    }

    private void ApplyThrust(float velocity, float dragForce)
    {
        var percent = _thrustPercent / maxThrottle;
        var maxVelocityOnThrottle = percent * maxVelocity;
        
        
        
        var currentThrust = percent * enginePower;

        var thrustVector = _transform.right * currentThrust * Time.fixedDeltaTime;
        
        _rigidbody2D.AddForce(thrustVector, ForceMode2D.Force);
        
        Debug.Log($"current Thrust: {currentThrust} / vector of force: {_transform.forward * currentThrust}");    
    }

    private void ApplyTurn()
    {
        var destinationVector = GetDestinationVector();
        var headingVector = _transform.right;
        
        var angle = Vector2.SignedAngle(headingVector, destinationVector);
        var signFactor = Mathf.Sign(angle);
        var angleFactor = signFactor * angle / 180;
        
        var currentTorque = signFactor * angleFactor * pitchFactor * Mathf.Abs(_rigidbody2D.velocity.sqrMagnitude);
        
        _rigidbody2D.AddTorque(currentTorque);
    }

    private Vector2 GetDestinationVector()
    {
        var transformPosition = _transform.position;
        
        var mousePosition = _mouse.position.ReadValue();
        var destinationPoint = _mainCamera.ScreenToWorldPoint(mousePosition);
        destinationPoint.z = transformPosition.z;
        
        var destinationVector = destinationPoint - transformPosition;
        destinationVector.Normalize();
        
        return destinationVector;
    }
    
    private Vector3 GetDestinationPoint()
    {
        var mousePosition = _mouse.position.ReadValue();
        var destinationPoint = _mainCamera.ScreenToWorldPoint(mousePosition);
        destinationPoint.z = 0f;
        
        return destinationPoint;
    }

    private void ChangeThrottle()
    {
        _thrustPercent += _throttleDelta;
        _thrustPercent = Mathf.Clamp(_thrustPercent, 0, maxThrottle);
        thrustText.text = $"{_thrustPercent} %";
    }
    
    public void OnThrottleChange(InputAction.CallbackContext context)
    {
        var multiplier = context.ReadValue<float>();
        _throttleDelta = multiplier * throttleStep;
    }

}