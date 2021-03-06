using System;
using UnityEngine;

[Serializable]
public struct PlaneSettings
{
    [Header("General settings")] 
    [Tooltip("Mass")] 
    public static float mass = 1f;
    
    [Tooltip("Max speed")] 
    public static float maxSpeed = 1f;
    
    [Header("Engine settings")]
    [Tooltip("Max engine power")] 
    public static float enginePower = 100f;
    
    [Tooltip("Throttle change step")] 
    public static float throttleStep = 0.01f;
    
    [Tooltip("Max throttle multiplier")] 
    public static float maxThrottle = 1f;
    
    [Tooltip("Pitch change factor")] 
    public static float pitchFactor = 0.1f;

    [Header("PID Controller settings")]
    [Range(-10, 10)]
    public float p, i, d;

    [Header("Physics settings")]
    [Tooltip("Angle of Attack will be used in physics calculation if checked")] 
    public static bool useAngleOfAttack = false;
    
    [Tooltip("Factor used in gravity force calculation")] 
    public static float gravityFactor = 0.1f;
    
    [Tooltip("Factor used in drag force calculation")] 
    public static float dragFactor = 0.1f;
    
    [Tooltip("Factor used in lift force calculation")] 
    public static float liftFactor = 0.1f; 
}
