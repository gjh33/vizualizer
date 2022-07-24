using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// A resource library for built-in resources stored as assets.
/// A dictionary cache is used for faster lookup
/// </summary>
/// <typeparam name="T">The resource stored in this asset</typeparam>
public class ResourceLibraryAsset<T> : ScriptableObject, IResourceLibrary<T> where T : Object
{
    [SerializeField] private List<Entry> entries = new List<Entry>();
    
    private Dictionary<string, Entry> nameLookup = new Dictionary<string, Entry>();

    /// <summary>
    /// Adds a resource to the asset
    /// </summary>
    /// <param name="resource">The resource to add</param>
    /// <param name="preview">The resource preview</param>
    public void AddResource(T resource, Texture2D preview)
    {
        Entry entry = new Entry() {Resource = resource, Preview = preview};
        entries.Add(entry);
        nameLookup[resource.name] = entry;
    }
    
    /// <summary>
    /// Removes a resource from the asset
    /// </summary>
    /// <param name="resource">The resource to remove</param>
    public void RemoveResource(T resource)
    {
        Entry entry = nameLookup[resource.name];
        entries.Remove(entry);
        nameLookup.Remove(resource.name);
    }
    
    /// <summary>
    /// Clears all resources from the asset
    /// </summary>
    public void Clear()
    {
        entries.Clear();
        nameLookup.Clear();
    }
    
    /// <summary>
    /// Implementation of <see cref="IResourceLibrary.LoadResource"/>
    /// </summary>
    public T LoadResource(string name)
    {
        if (nameLookup.TryGetValue(name, out Entry entry))
        {
            return entry.Resource;
        }
        return null;
    }
    
    /// <summary>
    /// Implementation of <see cref="IResourceLibrary.LoadAllResources"/>
    /// </summary>
    public IEnumerable<T> LoadAllResources()
    {
        foreach (Entry entry in entries)
        {
            yield return entry.Resource;
        }
    }
    
    /// <summary>
    /// Implementation of <see cref="IResourceLibrary.LoadResourcePreview"/>
    /// </summary>
    public Texture2D LoadResourcePreview(string name)
    {
        if (nameLookup.TryGetValue(name, out Entry entry))
        {
            return entry.Preview;
        }
        return null;
    }
    
    // Load up cache on enable for faster lookup
    private void OnEnable()
    {
        nameLookup.Clear();
        foreach (var entry in entries)
        {
            nameLookup[entry.Resource.name] = entry;
        }
    }
    
    [Serializable]
    private struct Entry
    {
        public T Resource;
        public Texture2D Preview;
    }
}