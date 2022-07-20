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

    private VisualInterfaceController rootController;

    private void OnEnable()
    {
        rootController = new VisualInterfaceController(userInterface.rootVisualElement, carouselCardTemplate);
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