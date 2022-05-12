// Copyright © 2022 Alexander Suvorov. All rights reserved.
// 
// Based on the code from Brian-Stone on the Unity forums
// https://forum.unity.com/threads/rigidbody-lookat-torque.146625/#post-1005645
// 
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

using System;


[Serializable] public class PID
{
    private float _p, _i, _d;
    private float _kp, _ki, _kd;
    private float _prevError;
    
    public float Kp
    {
        get => _kp;
        set => _kp = value;
    }
    
    public float Ki
    {
        get => _ki;
        set => _ki = value;
    }
    
    public float Kd
    {
        get => _kd;
        set => _kd = value;
    }

    public PID(float p, float i, float d)
    {
        _kp = p;
        _ki = i;
        _kd = d;
    }
    
    public float GetOutput(float currentError, float deltaTime)
    {
        _p = currentError;
        _i += _p * deltaTime;
        _d = (_p - _prevError) / deltaTime;
        _prevError = currentError;

        return _p * Kp + _i * Ki + _d * Kd;
    }
}