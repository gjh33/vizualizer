using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class AssetPreviewBuildCommand
{
    private const string RootPath = "Assets/Resources";
    private const string OutputPath = "Resources/Preview Images";
    private const string MeshResourcePath = "Assets/Resources/Meshes";
    private const string MaterialResourcePath = "Assets/Resources/Materials";
    private const string TextureResourcePath = "Assets/Resources/Textures";
    
    [MenuItem("Tools/Build Asset Previews")]
    public static void BuildAssetPreviews()
    {
        GenerateForPath<Mesh>(MeshResourcePath);
        GenerateForPath<Material>(MaterialResourcePath);
        GenerateForPath<Texture2D>(TextureResourcePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void GenerateForPath<T>(string path) where T : Object
    {
        var assetGuids = AssetDatabase.FindAssets("", new[] { path });
        foreach (string assetGuid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null) continue;
            
            // Busy loop to avoid null
            // Pretty sure I'm abusing this API
            AssetPreview.GetAssetPreview(asset);
            while (AssetPreview.IsLoadingAssetPreview(asset.GetInstanceID())) {}
            
            Texture2D previewTexture = AssetPreview.GetAssetPreview(asset);
            
            string outputPath = assetPath.Replace(RootPath, OutputPath);
            outputPath = outputPath.Substring(0, outputPath.LastIndexOf('.'));
            outputPath += ".png";
            outputPath = Application.dataPath + "/" + outputPath;
            byte[] bytes = previewTexture.EncodeToPNG();
            File.WriteAllBytes(outputPath, bytes);
        }
    }
}
