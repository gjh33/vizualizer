using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

/// <summary>
/// Editor tool to build preview textures for use at runtime by the picker controller
/// </summary>
public class AssetPreviewBuildCommand
{
    private const string RootPath = "Data/Resource Sources";
    
    private static readonly string OutputPath = $"{RootPath}/Preview Images";
    private static readonly string LibraryOutputPath = "Assets/Data/Resource Libraries";
    private static readonly string MeshResourcePath = $"Assets/{RootPath}/Meshes";
    private static readonly string MeshLibraryPath = $"{LibraryOutputPath}/Meshes.asset";
    private static readonly string MaterialResourcePath = $"Assets/{RootPath}/Materials";
    private static readonly string MaterialLibraryPath = $"{LibraryOutputPath}/Materials.asset";
    private static readonly string TextureResourcePath = $"Assets/{RootPath}/Textures";
    private static readonly string TextureLibraryPath = $"{LibraryOutputPath}/Textures.asset";
    
    [MenuItem("Tools/Build Asset Previews")]
    public static void BuildAssetPreviews()
    {
        string outputDirectory = Application.dataPath + "/" + OutputPath;
        
        // Clear the output directory
        ClearDirectory(outputDirectory);
        
        MeshLibraryAsset meshLib = AssetDatabase.LoadAssetAtPath<MeshLibraryAsset>(MeshLibraryPath);
        if (meshLib == null)
        {
            meshLib = ScriptableObject.CreateInstance<MeshLibraryAsset>();
            AssetDatabase.CreateAsset(meshLib, MeshLibraryPath);
        }
        GenerateForPath(MeshResourcePath, meshLib);
        
        MaterialLibraryAsset materialLib = AssetDatabase.LoadAssetAtPath<MaterialLibraryAsset>(MaterialLibraryPath);
        if (materialLib == null)
        {
            materialLib = ScriptableObject.CreateInstance<MaterialLibraryAsset>();
            AssetDatabase.CreateAsset(materialLib, MaterialLibraryPath);
        }
        GenerateForPath(MaterialResourcePath, materialLib);
        
        TextureLibraryAsset textureLib = AssetDatabase.LoadAssetAtPath<TextureLibraryAsset>(TextureLibraryPath);
        if (textureLib == null)
        {
            textureLib = ScriptableObject.CreateInstance<TextureLibraryAsset>();
            AssetDatabase.CreateAsset(textureLib, TextureLibraryPath);
        }
        GenerateForPath(TextureResourcePath, textureLib);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void GenerateForPath<T>(string path, ResourceLibraryAsset<T> library) where T : Object
    {
        string[] assetGuids = AssetDatabase.FindAssets("", new[] { path });
        library.Clear();

        foreach (string assetGuid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null) continue;
            
            // Busy loop to avoid null
            // Pretty sure I'm abusing this API but this is an editor tool being used only by me
            // So i'm sure we'll survive ;)
            AssetPreview.GetAssetPreview(asset);
            while (AssetPreview.IsLoadingAssetPreview(asset.GetInstanceID())) {}
            
            Texture2D previewTexture = AssetPreview.GetAssetPreview(asset);
            
            string outputPath = assetPath.Replace(RootPath, OutputPath);
            outputPath = outputPath.Substring(0, outputPath.LastIndexOf('.'));
            outputPath += ".png";
            string assetOutputPath = outputPath;
            outputPath = Application.dataPath.Replace("Assets","") + outputPath;
            
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            
            byte[] bytes = previewTexture.EncodeToPNG();
            File.WriteAllBytes(outputPath, bytes);
            AssetDatabase.ImportAsset(assetOutputPath);

            Texture2D previewAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(assetOutputPath);
            library.AddResource(asset, previewAsset);
        }
    }
    
    private static void ClearDirectory(string path)
    {
        foreach (string file in Directory.EnumerateFiles(path))
        {
            File.Delete(file);
        }

        foreach (var directory in Directory.EnumerateDirectories(path))
        {
            ClearDirectory(directory);
            Directory.Delete(directory);
        }
    }
}