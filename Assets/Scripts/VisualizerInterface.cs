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
    [SerializeField] private UIDocument userInterface;
    [SerializeField] private VisualTreeAsset carouselCardTemplate;
    [SerializeField] private DisplayMesh displayMesh;

    private VisualInterfaceController rootController;

    private void OnEnable()
    {
        rootController = new VisualInterfaceController(userInterface.rootVisualElement, carouselCardTemplate);
        rootController.OnMeshSelected += OnMeshSelected;
        rootController.OnMaterialSelected += OnMaterialSelected;
        rootController.OnTextureSelected += OnTextureSelected;
    }

    private void OnTextureSelected(Texture2D tex)
    {
        displayMesh.SetTexture(tex);
    }

    private void OnMaterialSelected(Material mat)
    {
        displayMesh.SetMaterial(mat);
    }

    private void OnMeshSelected(Mesh mesh)
    {
        displayMesh.SetMesh(mesh);
    }

    private void Update()
    {
        rootController.Update();
    }

    private void Reset()
    {
        userInterface = GetComponent<UIDocument>();
    }
}