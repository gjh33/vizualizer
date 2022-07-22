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
    private const string pickerContainerId = "picker-container";
    private const string selectMeshButtonId = "select-mesh-button";
    private const string selectMaterialButtonId = "select-material-button";
    private const string selectTextureButtonId = "select-texture-button";
    private const string translateControlId = "translate-control";
    private const string rotateControlId = "rotate-control";
    private const string scaleControlId = "scale-control";
    private const string lightControlId = "light-control";
    private const string effectControlId = "effect-control";
    private const string translateAxisId = "translate-axis";
    private const string rotationAxisId = "rotation-axis";
    private const string scaleAxisId = "scale-axis";
    private const string xzControlId = "xz-control";
    private const string xyControlId = "xy-control";
    private const string xControlId = "x-control";
    private const string yControlId = "y-control";
    private const string zControlId = "z-control";
    private const string uniformControlId = "uniform-control";
    private const string lightingControlsId = "lighting-controls";
    private const string temperatureSliderId = "temperature-slider";
    private const string angleSliderId = "angle-slider";
    private const string azimuthSliderId = "azimuth-slider";
    private const string intensitySliderId = "intensity-slider";
    private const string effectControlsId = "effect-controls";
    private const string bloomControlId = "bloom-control";
    private const string vignetteControlId = "vignette-control";
    private const string depthOfFieldControlId = "depth-of-field-control";
    private const string chromaticAberrationControlId = "chromatic-aberration-control";
    private const string filmGrainControlId = "film-grain-control";
    private const string paniniProjectionControlId = "panini-projection-control";

    private const string controlSelectedClass = "selected";
    
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
    
    private VisualTreeAsset carouselCardTemplate;
    private PickerController picker;
    private VerticalSliderController temperatureSlider;
    private VerticalSliderController angleSlider;
    private VerticalSliderController azimuthSlider;
    private VerticalSliderController intensitySlider;
    
    public VisualInterfaceController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carouselCardTemplate = cardTemplate;
        
        picker = new PickerController(root.Q<VisualElement>(pickerContainerId), carouselCardTemplate);
        picker.OnMeshSelected += OnPickerMeshSelected;
        picker.OnMaterialSelected += OnPickerMaterialSelected;
        picker.OnTextureSelected += OnPickerTextureSelected;
        
        temperatureSlider = new VerticalSliderController(root.Q<VisualElement>(temperatureSliderId));
        temperatureSlider.OnValueChanged += OnTemperatureSliderValueChanged;
        
        angleSlider = new VerticalSliderController(root.Q<VisualElement>(angleSliderId));
        angleSlider.OnValueChanged += OnAngleSliderValueChanged;
        
        azimuthSlider = new VerticalSliderController(root.Q<VisualElement>(azimuthSliderId));
        azimuthSlider.OnValueChanged += OnAzimuthSliderValueChanged;
        
        intensitySlider = new VerticalSliderController(root.Q<VisualElement>(intensitySliderId));
        intensitySlider.OnValueChanged += OnIntensitySliderValueChanged;
    }

    public void Update()
    {
        picker.Update();
    }
    
    public void SelectTranslateControl()
    {
        translateControl.AddToClassList(controlSelectedClass);
        rotateControl.RemoveFromClassList(controlSelectedClass);
        scaleControl.RemoveFromClassList(controlSelectedClass);
        lightControl.RemoveFromClassList(controlSelectedClass);
        effectControl.RemoveFromClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Translate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.Flex;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    public void SelectRotateControl()
    {
        translateControl.RemoveFromClassList(controlSelectedClass);
        rotateControl.AddToClassList(controlSelectedClass);
        scaleControl.RemoveFromClassList(controlSelectedClass);
        lightControl.RemoveFromClassList(controlSelectedClass);
        effectControl.RemoveFromClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Rotate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.Flex;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    public void SelectScaleControl()
    {
        translateControl.RemoveFromClassList(controlSelectedClass);
        rotateControl.RemoveFromClassList(controlSelectedClass);
        scaleControl.AddToClassList(controlSelectedClass);
        lightControl.RemoveFromClassList(controlSelectedClass);
        effectControl.RemoveFromClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Scale);
        scaleAxis.style.display = DisplayStyle.Flex;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.None;
    }
    
    public void SelectLightControl()
    {
        lightControl.AddToClassList(controlSelectedClass);
        effectControl.RemoveFromClassList(controlSelectedClass);
        translateControl.RemoveFromClassList(controlSelectedClass);
        rotateControl.RemoveFromClassList(controlSelectedClass);
        scaleControl.RemoveFromClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.None);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.Flex;
        effectControls.style.display = DisplayStyle.None;
    }

    public void SelectEffectControl()
    {
        effectControl.AddToClassList(controlSelectedClass);
        lightControl.RemoveFromClassList(controlSelectedClass);
        translateControl.RemoveFromClassList(controlSelectedClass);
        rotateControl.RemoveFromClassList(controlSelectedClass);
        scaleControl.RemoveFromClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.None);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
        lightingControls.style.display = DisplayStyle.None;
        effectControls.style.display = DisplayStyle.Flex;
    }
    
    public void SelectTranslateXZControl()
    {
        translateXZControl.AddToClassList(controlSelectedClass);
        translateXYControl.RemoveFromClassList(controlSelectedClass);
        OnTranslationAxisChanged?.Invoke(DisplayMesh.TranslationAxis.XZ);
    }
    
    public void SelectTranslateXYControl()
    {
        translateXZControl.RemoveFromClassList(controlSelectedClass);
        translateXYControl.AddToClassList(controlSelectedClass);
        OnTranslationAxisChanged?.Invoke(DisplayMesh.TranslationAxis.XY);
    }
    
    public void SelectRotateXControl()
    {
        rotateXControl.AddToClassList(controlSelectedClass);
        rotateYControl.RemoveFromClassList(controlSelectedClass);
        rotateZControl.RemoveFromClassList(controlSelectedClass);
        OnRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.X);
    }
    
    public void SelectRotateYControl()
    {
        rotateXControl.RemoveFromClassList(controlSelectedClass);
        rotateYControl.AddToClassList(controlSelectedClass);
        rotateZControl.RemoveFromClassList(controlSelectedClass);
        OnRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.Y);
    }
    
    public void SelectRotateZControl()
    {
        rotateXControl.RemoveFromClassList(controlSelectedClass);
        rotateYControl.RemoveFromClassList(controlSelectedClass);
        rotateZControl.AddToClassList(controlSelectedClass);
        OnRotationAxisChanged?.Invoke(DisplayMesh.RotationAxis.Z);
    }
    
    public void SelectScaleXControl()
    {
        scaleXControl.AddToClassList(controlSelectedClass);
        scaleYControl.RemoveFromClassList(controlSelectedClass);
        scaleZControl.RemoveFromClassList(controlSelectedClass);
        scaleUniformControl.RemoveFromClassList(controlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.X);
    }
    
    public void SelectScaleYControl()
    {
        scaleXControl.RemoveFromClassList(controlSelectedClass);
        scaleYControl.AddToClassList(controlSelectedClass);
        scaleZControl.RemoveFromClassList(controlSelectedClass);
        scaleUniformControl.RemoveFromClassList(controlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Y);
    }
    
    public void SelectScaleZControl()
    {
        scaleXControl.RemoveFromClassList(controlSelectedClass);
        scaleYControl.RemoveFromClassList(controlSelectedClass);
        scaleZControl.AddToClassList(controlSelectedClass);
        scaleUniformControl.RemoveFromClassList(controlSelectedClass);
        OnScaleAxisChanged?.Invoke(DisplayMesh.ScaleAxis.Z);
    }
    
    public void SelectScaleUniformControl()
    {
        scaleXControl.RemoveFromClassList(controlSelectedClass);
        scaleYControl.RemoveFromClassList(controlSelectedClass);
        scaleZControl.RemoveFromClassList(controlSelectedClass);
        scaleUniformControl.AddToClassList(controlSelectedClass);
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
        bloomControl.ToggleInClassList(controlSelectedClass);
        OnBloomToggled?.Invoke(bloomControl.ClassListContains(controlSelectedClass));
    }
    
    public void SelectVignetteControl()
    {
        vignetteControl.ToggleInClassList(controlSelectedClass);
        OnVignetteToggled?.Invoke(vignetteControl.ClassListContains(controlSelectedClass));
    }
    
    public void SelectDepthOfFieldControl()
    {
        depthOfFieldControl.ToggleInClassList(controlSelectedClass);
        OnDepthOfFieldToggled?.Invoke(depthOfFieldControl.ClassListContains(controlSelectedClass));
    }
    
    public void SelectChromaticAberrationControl()
    {
        chromaticAberrationControl.ToggleInClassList(controlSelectedClass);
        OnChromaticAberrationToggled?.Invoke(chromaticAberrationControl.ClassListContains(controlSelectedClass));
    }
    
    public void SelectFilmGrainControl()
    {
        filmGrainControl.ToggleInClassList(controlSelectedClass);
        OnFilmGrainToggled?.Invoke(filmGrainControl.ClassListContains(controlSelectedClass));
    }
    
    public void SelectPaniniProjectionControl()
    {
        paniniProjectionControl.ToggleInClassList(controlSelectedClass);
        OnPaniniProjectionToggled?.Invoke(paniniProjectionControl.ClassListContains(controlSelectedClass));
    }
    
    protected override void CollectElements()
    {
        base.CollectElements();
        
        selectMeshButton = root.Q<Button>(selectMeshButtonId);
        selectMaterialButton = root.Q<Button>(selectMaterialButtonId);
        selectTextureButton = root.Q<Button>(selectTextureButtonId);
        translateControl = root.Q<Button>(translateControlId);
        rotateControl = root.Q<Button>(rotateControlId);
        scaleControl = root.Q<Button>(scaleControlId);
        lightControl = root.Q<Button>(lightControlId);
        effectControl = root.Q<Button>(effectControlId);
        
        translateAxis = root.Q<VisualElement>(translateAxisId);
        rotateAxis = root.Q<VisualElement>(rotationAxisId);
        scaleAxis = root.Q<VisualElement>(scaleAxisId);
        lightingControls = root.Q<VisualElement>(lightingControlsId);
        effectControls = root.Q<VisualElement>(effectControlsId);
        
        translateXZControl = translateAxis.Q<Button>(xzControlId);
        translateXYControl = translateAxis.Q<Button>(xyControlId);
        rotateXControl = rotateAxis.Q<Button>(xControlId);
        rotateYControl = rotateAxis.Q<Button>(yControlId);
        rotateZControl = rotateAxis.Q<Button>(zControlId);
        scaleXControl = scaleAxis.Q<Button>(xControlId);
        scaleYControl = scaleAxis.Q<Button>(yControlId);
        scaleZControl = scaleAxis.Q<Button>(zControlId);
        scaleUniformControl = scaleAxis.Q<Button>(uniformControlId);
        
        bloomControl = effectControls.Q<Button>(bloomControlId);
        vignetteControl = effectControls.Q<Button>(vignetteControlId);
        depthOfFieldControl = effectControls.Q<Button>(depthOfFieldControlId);
        chromaticAberrationControl = effectControls.Q<Button>(chromaticAberrationControlId);
        filmGrainControl = effectControls.Q<Button>(filmGrainControlId);
        paniniProjectionControl = effectControls.Q<Button>(paniniProjectionControlId);
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