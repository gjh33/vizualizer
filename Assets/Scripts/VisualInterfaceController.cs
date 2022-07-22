using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

/// <summary>
/// Controls and delegates functionality for the visualizer UI root.
/// </summary>
public class VisualInterfaceController : UIController
{
    private const string PickerContainerId = "picker-container";
    private const string SelectMeshButtonId = "select-mesh-button";
    private const string SelectMaterialButtonId = "select-material-button";
    private const string SelectTextureButtonId = "select-texture-button";
    private const string TranslateControlId = "translate-control";
    private const string RotateControlId = "rotate-control";
    private const string ScaleControlId = "scale-control";
    private const string LightControlId = "light-control";
    private const string EffectControlId = "effect-control";
    private const string TranslateAxisId = "translate-axis";
    private const string RotationAxisId = "rotation-axis";
    private const string ScaleAxisId = "scale-axis";
    private const string XZControlId = "xz-control";
    private const string XYControlId = "xy-control";
    private const string XControlId = "x-control";
    private const string YControlId = "y-control";
    private const string ZControlId = "z-control";
    private const string UniformControlId = "uniform-control";
    private const string LightingControlsId = "lighting-controls";
    private const string TemperatureSliderId = "temperature-slider";
    private const string AngleSliderId = "angle-slider";
    private const string AzimuthSliderId = "azimuth-slider";
    private const string IntensitySliderId = "intensity-slider";
    private const string EffectControlsId = "effect-controls";
    private const string BloomControlId = "bloom-control";
    private const string VignetteControlId = "vignette-control";
    private const string DepthOfFieldControlId = "depth-of-field-control";
    private const string ChromaticAberrationControlId = "chromatic-aberration-control";
    private const string FilmGrainControlId = "film-grain-control";
    private const string PaniniProjectionControlId = "panini-projection-control";

    private const string ControlSelectedClass = "selected";
    
    public Action<Mesh> OnMeshSelected;
    public Action<Material> OnMaterialSelected;
    public Action<Texture2D> OnTextureSelected;
    
    public Action<DisplayMesh.ControlMode> OnControlModeChanged;
    public Action<DisplayMesh.TranslationAxis> OnTranslationAxisChanged;
    public Action<DisplayMesh.RotationAxis> OnRotationAxisChanged;
    public Action<DisplayMesh.ScaleAxis> OnScaleAxisChanged;

    public Action<float> OnTemperatureSliderChanged;
    public Action<float> OnAngleSliderChanged;
    public Action<float> OnAzimuthSliderChanged;
    public Action<float> OnIntensitySliderChanged;
    
    public Action<bool> OnBloomToggled;
    public Action<bool> OnVignetteToggled;
    public Action<bool> OnDepthOfFieldToggled;
    public Action<bool> OnChromaticAberrationToggled;
    public Action<bool> OnFilmGrainToggled;
    public Action<bool> OnPaniniProjectionToggled;

    private Button selectMeshButton;
    private Button selectMaterialButton;
    private Button selectTextureButton;
    private Button translateControl;
    private Button rotateControl;
    private Button scaleControl;
    private Button lightControl;
    private Button effectControl;
    private Button translateXZControl;
    private Button translateXYControl;
    private Button rotateXControl;
    private Button rotateYControl;
    private Button rotateZControl;
    private Button scaleXControl;
    private Button scaleYControl;
    private Button scaleZControl;
    private Button scaleUniformControl;
    private Button bloomControl;
    private Button vignetteControl;
    private Button depthOfFieldControl;
    private Button chromaticAberrationControl;
    private Button filmGrainControl;
    private Button paniniProjectionControl;
    
    private VisualElement translateAxis;
    private VisualElement rotateAxis;
    private VisualElement scaleAxis;
    private VisualElement lightingControls;
    private VisualElement effectControls;
    
    private PickerController picker;
    private VerticalSliderController temperatureSlider;
    private VerticalSliderController angleSlider;
    private VerticalSliderController azimuthSlider;
    private VerticalSliderController intensitySlider;
    
    public VisualInterfaceController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        InitSafeArea();
        
        picker = new PickerController(root.Q<VisualElement>(PickerContainerId), cardTemplate);
        picker.OnMeshSelected += OnPickerMeshSelected;
        picker.OnMaterialSelected += OnPickerMaterialSelected;
        picker.OnTextureSelected += OnPickerTextureSelected;
        
        temperatureSlider = new VerticalSliderController(root.Q<VisualElement>(TemperatureSliderId));
        temperatureSlider.OnValueChanged += OnTemperatureSliderValueChanged;
        
