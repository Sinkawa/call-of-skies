using UnityEngine;

public static class FlightPhysicsCalculator
{
    private const float AirDensity = 1.25f;
    
  
    /// <param name="angleOfAttack">in degrees</param>
    private static float GetAngleOfAttackFactor(float angleOfAttack = 0f)
    {
        var angleOfAttackFactor = 0f;
        if ((angleOfAttack > 0 && angleOfAttack <= 45) || (angleOfAttack > 90 && angleOfAttack <= 135))
            angleOfAttackFactor = Mathf.Sin(angleOfAttack * Mathf.Deg2Rad);
        else if ((angleOfAttack > 45 && angleOfAttack <= 90) || (angleOfAttack > 135 && angleOfAttack <= 180))
            angleOfAttackFactor = Mathf.Cos(angleOfAttack * Mathf.Deg2Rad);
        return angleOfAttackFactor;
    }
    
    public static float GetDragForce(float velocity, float dragFactor, bool useAngleOfAttack = false, float angleOfAttack = 0f)
    {
        //var attackAngle = Vector2.Angle(_transform.right, _rigidbody2D.velocity.normalized);
        
        var dragForce = velocity * velocity * dragFactor;
        return (useAngleOfAttack) ? dragForce * GetAngleOfAttackFactor(angleOfAttack) : dragForce;
    }
    
    public static float GetGravityForce(float mass, float gravityFactor)
    {
        var gravityForce = gravityFactor * mass;
        return gravityForce;
    }
    
    public static float GetLiftForce(float velocity, float liftFactor, bool useAngleOfAttack = false, float angleOfAttack = 0f)
    {
        //var attackAngle = transform.eulerAngles.z;
        
        var liftForce = liftFactor * 0.5f * AirDensity * velocity * velocity;
        return (useAngleOfAttack) ? liftForce * GetAngleOfAttackFactor(angleOfAttack) : liftForce;
    }
}
