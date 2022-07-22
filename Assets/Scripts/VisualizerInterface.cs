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
    
    [SerializeField] private float MinLightTemperature = 1500f;
    [SerializeField] private float MaxLightTemperature = 20000f;
    
    [SerializeField] private float MinLightAngle = 0.0f;
    [SerializeField] private float MaxLightAngle = 180f;

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