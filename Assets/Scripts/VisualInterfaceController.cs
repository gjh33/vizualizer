using System.Collections;
using Unity.VisualScripting;
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
    
    private VisualTreeAsset carouselCardTemplate;
    private PickerController picker;

    private Button selectMeshButton;
    private Button selectMaterialButton;
    private Button selectTextureButton;

    
    public VisualInterfaceController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carouselCardTemplate = cardTemplate;
        
        picker = new PickerController(root.Q<VisualElement>(pickerContainerId), carouselCardTemplate);
    }

    public void Update()
    {
        picker.Update();
    }

    protected override void CollectElements()
    {
        base.CollectElements();
        
        selectMeshButton = root.Q<Button>(selectMeshButtonId);
        selectMaterialButton = root.Q<Button>(selectMaterialButtonId);
        selectTextureButton = root.Q<Button>(selectTextureButtonId);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        
        selectMeshButton.clickable.clicked += OnSelectMeshButtonClicked;
        selectMaterialButton.clickable.clicked += OnSelectMaterialButtonClicked;
        selectTextureButton.clickable.clicked += OnSelectTextureButtonClicked;
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