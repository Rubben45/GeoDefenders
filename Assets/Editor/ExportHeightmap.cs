using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportHeightmap : EditorWindow
{
    [MenuItem("Tools/Export Heightmap")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ExportHeightmap));
    }

    private Terrain terrain;
    private string path = "Assets/Heightmap.png";

    void OnGUI()
    {
        GUILayout.Label("Export Terrain Heightmap", EditorStyles.boldLabel);

        terrain = EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true) as Terrain;

        path = EditorGUILayout.TextField("Path", path);

        if (GUILayout.Button("Export"))
        {
            if (terrain != null)
            {
                ExportTerrainHeightmap(terrain, path);
            }
            else
            {
                Debug.LogError("No terrain found. Please assign a terrain object.");
            }
        }
    }

    void ExportTerrainHeightmap(Terrain terrain, string path)
    {
        var terrainData = terrain.terrainData;
        int w = terrainData.heightmapResolution;
        int h = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, w, h);

        Texture2D texture = new Texture2D(w, h, TextureFormat.ARGB32, false);

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float value = heights[y, x];
                Color color = new Color(value, value, value, 1);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        Debug.Log("Heightmap exported to " + path);
        AssetDatabase.Refresh();
    }
}