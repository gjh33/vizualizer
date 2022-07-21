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
        InitDefaults();
        RegisterCallbacks();
    }

    private void Update()
    {
        rootController.Update();
    }

    private void Reset()
    {
        userInterface = GetComponent<UIDocument>();
    }

    private void RegisterCallbacks()
    {
        rootController.OnMeshSelected += OnMeshSelected;
        rootController.OnMaterialSelected += OnMaterialSelected;
        rootController.OnTextureSelected += OnTextureSelected;
        rootController.OnControlModeChanged += OnControlModeChanged;
        rootController.OnTranslationAxisChanged += OnTranslationAxisChanged;
        rootController.OnRotationAxisChanged += OnRotationAxisChanged;
        rootController.OnScaleAxisChanged += OnScaleAxisChanged;
        rootController.OnPickerVisibilityChanged += OnPickerVisibilityChanged;
    }

    private void OnPickerVisibilityChanged(bool visible)
    {
        displayMesh.ControlsEnabled = !visible;
    }

    private void InitDefaults()
    {
        switch (displayMesh.CurrentControlMode)
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

        switch (displayMesh.CurrentTranslationAxis)
        {
            case DisplayMesh.TranslationAxis.XZ:
                rootController.SelectTranslateXZControl();
                break;
            case DisplayMesh.TranslationAxis.XY:
                rootController.SelectTranslateXYControl();
                break;
        }
        
        switch (displayMesh.CurrentRotationAxis)
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
        
        switch (displayMesh.CurrentScaleAxis)
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
    }

    private void OnScaleAxisChanged(DisplayMesh.ScaleAxis axis)
    {
        displayMesh.CurrentScaleAxis = axis;
    }

    private void OnRotationAxisChanged(DisplayMesh.RotationAxis axis)
    {
        displayMesh.CurrentRotationAxis = axis;
    }

    private void OnTranslationAxisChanged(DisplayMesh.TranslationAxis axis)
    {
        displayMesh.CurrentTranslationAxis = axis;
    }

    private void OnControlModeChanged(DisplayMesh.ControlMode mode)
    {
        displayMesh.CurrentControlMode = mode;
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
}