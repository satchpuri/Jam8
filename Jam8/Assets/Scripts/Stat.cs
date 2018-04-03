using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{
    [SerializeField]
    private HealthBar health;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }
        set
        {
            currentVal = Mathf.Clamp(value,0,MaxVal);
            health.Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
            health.MaxValue = maxVal;
        }
    }
    public void Initialize()
    {
        MaxVal = maxVal;
        CurrentVal = currentVal;    
    }
}
