using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

/// <summary>
/// Controls and delegates functionality for the visualizer UI.
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
    
    /// <summary>
    /// Fired when a mesh has been chosen by the picker
    /// </summary>
    public Action<Mesh> OnMeshSelected;
    /// <summary>
    /// Fired when a material has been chosen by the picker
    /// </summary>
    public Action<Material> OnMaterialSelected;
    /// <summary>
    /// Fired when a texture has been chosen by the picker
    /// </summary>
    public Action<Texture2D> OnTextureSelected;
    
    /// <summary>
    /// Fired when a different control mode has been selected for the display mesh
    /// </summary>
    public Action<DisplayMesh.ControlMode> OnMeshControlModeChanged;
    /// <summary>
    /// Fired when the plane of translation for the display mesh has been changed
    /// </summary>
    public Action<DisplayMesh.TranslationAxis> OnMeshTranslationPlaneChanged;
    /// <summary>
    /// Fired when the axis of rotation for the display mesh has been changed
    /// </summary>
    public Action<DisplayMesh.RotationAxis> OnMeshRotationAxisChanged;
    /// <summary>
    /// Fired when the axis of scale for the display mesh has been changed
    /// </summary>
    public Action<DisplayMesh.ScaleAxis> OnMeshScaleAxisChanged;

    /// <summary>
    /// Fired when the light temperature slider value has changed
    /// </summary>
    public Action<float> OnLightTemperatureSliderChanged;
    /// <summary>
    /// Fired when the light angle slider value has changed
    /// </summary>
    public Action<float> OnLightAngleSliderChanged;
    /// <summary>
    /// Fired when the light azimuth slider value has changed
    /// </summary>
    public Action<float> OnLightAzimuthSliderChanged;
    /// <summary>
    /// Fired when the light intensity slider value has changed
    /// </summary>
    public Action<float> OnLightIntensitySliderChanged;
    
    /// <summary>
    /// Fired when the bloom effect has been toggled on or off
    /// </summary>
    public Action<bool> OnBloomToggled;
    /// <summary>
    /// Fired when the vignette effect has been toggled on or off
    /// </summary>
    public Action<bool> OnVignetteToggled;
    /// <summary>
    /// Fired when the depth of field effect has been toggled on or off
    /// </summary>
    public Action<bool> OnDepthOfFieldToggled;
    /// <summary>
    /// Fired when the chromatic aberration effect has been toggled on or off
    /// </summary>
    public Action<bool> OnChromaticAberrationToggled;
    /// <summary>
    /// Fired when the film grain effect has been toggled on or off
    /// </summary>
    public Action<bool> OnFilmGrainToggled;
    /// <summary>
    /// Fired when the panini projection effect has been toggled on or off
    /// </summary>
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
    
    private MeshLibraryAsset meshLibrary;
    private MaterialLibraryAsset materialLibrary;
    private TextureLibraryAsset textureLibrary;
    
    public VisualInterfaceController(
        VisualElement rootElement, 
        VisualTreeAsset cardTemplate,
        MeshLibraryAsset meshLibrary,
        MaterialLibraryAsset materialLibrary,
        TextureLibraryAsset textureLibrary) : base(rootElement)
    {
        InitSafeArea();
        
        this.meshLibrary = meshLibrary;
        this.materialLibrary = materialLibrary;
        this.textureLibrary = textureLibrary;
        
        picker = new PickerController(root.Q<VisualElement>(PickerContainerId), cardTemplate);
        picker.AddMeshLibrary(meshLibrary);
        picker.AddMaterialLibrary(materialLibrary);
        picker.AddTextureLibrary(textureLibrary);
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
    
    /// <summary>
    /// Selects the translate control in the controls interface
    /// </summary>
    public void SelectTranslateControl()
    {
        translateControl.AddToClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshControlModeChanged?.Invoke(DisplayMesh.ControlMode.Translate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.Flex;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    /// <summary>
    /// Selects the rotate control in the controls interface
    /// </summary>
    public void SelectRotateControl()
    {
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.AddToClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshControlModeChanged?.Invoke(DisplayMesh.ControlMode.Rotate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.Flex;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    /// <summary>
    /// Selects the scale control in the controls interface
    /// </summary>
    public void SelectScaleControl()
    {
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.AddToClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshControlModeChanged?.Invoke(DisplayMesh.ControlMode.Scale);
        scaleAxis.style.display = DisplayStyle.Flex;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    /// <summary>
    /// Selects the light control in the controls interface
    /// </summary>
    public void SelectLightControl()
    {
        lightControl.AddToClassList(ControlSelectedClass);
        effectControl.RemoveFromClassList(ControlSelectedClass);
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshControlModeChanged?.Invoke(DisplayMesh.ControlMode.None);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.Flex;
        effectControls.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Selects the effect control in the controls interface
    /// </summary>
    public void SelectEffectControl()
    {
        effectControl.AddToClassList(ControlSelectedClass);
        lightControl.RemoveFromClassList(ControlSelectedClass);
        translateControl.RemoveFromClassList(ControlSelectedClass);
        rotateControl.RemoveFromClassList(ControlSelectedClass);
        scaleControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshControlModeChanged?.Invoke(DisplayMesh.ControlMode.None);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.Flex;
    }
    
    /// <summary>
    /// Selects the XZ plane control in the translation controls interface
    /// </summary>
    public void SelectTranslateXZControl()
    {
        translateXZControl.AddToClassList(ControlSelectedClass);
        translateXYControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshTranslationPlaneChanged?.Invoke(DisplayMesh.TranslationAxis.XZ);
    }
    
    /// <summary>
    /// Selects the XY plane control in the translation controls interface
    /// </summary>
    public void SelectTranslateXYControl()
    {
        translateXZControl.RemoveFromClassList(ControlSelectedClass);
        translateXYControl.AddToClassList(ControlSelectedClass);
        OnMeshTranslationPlaneChanged?.Invoke(DisplayMesh.TranslationAxis.XY);
    }
    
    /// <summary>
    /// Selects the X axis control in the rotation controls interface
    /// </summary>
    public void SelectRotateXControl()
    {
        rotateXControl.AddToClassList(ControlSelectedClass);
        rotateYControl.RemoveFromClassList(ControlSelectedClass);
        rotateZControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.X);
    }
    
    /// <summary>
    /// Selects the Y axis control in the rotation controls interface
    /// </summary>
    public void SelectRotateYControl()
    {
        rotateXControl.RemoveFromClassList(ControlSelectedClass);
        rotateYControl.AddToClassList(ControlSelectedClass);
        rotateZControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.Y);
    }
    
    /// <summary>
    /// Selects the Z axis control in the rotation controls interface
    /// </summary>
    public void SelectRotateZControl()
    {
        rotateXControl.RemoveFromClassList(ControlSelectedClass);
        rotateYControl.RemoveFromClassList(ControlSelectedClass);
        rotateZControl.AddToClassList(ControlSelectedClass);
        OnMeshRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.Z);
    }
    
    /// <summary>
    /// Selects the X axis control in the scale controls interface
    /// </summary>
    public void SelectScaleXControl()
    {
        scaleXControl.AddToClassList(ControlSelectedClass);
        scaleYControl.RemoveFromClassList(ControlSelectedClass);
        scaleZControl.RemoveFromClassList(ControlSelectedClass);
        scaleUniformControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.X);
    }
    
    /// <summary>
    /// Selects the Y axis control in the scale controls interface
    /// </summary>
    public void SelectScaleYControl()
    {
        scaleXControl.RemoveFromClassList(ControlSelectedClass);
        scaleYControl.AddToClassList(ControlSelectedClass);
        scaleZControl.RemoveFromClassList(ControlSelectedClass);
        scaleUniformControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Y);
    }
    
    /// <summary>
    /// Selects the Z axis control in the scale controls interface
    /// </summary>
    public void SelectScaleZControl()
    {
        scaleXControl.RemoveFromClassList(ControlSelectedClass);
        scaleYControl.RemoveFromClassList(ControlSelectedClass);
        scaleZControl.AddToClassList(ControlSelectedClass);
        scaleUniformControl.RemoveFromClassList(ControlSelectedClass);
        OnMeshScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Z);
    }
    
    /// <summary>
    /// Selects the uniform axis control in the scale controls interface
    /// </summary>
    public void SelectScaleUniformControl()
    {
        scaleXControl.RemoveFromClassList(ControlSelectedClass);
        scaleYControl.RemoveFromClassList(ControlSelectedClass);
        scaleZControl.RemoveFromClassList(ControlSelectedClass);
        scaleUniformControl.AddToClassList(ControlSelectedClass);
        OnMeshScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Uniform);
    }
    
    /// <summary>
    /// Sets the temperature slider in the lighting controls interface
    /// </summary>
    /// <param name="temperature">slider value between 0 and 1</param>
    public void SetLightTemperature(float temperature)
    {
        temperatureSlider.Value = temperature;
    }
    
    /// <summary>
    /// Sets the light angle slider in the lighting controls interface
    /// </summary>
    /// <param name="angle">slider value between 0 and 1</param>
    public void SetLightAngle(float angle)
    {
        angleSlider.Value = angle;
    }
    
    /// <summary>
    /// Sets the light intensity slider in the lighting controls interface
    /// </summary>
    /// <param name="intensity">slider value between 0 and 1</param>
    public void SetLightIntensity(float intensity)
    {
        intensitySlider.Value = intensity;
    }
    
    /// <summary>
    /// Sets the light azimuth slider in the lighting controls interface
    /// </summary>
    /// <param name="azimuth">slider value between 0 and 1</param>
    public void SetLightAzimuth(float azimuth)
    {
        azimuthSlider.Value = azimuth;
    }
    
    /// <summary>
    /// Toggles the bloom control in the effect controls interface
    /// </summary>
    public void SelectBloomControl()
    {
        bloomControl.ToggleInClassList(ControlSelectedClass);
        OnBloomToggled?.Invoke(bloomControl.ClassListContains(ControlSelectedClass));
    }
    
    /// <summary>
    /// Toggles the vignette control in the effect controls interface
    /// </summary>
    public void SelectVignetteControl()
    {
        vignetteControl.ToggleInClassList(ControlSelectedClass);
        OnVignetteToggled?.Invoke(vignetteControl.ClassListContains(ControlSelectedClass));
    }
    
    /// <summary>
    /// Toggles the depth of field control in the effect controls interface
    /// </summary>
    public void SelectDepthOfFieldControl()
    {
        depthOfFieldControl.ToggleInClassList(ControlSelectedClass);
        OnDepthOfFieldToggled?.Invoke(depthOfFieldControl.ClassListContains(ControlSelectedClass));
    }
    
    /// <summary>
    /// Toggles the chromatic aberration control in the effect controls interface
    /// </summary>
    public void SelectChromaticAberrationControl()
    {
        chromaticAberrationControl.ToggleInClassList(ControlSelectedClass);
        OnChromaticAberrationToggled?.Invoke(chromaticAberrationControl.ClassListContains(ControlSelectedClass));
    }
    
    /// <summary>
    /// Toggles the film grain control in the effect controls interface
    /// </summary>
    public void SelectFilmGrainControl()
    {
        filmGrainControl.ToggleInClassList(ControlSelectedClass);
        OnFilmGrainToggled?.Invoke(filmGrainControl.ClassListContains(ControlSelectedClass));
    }
    
    /// <summary>
    /// Toggles the panini projection control in the effect controls interface
    /// </summary>
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

    // Corrects UI bounds based on the current device safe area
    private void InitSafeArea()
    {
        Vector2 screenTopLeft = new Vector2(Screen.safeArea.xMin, Screen.height - Screen.safeArea.yMax);
        Vector2 screenBottomRight = new Vector2(Screen.width - Screen.safeArea.xMax, Screen.safeArea.yMin);
        Vector2 topLeft = RuntimePanelUtils.ScreenToPanel(root.panel, screenTopLeft);
        Vector2 bottomRight = RuntimePanelUtils.ScreenToPanel(root.panel, screenBottomRight);
        UQueryBuilder<VisualElement> safeAreas = root.Query<VisualElement>(className: "safe-area");
        foreach (VisualElement safeArea in safeAreas.ToList())
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
        OnLightAzimuthSliderChanged?.Invoke(value);
    }
    
    private void OnIntensitySliderValueChanged(float value)
    {
        OnLightIntensitySliderChanged?.Invoke(value);
    }
    
    private void OnAngleSliderValueChanged(float angle)
    {
        OnLightAngleSliderChanged?.Invoke(angle);
    }

    private void OnTemperatureSliderValueChanged(float temp)
    {
        OnLightTemperatureSliderChanged?.Invoke(temp);
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