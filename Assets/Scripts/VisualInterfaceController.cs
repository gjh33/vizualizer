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
    private const string translateAxisId = "translate-axis";
    private const string rotationAxisId = "rotation-axis";
    private const string scaleAxisId = "scale-axis";
    private const string xzControlId = "xz-control";
    private const string xyControlId = "xy-control";
    private const string xControlId = "x-control";
    private const string yControlId = "y-control";
    private const string zControlId = "z-control";
    private const string uniformControlId = "uniform-control";

    private const string controlSelectedClass = "selected";
    
    public Action<Mesh> OnMeshSelected;
    public Action<Material> OnMaterialSelected;
    public Action<Texture2D> OnTextureSelected;
    
    public Action<DisplayMesh.ControlMode> OnControlModeChanged;
    public Action<DisplayMesh.TranslationAxis> OnTranslationAxisChanged;
    public Action<DisplayMesh.RotationAxis> OnRotationAxisChanged;
    public Action<DisplayMesh.ScaleAxis> OnScaleAxisChanged;

    public Action<bool> OnPickerVisibilityChanged;
    
    private VisualTreeAsset carouselCardTemplate;
    private PickerController picker;

    private Button selectMeshButton;
    private Button selectMaterialButton;
    private Button selectTextureButton;
    private Button translateControl;
    private Button rotateControl;
    private Button scaleControl;
    private Button translateXZControl;
    private Button translateXYControl;
    private Button rotateXControl;
    private Button rotateYControl;
    private Button rotateZControl;
    private Button scaleXControl;
    private Button scaleYControl;
    private Button scaleZControl;
    private Button scaleUniformControl;
    
    private VisualElement translateAxis;
    private VisualElement rotateAxis;
    private VisualElement scaleAxis;
    
    public VisualInterfaceController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carouselCardTemplate = cardTemplate;
        
        picker = new PickerController(root.Q<VisualElement>(pickerContainerId), carouselCardTemplate);
        picker.OnMeshSelected += OnPickerMeshSelected;
        picker.OnMaterialSelected += OnPickerMaterialSelected;
        picker.OnTextureSelected += OnPickerTextureSelected;
    }

    private void OnPickerTextureSelected(Texture2D tex)
    {
        OnTextureSelected?.Invoke(tex);
        OnPickerVisibilityChanged?.Invoke(false);
    }

    private void OnPickerMaterialSelected(Material mat)
    {
        OnMaterialSelected?.Invoke(mat);
        OnPickerVisibilityChanged?.Invoke(false);
    }

    private void OnPickerMeshSelected(Mesh mesh)
    {
        OnMeshSelected?.Invoke(mesh);
        OnPickerVisibilityChanged?.Invoke(false);
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
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Translate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.Flex;
        rotateAxis.style.display = DisplayStyle.None;
    }
    
    public void SelectRotateControl()
    {
        translateControl.RemoveFromClassList(controlSelectedClass);
        rotateControl.AddToClassList(controlSelectedClass);
        scaleControl.RemoveFromClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Rotate);
        scaleAxis.style.display = DisplayStyle.None;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.Flex;
    }
    
    public void SelectScaleControl()
    {
        translateControl.RemoveFromClassList(controlSelectedClass);
        rotateControl.RemoveFromClassList(controlSelectedClass);
        scaleControl.AddToClassList(controlSelectedClass);
        OnControlModeChanged?.Invoke(DisplayMesh.ControlMode.Scale);
        scaleAxis.style.display = DisplayStyle.Flex;
        translateAxis.style.display = DisplayStyle.None;
        rotateAxis.style.display = DisplayStyle.None;
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

    protected override void CollectElements()
    {
        base.CollectElements();
        
        selectMeshButton = root.Q<Button>(selectMeshButtonId);
        selectMaterialButton = root.Q<Button>(selectMaterialButtonId);
        selectTextureButton = root.Q<Button>(selectTextureButtonId);
        translateControl = root.Q<Button>(translateControlId);
        rotateControl = root.Q<Button>(rotateControlId);
        scaleControl = root.Q<Button>(scaleControlId);
        
        translateAxis = root.Q<VisualElement>(translateAxisId);
        rotateAxis = root.Q<VisualElement>(rotationAxisId);
        scaleAxis = root.Q<VisualElement>(scaleAxisId);
        
        translateXZControl = translateAxis.Q<Button>(xzControlId);
        translateXYControl = translateAxis.Q<Button>(xyControlId);
        rotateXControl = rotateAxis.Q<Button>(xControlId);
        rotateYControl = rotateAxis.Q<Button>(yControlId);
        rotateZControl = rotateAxis.Q<Button>(zControlId);
        scaleXControl = scaleAxis.Q<Button>(xControlId);
        scaleYControl = scaleAxis.Q<Button>(yControlId);
        scaleZControl = scaleAxis.Q<Button>(zControlId);
        scaleUniformControl = scaleAxis.Q<Button>(uniformControlId);
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
        
        translateXYControl.clickable.clicked += OnTranslateXYControlClicked;
        translateXZControl.clickable.clicked += OnTranslateXZControlClicked;
        rotateXControl.clickable.clicked += OnRotateXControlClicked;
        rotateYControl.clickable.clicked += OnRotateYControlClicked;
        rotateZControl.clickable.clicked += OnRotateZControlClicked;
        scaleXControl.clickable.clicked += OnScaleXControlClicked;
        scaleYControl.clickable.clicked += OnScaleYControlClicked;
        scaleZControl.clickable.clicked += OnScaleZControlClicked;
        scaleUniformControl.clickable.clicked += OnScaleUniformControlClicked;
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
        OnPickerVisibilityChanged?.Invoke(true);
    }

    private void OnSelectMaterialButtonClicked()
    {
        picker.PickMaterial();
        OnPickerVisibilityChanged?.Invoke(true);
    }

    private void OnSelectTextureButtonClicked()
    {
        picker.PickTexture();
        OnPickerVisibilityChanged?.Invoke(true);
    }
}