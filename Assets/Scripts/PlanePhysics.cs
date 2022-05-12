// Copyright © 2022 Alexander Suvorov. All rights reserved.
// 
// This file is part of call-of-skies.
// 
// call-of-skies is free software: you can redistribute it and/or modify it under the terms of the GNU General
// Public License as published by the Free Software Foundation, either version 3 of the License, or (at your
// option) any later version.
// 
// call-of-skies is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
// implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
// Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with call-of-skies. If not, see
// <https://www.gnu.org/licenses/>.

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
