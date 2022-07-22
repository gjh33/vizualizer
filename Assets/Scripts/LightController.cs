using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light Light;
    
    public float Temperature { get => Light.colorTemperature; set => SetTemperature(value); }
    public float Intensity { get => Light.intensity; set => SetIntensity(value); }
    public float Angle { get => angle; set => SetAngle(value); }
    public float Azimuth { get => azimuth; set => SetAzimuth(value); }

    private float angle;
    private float azimuth;

    private void OnEnable()
    {
        var eulerAngles = Light.transform.eulerAngles;
        angle = eulerAngles.x;
        azimuth = eulerAngles.y;
    }

    private void SetTemperature(float temperature)
    {
        Light.colorTemperature = temperature;
    }
    
    private void SetIntensity(float intensity)
    {
        Light.intensity = intensity;
    }
    
    private void SetAngle(float angle)
    {
        this.angle = angle;
        Light.transform.rotation = Quaternion.Euler(angle, azimuth, 0);
    }
    
    private void SetAzimuth(float azimuth)
    {
        this.azimuth = azimuth;
        Light.transform.rotation = Quaternion.Euler(angle, azimuth, 0);
    }

    private void Reset()
    {
        Light = GetComponent<Light>();
    }
}