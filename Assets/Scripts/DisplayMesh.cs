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
    
    [SerializeField] private MeshFilter Filter;
    [SerializeField] private MeshRenderer Renderer;

    public ControlMode CurrentControlMode = ControlMode.Translate;
    
    private VisualizerControls input;
    private bool dragging = false;

    /// <summary>
    /// Sets the mesh currently being displayed
    /// </summary>
    /// <param name="mesh">The mesh to display</param>
    public void SetMesh(Mesh mesh)
    {
        Filter.mesh = mesh;
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
                transform.position += new Vector3(delta.x * 0.01f, 0, delta.y * 0.01f);
            }
            else if (CurrentControlMode == ControlMode.Rotate)
            {
                transform.Rotate(new Vector3(delta.y * 0.5f, -delta.x * 0.5f, 0), Space.World);
            }
            else if (CurrentControlMode == ControlMode.Scale)
            {
                transform.localScale += Vector3.one * delta.y * 0.01f;
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
