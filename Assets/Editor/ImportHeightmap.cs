using UnityEngine;
using UnityEditor;
using System.IO;

public class ImportHeightmap : EditorWindow
{
    [MenuItem("Tools/Import Heightmap")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ImportHeightmap));
    }

    private Terrain terrain;
    private Texture2D heightmapTexture;

    void OnGUI()
    {
        GUILayout.Label("Import Terrain Heightmap", EditorStyles.boldLabel);

        terrain = EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true) as Terrain;
        heightmapTexture = EditorGUILayout.ObjectField("Heightmap Texture", heightmapTexture, typeof(Texture2D), false) as Texture2D;

        if (GUILayout.Button("Import"))
        {
            if (terrain != null && heightmapTexture != null)
            {
                ApplyHeightmapToTerrain(terrain, heightmapTexture);
            }
            else
            {
                Debug.LogError("Please assign both a terrain and a heightmap texture.");
            }
        }
    }

    void ApplyHeightmapToTerrain(Terrain terrain, Texture2D heightmap)
    {
        TerrainData terrainData = terrain.terrainData;
        int width = heightmap.width;
        int height = heightmap.height;

        float[,] heights = new float[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixelColor = heightmap.GetPixel(x, y);
                heights[y, x] = pixelColor.grayscale;
            }
        }

        terrainData.heightmapResolution = width;
        terrainData.SetHeights(0, 0, heights);
    }
}