        angleSlider = new VerticalSliderController(root.Q<VisualElement>(AngleSliderId));
        angleSlider.OnValueChanged += OnAngleSliderValueChanged;
        
        azimuthSlider = new VerticalSliderController(root.Q<VisualElement>(AzimuthSliderId));
        azimuthSlider.OnValueChanged += OnAzimuthSliderValueChanged;
        
        intensitySlider = new VerticalSliderController(root.Q<VisualElement>(IntensitySliderId));
        intensitySlider.OnValueChanged += OnIntensitySliderValueChanged;
    }

    public void Update()
    {
        picker.Update();
    }
    
    public void SelectTranslateControl()
    {
        translateControl.AddToClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Translate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.Flex;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    public void SelectRotateControl()
    {
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.AddToClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Rotate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.Flex;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    public void SelectScaleControl()
    {
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.AddToClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Scale);
        scaleAxis.style.display = DisplayStyle.Flex;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    public void SelectLightControl()
    {
        lightControl.AddToClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.None);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.Flex;
        effectControls.style.display = DisplayStyle.None;
    }

    public void SelectEffectControl()
    {
        effectControl.AddToClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.None);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.Flex;
    }
    
    public void SelectTranslateXZControl()
    {
        translateXZControl.AddToClassList(ControlSelectedClass);
        translateXYControl.RemoveFromClassList(ControlSelectedClass);
        OnTranslationAxisChanged?.Invoke(DisplayMesh.TranslationAxis.XZ);
    }
    
    public void SelectTranslateXYControl()
    {
        translateXZControl.RemoveFromClassList(ControlSelectedClass);
        translateXYControl.AddToClassList(ControlSelectedClass);
        OnTranslationAxisChanged?.Invoke(DisplayMesh.TranslationAxis.XY);
    }
    
    public void SelectRotateXControl()
    {
        rotateXControl.AddToClassList(ControlSelectedClass);
        rotateYControl.RemoveFromClassList(ControlSelectedClass);
        rotateZControl.RemoveFromClassList(ControlSelectedClass);
        OnRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.X);
    }
    
    public void SelectRotateYControl()
    {
        rotateXControl.RemoveFromClassList(ControlSelectedClass);
        rotateYControl.AddToClassList(ControlSelectedClass);
        rotateZControl.RemoveFromClassList(ControlSelectedClass);
        OnRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.Y);
    }
    
    public void SelectRotateZControl()
    {
        rotateXControl.RemoveFromClassList(ControlSelectedClass);
        rotateYControl.RemoveFromClassList(ControlSelectedClass);
        rotateZControl.AddToClassList(ControlSelectedClass);
        OnRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.Z);
    }
    
    public void SelectScaleXControl()
    {
        scaleXControl.AddToClassList(ControlSelectedClass);
        scaleYControl.RemoveFromClassList(ControlSelectedClass);
        scaleZControl.RemoveFromClassList(ControlSelectedClass);
        scaleUniformControl.RemoveFromClassList(ControlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.X);
    }
    
    public void SelectScaleYControl()
    {
        scaleXControl.RemoveFromClassList(ControlSelectedClass);
        scaleYControl.AddToClassList(ControlSelectedClass);
        scaleZControl.RemoveFromClassList(ControlSelectedClass);
        scaleUniformControl.RemoveFromClassList(ControlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Y);
    }
    
    public void SelectScaleZControl()
    {
        scaleXControl.RemoveFromClassList(ControlSelectedClass);
        scaleYControl.RemoveFromClassList(ControlSelectedClass);
        scaleZControl.AddToClassList(ControlSelectedClass);
        scaleUniformControl.RemoveFromClassList(ControlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Z);
    }
    
    public void SelectScaleUniformControl()
    {
        scaleXControl.RemoveFromClassList(ControlSelectedClass);
        scaleYControl.RemoveFromClassList(ControlSelectedClass);
        scaleZControl.RemoveFromClassList(ControlSelectedClass);
        scaleUniformControl.AddToClassList(ControlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Uniform);
    }
    
    public void SetLightTemperature(float temperature)
    {
        temperatureSlider.Value = temperature;
    }
    
    public void SetLightAngle(float angle)
    {
        angleSlider.Value = angle;
    }
    
    public void SetLightIntensity(float intensity)
    {
        intensitySlider.Value = intensity;
    }
    
    public void SetLightAzimuth(float azimuth)
    {
        azimuthSlider.Value = azimuth;
    }
    
    public void SelectBloomControl()
    {
        bloomControl.ToggleInClassList(ControlSelectedClass);
        OnBloomToggled?.Invoke(bloomControl.ClassListContains(ControlSelectedClass));
    }
    
    public void SelectVignetteControl()
    {
        vignetteControl.ToggleInClassList(ControlSelectedClass);
        OnVignetteToggled?.Invoke(vignetteControl.ClassListContains(ControlSelectedClass));
    }
    
    public void SelectDepthOfFieldControl()
    {
        depthOfFieldControl.ToggleInClassList(ControlSelectedClass);
        OnDepthOfFieldToggled?.Invoke(depthOfFieldControl.ClassListContains(ControlSelectedClass));
    }
    
    public void SelectChromaticAberrationControl()
    {
        chromaticAberrationControl.ToggleInClassList(ControlSelectedClass);
        OnChromaticAberrationToggled?.Invoke(chromaticAberrationControl.ClassListContains(ControlSelectedClass));
    }
    
    public void SelectFilmGrainControl()
    {
        filmGrainControl.ToggleInClassList(ControlSelectedClass);
        OnFilmGrainToggled?.Invoke(filmGrainControl.ClassListContains(ControlSelectedClass));
    }
    
    public void SelectPaniniProjectionControl()
    {
        paniniProjectionControl.ToggleInClassList(ControlSelectedClass);
        OnPaniniProjectionToggled?.Invoke(paniniProjectionControl.ClassListContains(ControlSelectedClass));
    }
    
    protected override void CollectElements()
    {
        base.CollectElements();
        
        selectMeshButton = root.Q<Button>(SelectMeshButtonId);
        selectMaterialButton = root.Q<Button>(SelectMaterialButtonId);
        selectTextureButton = root.Q<Button>(SelectTextureButtonId);
        translateControl = root.Q<Button>(TranslateControlId);
        rotateControl = root.Q<Button>(RotateControlId);
        scaleControl = root.Q<Button>(ScaleControlId);
        lightControl = root.Q<Button>(LightControlId);
        effectControl = root.Q<Button>(EffectControlId);
        
        translateAxis = root.Q<VisualElement>(TranslateAxisId);
        rotateAxis = root.Q<VisualElement>(RotationAxisId);
        scaleAxis = root.Q<VisualElement>(ScaleAxisId);
        lightingControls = root.Q<VisualElement>(LightingControlsId);
        effectControls = root.Q<VisualElement>(EffectControlsId);
        
        translateXZControl = translateAxis.Q<Button>(XZControlId);
        translateXYControl = translateAxis.Q<Button>(XYControlId);
        rotateXControl = rotateAxis.Q<Button>(XControlId);
        rotateYControl = rotateAxis.Q<Button>(YControlId);
        rotateZControl = rotateAxis.Q<Button>(ZControlId);
        scaleXControl = scaleAxis.Q<Button>(XControlId);
        scaleYControl = scaleAxis.Q<Button>(YControlId);
        scaleZControl = scaleAxis.Q<Button>(ZControlId);
        scaleUniformControl = scaleAxis.Q<Button>(UniformControlId);
        
        bloomControl = effectControls.Q<Button>(BloomControlId);
        vignetteControl = effectControls.Q<Button>(VignetteControlId);
        depthOfFieldControl = effectControls.Q<Button>(DepthOfFieldControlId);
        chromaticAberrationControl = effectControls.Q<Button>(ChromaticAberrationControlId);
        filmGrainControl = effectControls.Q<Button>(FilmGrainControlId);
        paniniProjectionControl = effectControls.Q<Button>(PaniniProjectionControlId);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        
        selectMeshButton.clickable.clicked += OnSelectMeshButtonClicked;
        selectMaterialButton.clickable.clicked += OnSelectMaterialButtonClicked;
        selectTextureButton.clickable.clicked += OnSelectTextureButtonClicked;
        translateControl.clickable.clicked += OnTranslateControlClicked;
        rotateControl.clickable.clicked += OnRotateControlClicked;
        scaleControl.clickable.clicked += OnScaleControlClicked;
        lightControl.clickable.clicked += OnLightControlClicked;
        effectControl.clickable.clicked += OnEffectControlClicked;
        
        translateXYControl.clickable.clicked += OnTranslateXYControlClicked;
        translateXZControl.clickable.clicked += OnTranslateXZControlClicked;
        rotateXControl.clickable.clicked += OnRotateXControlClicked;
        rotateYControl.clickable.clicked += OnRotateYControlClicked;
        rotateZControl.clickable.clicked += OnRotateZControlClicked;
        scaleXControl.clickable.clicked += OnScaleXControlClicked;
        scaleYControl.clickable.clicked += OnScaleYControlClicked;
        scaleZControl.clickable.clicked += OnScaleZControlClicked;
        scaleUniformControl.clickable.clicked += OnScaleUniformControlClicked;
        
        bloomControl.clickable.clicked += OnBloomControlClicked;
        vignetteControl.clickable.clicked += OnVignetteControlClicked;
        depthOfFieldControl.clickable.clicked += OnDepthOfFieldControlClicked;
        chromaticAberrationControl.clickable.clicked += OnChromaticAberrationControlClicked;
        filmGrainControl.clickable.clicked += OnFilmGrainControlClicked;
        paniniProjectionControl.clickable.clicked += OnPaniniProjectionControlClicked;
    }

    private void InitSafeArea()
    {
        Vector2 screenTopLeft = new Vector2(Screen.safeArea.xMin, Screen.height - Screen.safeArea.yMax);
        Vector2 screenBottomRight = new Vector2(Screen.width - Screen.safeArea.xMax, Screen.safeArea.yMin);
        Vector2 topLeft = RuntimePanelUtils.ScreenToPanel(root.panel, screenTopLeft);
        Vector2 bottomRight = RuntimePanelUtils.ScreenToPanel(root.panel, screenBottomRight);
        var safeAreas = root.Query<VisualElement>(className: "safe-area");
        foreach (var safeArea in safeAreas.ToList())
        {
            safeArea.style.marginLeft = topLeft.x;
            safeArea.style.marginTop = topLeft.y;
            safeArea.style.marginRight = bottomRight.x;
            safeArea.style.marginBottom = bottomRight.y;
        }
    }

    private void OnBloomControlClicked()
    {
        SelectBloomControl();
    }

    private void OnVignetteControlClicked()
    {
        SelectVignetteControl();
    }

    private void OnDepthOfFieldControlClicked()
    {
        SelectDepthOfFieldControl();
    }

    private void OnChromaticAberrationControlClicked()
    {
        SelectChromaticAberrationControl();
    }

    private void OnFilmGrainControlClicked()
    {
        SelectFilmGrainControl();
    }

    private void OnPaniniProjectionControlClicked()
    {
        SelectPaniniProjectionControl();
    }

    private void OnAzimuthSliderValueChanged(float value)
    {
        OnAzimuthSliderChanged?.Invoke(value);
    }
    
    private void OnIntensitySliderValueChanged(float value)
    {
        OnIntensitySliderChanged?.Invoke(value);
    }
    
    private void OnAngleSliderValueChanged(float angle)
    {
        OnAngleSliderChanged?.Invoke(angle);
    }

    private void OnTemperatureSliderValueChanged(float temp)
    {
        OnTemperatureSliderChanged?.Invoke(temp);
    }

    private void OnPickerTextureSelected(Texture2D tex)
    {
        OnTextureSelected?.Invoke(tex);
    }

    private void OnPickerMaterialSelected(Material mat)
    {
        OnMaterialSelected?.Invoke(mat);
    }

    private void OnPickerMeshSelected(Mesh mesh)
    {
        OnMeshSelected?.Invoke(mesh);
    }
    
    private void OnTranslateXYControlClicked()
    {
        SelectTranslateXYControl();
    }
    
    private void OnTranslateXZControlClicked() 
    {
        SelectTranslateXZControl();
    }
    
    private void OnRotateXControlClicked() 
    {
        SelectRotateXControl();
    }
    
    private void OnRotateYControlClicked() 
    {
        SelectRotateYControl();
    }
    
    private void OnRotateZControlClicked() 
    {
        SelectRotateZControl();
    }
    
    private void OnScaleXControlClicked() 
    {
        SelectScaleXControl();
    }
    
    private void OnScaleYControlClicked() 
    {
        SelectScaleYControl();
    }
    
    private void OnScaleZControlClicked() 
    {
        SelectScaleZControl();
    }
    
    private void OnScaleUniformControlClicked() 
    {
        SelectScaleUniformControl();
    }

    private void OnEffectControlClicked()
    {
        SelectEffectControl();
    }

    private void OnLightControlClicked()
    {
        SelectLightControl();
    }
    
    private void OnScaleControlClicked()
    {
        SelectScaleControl();
    }

    private void OnRotateControlClicked()
    {
        SelectRotateControl();
    }

    private void OnTranslateControlClicked()
    {
        SelectTranslateControl();
    }

    private void OnSelectMeshButtonClicked()
    {
        picker.PickMesh();
    }

    private void OnSelectMaterialButtonClicked()
    {
        picker.PickMaterial();
    }

    private void OnSelectTextureButtonClicked()
    {
        picker.PickTexture();
    }
}