using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controls mesh properties with user input and API.
/// </summary>
public class DisplayMesh : MonoBehaviour
{
    public enum ControlMode { Translate, Rotate, Scale, None };
    public enum TranslationAxis { XY, XZ };
    public enum RotationAxis { X, Y, Z };
    public enum ScaleAxis { X, Y, Z, Uniform };
    
    [Tooltip("The mesh filter used to set the displayed mesh")]
    [SerializeField] private MeshFilter Filter;
    [Tooltip("The mesh renderer used to set the displayed material")]
    [SerializeField] private MeshRenderer Renderer;
    [Tooltip("The factor by which user input is scaled to world units when translating")]
    [SerializeField] private float TranslationSensitivity = 0.01f;
    [Tooltip("The factor by which user input is scaled to degrees when rotating")]
    [SerializeField] private float RotationSensitivity = 0.5f;
    [Tooltip("The factor by which user input is scaled a multiplier when scaling")]
    [SerializeField] private float ScaleSensitivity = 0.01f;

    /// <summary>
    /// Determines how the mesh is manipulated by user input
    /// </summary>
    public ControlMode CurrentControlMode = ControlMode.Translate;
    
    /// <summary>
    /// When in translation mode, determines the plane along which the mesh is translated
    /// </summary>
    public TranslationAxis CurrentTranslationPlane = TranslationAxis.XZ;
    
    /// <summary>
    /// When in rotation mode, determines the axis around which the mesh is rotated
    /// </summary>
    public RotationAxis CurrentRotationAxis = RotationAxis.Y;
    
    /// <summary>
    /// When in scale mode, determines the axis the mesh is scaled along
    /// </summary>
    public ScaleAxis CurrentScaleAxis = ScaleAxis.Uniform;

    /// <summary>
    /// Determines whether or not user input is received to control the mesh
    /// </summary>
    public bool ControlsEnabled = true;
    
    private VisualizerControls input;
    private bool dragging = false;

    /// <summary>
    /// Sets the mesh currently being displayed
    /// </summary>
    /// <param name="mesh">The mesh to display</param>
    public void SetMesh(Mesh mesh)
    {
        Filter.mesh = mesh;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.up;
    }
    
    /// <summary>
    /// Sets the material for the currently displayed mesh
    /// </summary>
    /// <param name="material">The material to apply</param>
    public void SetMaterial(Material material)
    {
        Texture2D currentTexture = Renderer.material.mainTexture as Texture2D;
        Renderer.material = material;
        Renderer.material.mainTexture = currentTexture;
    }
    
    /// <summary>
    /// Sets the texture for the material of the currently displayed mesh
    /// </summary>
    /// <param name="texture">The texture to set</param>
    public void SetTexture(Texture2D texture)
    {
        Renderer.material.mainTexture = texture;
    }

    private void OnEnable()
    {
        input = new VisualizerControls();
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        if (!ControlsEnabled || EventSystem.current.IsPointerOverGameObject())
        {
            dragging = false;
            return;
        }
        
        if (input.ModelManipulation.DragActive.WasPressedThisFrame())
        {
            dragging = true;
        }
        if (input.ModelManipulation.DragActive.WasReleasedThisFrame())
        {
            dragging = false;
        }
        if (dragging)
        {
            Vector2 delta = input.ModelManipulation.MoveDrag.ReadValue<Vector2>();
            
            if (CurrentControlMode == ControlMode.Translate)
            {
                delta *= TranslationSensitivity;
                if (CurrentTranslationPlane == TranslationAxis.XZ)
                {
                    transform.Translate(new Vector3(delta.x, 0, delta.y), Space.World);
                }
                else
                {
                    transform.Translate(new Vector3(delta.x, delta.y, 0), Space.World);
                }
            }
            else if (CurrentControlMode == ControlMode.Rotate)
            {
                delta *= RotationSensitivity;
                if (CurrentRotationAxis == RotationAxis.X)
                {
                    transform.Rotate(new Vector3(delta.y, 0, 0), Space.World);
                }
                else if (CurrentRotationAxis == RotationAxis.Y)
                {
                    transform.Rotate(new Vector3(0, -delta.x, 0), Space.World);
                }
                else if (CurrentRotationAxis == RotationAxis.Z)
                {
                    transform.Rotate(new Vector3(0, 0, -delta.x), Space.World);
                }
            }
            else if (CurrentControlMode == ControlMode.Scale)
            {
                delta *= ScaleSensitivity;

                if (CurrentScaleAxis == ScaleAxis.X)
                {
                    transform.localScale += Vector3.right * delta.y;
                }
                else if (CurrentScaleAxis == ScaleAxis.Y)
                {
                    transform.localScale += Vector3.up * delta.y;
                }
                else if (CurrentScaleAxis == ScaleAxis.Z)
                {
                    transform.localScale += Vector3.forward * delta.y;
                }
                else if (CurrentScaleAxis == ScaleAxis.Uniform)
                {
                    transform.localScale += Vector3.one * delta.y;
                }
            }
        }
    }

    // Attempt to resolve dependencies on create or reset
    private void Reset()
    {
        Filter = GetComponent<MeshFilter>();
        Renderer = GetComponent<MeshRenderer>();
    }
}
