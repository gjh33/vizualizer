using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controller for the picker UI that allows the user to choose between various assets
/// </summary>
public class PickerController : UIController
{
    private const string PickMeshTitle = "Select Mesh";
    private const string PickMaterialTitle = "Select Material";
    private const string PickTextureTitle = "Select Texture";
    private const string MeshResourcePath = "Meshes";
    private const string MaterialResourcePath = "Materials";
    private const string TextureResourcePath = "Textures";
    private const string IconRootPath = "Preview Images";
    private const string CarouselId = "carousel";
    private const string CloseButtonId = "close-button";
    private const string TitleLabelId = "title-label";
    
    /// <summary>
    /// Fired when the user selects a mesh asset
    /// </summary>
    public Action<Mesh> OnMeshSelected;
    /// <summary>
    /// Fired when the user selects a material asset
    /// </summary>
    public Action<Material> OnMaterialSelected;
    /// <summary>
    /// Fired when the user selects a texture asset
    /// </summary>
    public Action<Texture2D> OnTextureSelected;
    
    private CarouselController carousel;

    private Label titleLabel;
    private Button closeButton;

    public PickerController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carousel = new CarouselController(root.Q<VisualElement>(CarouselId), cardTemplate);
        carousel.ItemSelected += OnItemSelected;
    }

    public void Update()
    {
        carousel.Update();
    }

    /// <summary>
    /// Open the picker UI and display a list of meshes
    /// </summary>
    public void PickMesh()
    {
        titleLabel.text = PickMeshTitle;
        carousel.SetItems(EnumerateItems<Mesh>(MeshResourcePath));
        Show();
    }
    
    /// <summary>
    /// Open the picker UI and display a list of materials
    /// </summary>
    public void PickMaterial()
    {
        titleLabel.text = PickMaterialTitle;
        carousel.SetItems(EnumerateItems<Material>(MaterialResourcePath));
        Show();
    }
    
    /// <summary>
    /// Open the picker UI and display a list of textures
    /// </summary>
    public void PickTexture()
    {
        titleLabel.text = PickTextureTitle;
        carousel.SetItems(EnumerateItems<Texture2D>(TextureResourcePath));
        Show();
    }

    protected override void CollectElements()
    {
        base.CollectElements();
        
        titleLabel = root.Q<Label>(TitleLabelId);
        closeButton = root.Q<Button>(CloseButtonId);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        closeButton.clicked += OnCloseButtonClicked;
    }

    private void OnCloseButtonClicked()
    {
        Hide();
    }

    private void Show()
    {
        root.style.display = DisplayStyle.Flex;
    }

    private void Hide()
    {
        root.style.display = DisplayStyle.None;
    }

    private void OnItemSelected(CarouselItem item)
    {
        switch (item.Value)
        {
            case Mesh mesh:
                OnMeshSelected?.Invoke(mesh);
                break;
            case Material mat:
                OnMaterialSelected?.Invoke(mat);
                break;
            case Texture2D tex:
                OnTextureSelected?.Invoke(tex);
                break;
        }
        Hide();
    }

    // Loads assets into CarouselItem objects. Right now graphics are done by convention not configuration
    // In the future this should be configurable to allow user uploaded content
    private IEnumerable<CarouselItem> EnumerateItems<T>(string resourcePath)
        where T : UnityEngine.Object
    {
        T[] assets = Resources.LoadAll<T>(resourcePath);
        foreach (T asset in assets)
        {
            Texture2D graphic = Resources.Load<Texture2D>($"{IconRootPath}/{resourcePath}/{asset.name}");
            yield return new CarouselItem()
            {
                Title = asset.name,
                Value = asset,
                Graphic = graphic
            };
        }
    }
}