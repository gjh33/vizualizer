using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickerController : UIController
{
    private const string PickMeshTitle = "Select Mesh";
    private const string PickMaterialTitle = "Select Material";
    private const string PickTextureTitle = "Select Texture";
    private const string MeshResourcePath = "Meshes";
    private const string MaterialResourcePath = "Materials";
    private const string TextureResourcePath = "Textures";
    private const string IconRootPath = "Preview Images";
    private const string carouselId = "carousel";
    
    public Action<Mesh> OnMeshSelected;
    public Action<Material> OnMaterialSelected;
    public Action<Texture2D> OnTextureSelected;
    
    private VisualTreeAsset carouselCardTemplate;
    private CarouselController carousel;

    private Label titleLabel;

    public PickerController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carouselCardTemplate = cardTemplate;
        
        carousel = new CarouselController(root.Q<VisualElement>(carouselId), carouselCardTemplate);
        carousel.ItemSelected += OnItemSelected;
    }

    public void Update()
    {
        carousel.Update();
    }

    public void PickMesh()
    {
        titleLabel.text = PickMeshTitle;
        carousel.SetItems(EnumerateItems<Mesh>(MeshResourcePath));
        Show();
    }
    
    public void PickMaterial()
    {
        titleLabel.text = PickMaterialTitle;
        carousel.SetItems(EnumerateItems<Material>(MaterialResourcePath));
        Show();
    }
    
    public void PickTexture()
    {
        titleLabel.text = PickTextureTitle;
        carousel.SetItems(EnumerateItems<Texture2D>(TextureResourcePath));
        Show();
    }

    protected override void CollectElements()
    {
        base.CollectElements();
        
        titleLabel = root.Q<Label>("title-label");
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