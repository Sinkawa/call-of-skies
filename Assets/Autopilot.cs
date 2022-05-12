using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Autopilot : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] [Tooltip("Force to push plane forwards with")] private float thrust = 100f;
    [SerializeField] [Tooltip("Pitch")] private float turnTorque = 90f;
    [SerializeField] [Tooltip("Multiplier for all forces")] private float forceMult = 1000f;
    
    [Header("Autopilot")]
    [SerializeField] [Tooltip("Sensitivity for autopilot flight.")] private float sensitivity = 5f;
    [SerializeField] [Tooltip("Angle at which airplane banks fully into target.")] private float aggressiveTurnAngle = 10f;
    
    [Header("Input")]
    [SerializeField] [Range(-1f, 1f)] private float pitch = 0f;
    
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }
    
    private void Update()
    {
        RunAutopilot(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())
            , out var autoPitch);
        
        pitch = autoPitch;
    }
    
    private void RunAutopilot(Vector3 flyTarget, out float pitch)
    {
        // This is my usual trick of converting the fly to position to local space.
        // You can derive a lot of information from where the target is relative to self.
        var localFlyTarget = _transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
        var angleOffTarget = Vector3.Angle(_transform.right, flyTarget - _transform.position);

        // IMPORTANT!
        // These inputs are created proportionally. This means it can be prone to
        // overshooting. The physics in this example are tweaked so that it's not a big
        // issue, but in something with different or more realistic physics this might
        // not be the case. Use of a PID controller for each axis is highly recommended.

        // ====================
        // PITCH AND YAW
        // ====================

        // Yaw/Pitch into the target so as to put it directly in front of the aircraft.
        // A target is directly in front the aircraft if the relative X and Y are both
        // zero. Note this does not handle for the case where the target is directly behind.
        //yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);
    }
    
    private void FixedUpdate()
    {
        // Ultra simple flight where the plane just gets pushed forward and manipulated
        // with torques to turn.
        _rigidbody2D.AddRelativeForce(Vector2.right * thrust * forceMult, ForceMode2D.Force);
        _rigidbody2D.AddTorque(turnTorque * pitch * forceMult, ForceMode2D.Force);
    }
    
}
