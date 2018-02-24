using UnityEngine;
using UnityEditor;

public class ChunkEditor : ScriptableObject
{
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }

    [MenuItem("CONTEXT/Chunk/Open Chunk Editor")]
    static void Open()
    {
        EditorUtility.DisplayDialog("Editor", "Editor soon", "OK", "");
    }

    //[MenuItem("CONTEXT/Chunk/Open Chunk Editor")]
}