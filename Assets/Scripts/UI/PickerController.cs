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

    private List<IResourceLibrary<Mesh>> meshLibraries = new List<IResourceLibrary<Mesh>>();
    private List<IResourceLibrary<Material>> materialLibraries = new List<IResourceLibrary<Material>>();
    private List<IResourceLibrary<Texture2D>> textureLibraries = new List<IResourceLibrary<Texture2D>>();

    public PickerController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carousel = new CarouselController(root.Q<VisualElement>(CarouselId), cardTemplate);
        carousel.ItemSelected += OnItemSelected;
    }

    public void Update()
    {
        carousel.Update();
    }
    
    public void AddMeshLibrary(IResourceLibrary<Mesh> library)
    {
        meshLibraries.Add(library);
    }
    
    public void RemoveMeshLibrary(IResourceLibrary<Mesh> library)
    {
        meshLibraries.Remove(library);
    }
    
    public void ClearMeshLibraries()
    {
        meshLibraries.Clear();
    }
    
    public void AddMaterialLibrary(IResourceLibrary<Material> library)
    {
        materialLibraries.Add(library);
    }
    
    public void RemoveMaterialLibrary(IResourceLibrary<Material> library)
    {
        materialLibraries.Remove(library);
    }
    
    public void ClearMaterialLibraries()
    {
        materialLibraries.Clear();
    }
    
    public void AddTextureLibrary(IResourceLibrary<Texture2D> library)
    {
        textureLibraries.Add(library);
    }
    
    public void RemoveTextureLibrary(IResourceLibrary<Texture2D> library)
    {
        textureLibraries.Remove(library);
    }
    
    public void ClearTextureLibraries()
    {
        textureLibraries.Clear();
    }
    
    public void ClearAll()
    {
        ClearMeshLibraries();
        ClearMaterialLibraries();
        ClearTextureLibraries();
    }

    /// <summary>
    /// Open the picker UI and display a list of meshes
    /// </summary>
    public void PickMesh()
    {
        titleLabel.text = PickMeshTitle;
        List<CarouselItem> meshes = new List<CarouselItem>();
        foreach (IResourceLibrary<Mesh> library in meshLibraries)
        {
            meshes.AddRange(EnumerateItems(library));
        }
        carousel.SetItems(meshes);
        Show();
    }
    
    /// <summary>
    /// Open the picker UI and display a list of materials
    /// </summary>
    public void PickMaterial()
    {
        titleLabel.text = PickMaterialTitle;
        List<CarouselItem> materials = new List<CarouselItem>();
        foreach (IResourceLibrary<Material> library in materialLibraries)
        {
            materials.AddRange(EnumerateItems(library));
        }
        carousel.SetItems(materials);
        Show();
    }
    
    /// <summary>
    /// Open the picker UI and display a list of textures
    /// </summary>
    public void PickTexture()
    {
        titleLabel.text = PickTextureTitle;
        List<CarouselItem> textures = new List<CarouselItem>();
        foreach (IResourceLibrary<Texture2D> library in textureLibraries)
        {
            textures.AddRange(EnumerateItems(library));
        }
        carousel.SetItems(textures);
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
    private IEnumerable<CarouselItem> EnumerateItems<T>(IResourceLibrary<T> library)
        where T : UnityEngine.Object
    {
        foreach (T asset in library.LoadAllResources())
        {
            Texture2D graphic = library.LoadResourcePreview(asset.name);
            yield return new CarouselItem()
            {
                Title = asset.name,
                Value = asset,
                Graphic = graphic
            };
        }
    }
}