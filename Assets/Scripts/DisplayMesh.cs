using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays a mesh with a material and texture.
/// </summary>
public class DisplayMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter Filter;
    [SerializeField] private MeshRenderer Renderer;
    
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

    // Attempt to resolve dependencies on create or reset
    private void Reset()
    {
        Filter = GetComponent<MeshFilter>();
        Renderer = GetComponent<MeshRenderer>();
    }
}
