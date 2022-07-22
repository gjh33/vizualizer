using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for lighting at runtime
/// </summary>
public class LightController : MonoBehaviour
{
    [Tooltip("The light being controlled")]
    [SerializeField] private Light Light;
    
    /// <summary>
    /// The color temperature of the light. Lower is warmer.
    /// </summary>
    public float Temperature { get => Light.colorTemperature; set => SetTemperature(value); }
    /// <summary>
    /// The intensity of the light. Values above 1 are subject to bloom and HDR tonemapping
    /// </summary>
    public float Intensity { get => Light.intensity; set => SetIntensity(value); }
    /// <summary>
    /// The elevation angle of the light in degrees.
    /// </summary>
    public float Angle { get => angle; set => SetAngle(value); }
    /// <summary>
    /// The azimuth angle of the light in degrees.
    /// </summary>
    public float Azimuth { get => azimuth; set => SetAzimuth(value); }

    // The angles are stored to prevent bugs from reading euler angles directly that were just written.
    private float angle;
    private float azimuth;

    private void OnEnable()
    {
        Vector3 eulerAngles = Light.transform.eulerAngles;
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