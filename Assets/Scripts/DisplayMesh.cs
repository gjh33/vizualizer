using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays a mesh with a material and texture.
/// </summary>
public class DisplayMesh : MonoBehaviour
{
    public enum ControlMode { Translate, Rotate, Scale };
    public enum TranslationAxis { XY, XZ };
    public enum RotationAxis { X, Y, Z };
    public enum ScaleAxis { X, Y, Z, Uniform };
    
    [SerializeField] private MeshFilter Filter;
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private float TranslationSensitivity = 0.01f;
    [SerializeField] private float RotationSensitivity = 0.5f;
    [SerializeField] private float ScaleSensitivity = 0.01f;

    public ControlMode CurrentControlMode = ControlMode.Translate;
    public TranslationAxis CurrentTranslationAxis = TranslationAxis.XZ;
    public RotationAxis CurrentRotationAxis = RotationAxis.Y;
    public ScaleAxis CurrentScaleAxis = ScaleAxis.Uniform;

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
        Renderer.material = material;
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
        if (!ControlsEnabled)
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
                if (CurrentTranslationAxis == TranslationAxis.XZ)
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
