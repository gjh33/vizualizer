using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Represents the interface to the visualizer UI for the game world.
/// </summary>
public class VisualizerInterface : MonoBehaviour
{
    [SerializeField] private UIDocument UserInterface;
    [SerializeField] private VisualTreeAsset CarouselCardTemplate;
    [SerializeField] private DisplayMesh DisplayMesh;
    [SerializeField] private LightController LightController;
    [SerializeField] private EffectsController EffectsController;
    
    [SerializeField] private float MinLightAngle = 0.0f;
    [SerializeField] private float MaxLightAngle = 180f;
    
    [SerializeField] private float MinLightAzimuth = 0.0f;
    [SerializeField] private float MaxLightAzimuth = 360f;
    
    [SerializeField] private float MinLightIntensity = 0.0f;
    [SerializeField] private float MaxLightIntensity = 3.0f;
    
    [SerializeField] private float MinLightTemperature = 1500f;
    [SerializeField] private float MaxLightTemperature = 20000f;

    private VisualInterfaceController rootController;

    private void OnEnable()
    {
        rootController = new VisualInterfaceController(UserInterface.rootVisualElement, CarouselCardTemplate);
        
        // Delay until geometry is updated
        UserInterface.rootVisualElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        InitDefaults();
        RegisterCallbacks();
    }

    private void Update()
    {
        rootController.Update();
    }

    private void Reset()
    {
        UserInterface = GetComponent<UIDocument>();
    }

    private void RegisterCallbacks()
    {
        rootController.OnMeshSelected += OnMeshSelected;
        rootController.OnMaterialSelected += OnMaterialSelected;
        rootController.OnTextureSelected += OnTextureSelected;
        rootController.OnControlModeChanged += OnControlModeChanged;
        rootController.OnTranslationAxisChanged += OnTranslationAxisChanged;
        rootController.OnRotationAxisChanged += OnRotationAxisChanged;
        rootController.OnScaleAxisChanged += OnScaleAxisChanged;
        rootController.OnTemperatureSliderChanged += OnTemperatureSliderChanged;
        rootController.OnAngleSliderChanged += OnLightAngleSliderChanged;
        rootController.OnAzimuthSliderChanged += OnLightAzimuthSliderChanged;
        rootController.OnIntensitySliderChanged += OnLightIntensitySliderChanged;
        rootController.OnBloomToggled += OnBloomToggled;
        rootController.OnVignetteToggled += OnVignetteToggled;
        rootController.OnDepthOfFieldToggled += OnDepthOfFieldToggled;
        rootController.OnChromaticAberrationToggled += OnChromaticAberrationToggled;
        rootController.OnFilmGrainToggled += OnFilmGrainToggled;
        rootController.OnPaniniProjectionToggled += OnPaniniProjectionToggled;
    }

    private void InitDefaults()
    {
        switch (DisplayMesh.CurrentControlMode)
        {
            case DisplayMesh.ControlMode.Translate:
                rootController.SelectTranslateControl();
                break;
            case DisplayMesh.ControlMode.Rotate:
                rootController.SelectRotateControl();
                break;
            case DisplayMesh.ControlMode.Scale:
                rootController.SelectScaleControl();
                break;
        }

        switch (DisplayMesh.CurrentTranslationAxis)
        {
            case DisplayMesh.TranslationAxis.XZ:
                rootController.SelectTranslateXZControl();
                break;
            case DisplayMesh.TranslationAxis.XY:
                rootController.SelectTranslateXYControl();
                break;
        }
        
        switch (DisplayMesh.CurrentRotationAxis)
        {
            case DisplayMesh.RotationAxis.X:
                rootController.SelectRotateXControl();
                break;
            case DisplayMesh.RotationAxis.Y:
                rootController.SelectRotateYControl();
                break;
            case DisplayMesh.RotationAxis.Z:
                rootController.SelectRotateZControl();
                break;
        }
        
        switch (DisplayMesh.CurrentScaleAxis)
        {
            case DisplayMesh.ScaleAxis.X:
                rootController.SelectScaleXControl();
                break;
            case DisplayMesh.ScaleAxis.Y:
                rootController.SelectScaleYControl();
                break;
            case DisplayMesh.ScaleAxis.Z:
                rootController.SelectScaleZControl();
                break;
            case DisplayMesh.ScaleAxis.Uniform:
                rootController.SelectScaleUniformControl();
                break;
        }
        
        float anglePercent = (LightController.Angle - MinLightAngle) / (MaxLightAngle - MinLightAngle);
        rootController.SetLightAngle(anglePercent);
        float temperaturePercent = (LightController.Temperature - MinLightTemperature) / (MaxLightTemperature - MinLightTemperature);
        rootController.SetLightTemperature(temperaturePercent);
        float intensityPercent = (LightController.Intensity - MinLightIntensity) / (MaxLightIntensity - MinLightIntensity);
        rootController.SetLightIntensity(intensityPercent);
        float azimuthPercent = (LightController.Azimuth - MinLightAzimuth) / (MaxLightAzimuth - MinLightAzimuth);
        rootController.SetLightAzimuth(azimuthPercent);
    }

    private void OnPaniniProjectionToggled(bool on)
    {
        EffectsController.SetPaniniProjection(on);
    }

    private void OnFilmGrainToggled(bool on)
    {
        EffectsController.SetFilmGrain(on);
    }

    private void OnChromaticAberrationToggled(bool on)
    {
        EffectsController.SetChromaticAberration(on);
    }

    private void OnDepthOfFieldToggled(bool on)
    {
        EffectsController.SetDepthOfField(on);
    }

    private void OnVignetteToggled(bool on)
    {
        EffectsController.SetVignette(on);
    }

    private void OnBloomToggled(bool on)
    {
        EffectsController.SetBloom(on);
    }

    private void OnLightAngleSliderChanged(float a)
    {
        float angle = Mathf.Lerp(MinLightAngle, MaxLightAngle, a);
        LightController.Angle = angle;
    }

    private void OnTemperatureSliderChanged(float t)
    {
        float temp = Mathf.Lerp(MinLightTemperature, MaxLightTemperature, t);
        LightController.Temperature = temp;
    }

    private void OnLightIntensitySliderChanged(float i)
    {
        float intensity = Mathf.Lerp(MinLightIntensity, MaxLightIntensity, i);
        LightController.Intensity = intensity;
    }

    private void OnLightAzimuthSliderChanged(float a)
    {
        float azimuth = Mathf.Lerp(MinLightAzimuth, MaxLightAzimuth, a);
        LightController.Azimuth = azimuth;
    }

    private void OnScaleAxisChanged(DisplayMesh.ScaleAxis axis)
    {
        DisplayMesh.CurrentScaleAxis = axis;
    }

    private void OnRotationAxisChanged(DisplayMesh.RotationAxis axis)
    {
        DisplayMesh.CurrentRotationAxis = axis;
    }

    private void OnTranslationAxisChanged(DisplayMesh.TranslationAxis axis)
    {
        DisplayMesh.CurrentTranslationAxis = axis;
    }

    private void OnControlModeChanged(DisplayMesh.ControlMode mode)
    {
        DisplayMesh.CurrentControlMode = mode;
    }

    private void OnTextureSelected(Texture2D tex)
    {
        DisplayMesh.SetTexture(tex);
    }

    private void OnMaterialSelected(Material mat)
    {
        DisplayMesh.SetMaterial(mat);
    }

    private void OnMeshSelected(Mesh mesh)
    {
        DisplayMesh.SetMesh(mesh);
    }
}