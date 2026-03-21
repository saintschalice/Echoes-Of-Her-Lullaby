using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ForceSlice : EditorWindow
{
    [MenuItem("Tools/Slicing/Slice 2x2")]
    public static void SliceNow()
    {
        // Kunin ang image na naka-select sa Project Window
        Texture2D texture = Selection.activeObject as Texture2D;
        if (texture == null)
        {
            Debug.LogError("Pumili ka muna ng Image sa Project window bago i-click ito!");
            return;
        }

        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

        ti.spriteImportMode = SpriteImportMode.Multiple;

        // Hatiin sa 2x2
        int sliceW = texture.width / 2;
        int sliceH = texture.height / 2;

        List<SpriteMetaData> metas = new List<SpriteMetaData>();

        // Loop para sa apat na quadrants
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                SpriteMetaData smd = new SpriteMetaData();
                smd.alignment = (int)SpriteAlignment.Center;
                smd.rect = new Rect(x * sliceW, (1 - y) * sliceH, sliceW, sliceH);
                smd.name = texture.name + "_" + x + "_" + y;
                metas.Add(smd);
            }
        }

        ti.spritesheet = metas.ToArray();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log("Ayos! Na-slice na ang " + texture.name + " sa apat.");
    }
}