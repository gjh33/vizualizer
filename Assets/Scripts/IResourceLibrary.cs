using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IResourceLibrary<T> where T : Object
{
    /// <summary>
    /// Load a resource from the library
    /// </summary>
    /// <param name="name">the name of the resource</param>
    /// <returns>the resource if found, null otherwise</returns>
    T LoadResource(string name);
    
    /// <summary>
    /// Load all resource in the library
    /// </summary>
    /// <returns>an enumerable of all resources</returns>
    IEnumerable<T> LoadAllResources();
    
    /// <summary>
    /// Load an image preview of a resource from the library
    /// </summary>
    /// <param name="name">the name of the resource the preview belongs to</param>
    /// <returns>the preview image if found, null otherwise</returns>
    Texture2D LoadResourcePreview(string name);
}