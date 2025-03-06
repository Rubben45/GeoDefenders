using UnityEngine;
using UnityEditor; 
using System.IO;

public class CountLinesOfCode : EditorWindow
{
    [MenuItem("Tools/Count Lines of Code")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow<CountLinesOfCode>("Line Counter");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Count Lines of Code"))
        {
            CountLines();
        }
    }

    private void CountLines()
    {
        string pathToScripts = Path.Combine(Application.dataPath, "Scripts");

        string[] files = Directory.GetFiles(pathToScripts, "*.cs", SearchOption.AllDirectories);
        int lineCount = 0;
        foreach (string file in files)
        {
            string[] lines = File.ReadAllLines(file);
            lineCount += lines.Length;
        }
        Debug.Log("Total Lines of Code in Scripts folder: " + lineCount);
    }
}
