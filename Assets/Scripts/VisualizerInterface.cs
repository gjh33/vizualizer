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
    [Tooltip("The UI Document containing the user interface.")]
    [SerializeField] private UIDocument UserInterface;
    [Tooltip("The template for carousel cards in the picker.")]
    [SerializeField] private VisualTreeAsset CarouselCardTemplate;
    [Tooltip("The display mesh being controlled by the interface")]
    [SerializeField] private DisplayMesh DisplayMesh;
    [Tooltip("The light controller being controlled by the interface")]
    [SerializeField] private LightController LightController;
    [Tooltip("The effects controller being controlled by the interface")]
    [SerializeField] private EffectsController EffectsController;
    [Tooltip("The mesh library used to load built in meshes")]
    [SerializeField] private MeshLibraryAsset MeshLibrary;
    [Tooltip("The material library used to load built in materials")]
    [SerializeField] private MaterialLibraryAsset MaterialLibrary;
    [Tooltip("The texture library used to load built in textures")]
    [SerializeField] private TextureLibraryAsset TextureLibrary;
    
    [Tooltip("The min light angle for the slider control")]
    [SerializeField] private float MinLightAngle = 0.0f;
    [Tooltip("The max light angle for the slider control")]
    [SerializeField] private float MaxLightAngle = 180f;
    
    [Tooltip("The min light azimuth for the slider control")]
    [SerializeField] private float MinLightAzimuth = 0.0f;
    [Tooltip("The max light azimuth for the slider control")]
    [SerializeField] private float MaxLightAzimuth = 360f;
    
    [Tooltip("The min light intensity for the slider control")]
    [SerializeField] private float MinLightIntensity = 0.0f;
    [Tooltip("The max light intensity for the slider control")]
    [SerializeField] private float MaxLightIntensity = 3.0f;
    
    [Tooltip("The min light temperature for the slider control")]
    [SerializeField] private float MinLightTemperature = 1500f;
    [Tooltip("The max light temperature for the slider control")]
    [SerializeField] private float MaxLightTemperature = 20000f;

    private VisualInterfaceController rootController;

    private void OnEnable()
    {
        rootController = new VisualInterfaceController(
            UserInterface.rootVisualElement, 
            CarouselCardTemplate,
            MeshLibrary,
            MaterialLibrary,
            TextureLibrary);
        
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
        rootController.OnMeshControlModeChanged += OnControlModeChanged;
        rootController.OnMeshTranslationPlaneChanged += OnTranslationPlaneChanged;
        rootController.OnMeshRotationAxisChanged += OnRotationAxisChanged;
        rootController.OnMeshScaleAxisChanged += OnScaleAxisChanged;
        rootController.OnLightTemperatureSliderChanged += OnTemperatureSliderChanged;
        rootController.OnLightAngleSliderChanged += OnLightAngleSliderChanged;
        rootController.OnLightAzimuthSliderChanged += OnLightAzimuthSliderChanged;
        rootController.OnLightIntensitySliderChanged += OnLightIntensitySliderChanged;
        rootController.OnBloomToggled += OnBloomToggled;
        rootController.OnVignetteToggled += OnVignetteToggled;
        rootController.OnDepthOfFieldToggled += OnDepthOfFieldToggled;
        rootController.OnChromaticAberrationToggled += OnChromaticAberrationToggled;
        rootController.OnFilmGrainToggled += OnFilmGrainToggled;
        rootController.OnPaniniProjectionToggled += OnPaniniProjectionToggled;
    }

    // Scans the current state of the scene and sets up the UI to match accordingly
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

        switch (DisplayMesh.CurrentTranslationPlane)
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
        
        if (EffectsController.Bloom)
        {
            rootController.SelectBloomControl();
        }
        if (EffectsController.Vignette)
        {
            rootController.SelectVignetteControl();
        }
        if (EffectsController.DepthOfField)
        {
            rootController.SelectDepthOfFieldControl();
        }
        if (EffectsController.ChromaticAberration)
        {
            rootController.SelectChromaticAberrationControl();
        }
        if (EffectsController.FilmGrain)
        {
            rootController.SelectFilmGrainControl();
        }
        if (EffectsController.PaniniProjection)
        {
            rootController.SelectPaniniProjectionControl();
        }
    }

    private void OnPaniniProjectionToggled(bool on)
    {
        EffectsController.PaniniProjection = on;
    }

    private void OnFilmGrainToggled(bool on)
    {
        EffectsController.FilmGrain = on;
    }

    private void OnChromaticAberrationToggled(bool on)
    {
        EffectsController.ChromaticAberration = on;
    }

    private void OnDepthOfFieldToggled(bool on)
    {
        EffectsController.DepthOfField = on;
    }

    private void OnVignetteToggled(bool on)
    {
        EffectsController.Vignette = on;
    }

    private void OnBloomToggled(bool on)
    {
        EffectsController.Bloom = on;
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

    private void OnTranslationPlaneChanged(DisplayMesh.TranslationAxis plane)
    {
        DisplayMesh.CurrentTranslationPlane = plane;
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