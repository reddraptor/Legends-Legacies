using UnityEngine;
using UnityEditor;

public class TerrainTile_TilePrefabs : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    [MenuItem("CONTEXT/TerrainTile/Tile Prefabs")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainTile_TilePrefabs));
    }

    void OnGUI()
    {
        titleContent.text = "Tile Prefabs";
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}