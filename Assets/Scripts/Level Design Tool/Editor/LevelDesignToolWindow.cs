using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class LevelDesignToolWindow : EditorWindow
{
    [MenuItem("Tools/Level Design Tool %L")]
    public static void ShowWindow()
    {
        GetWindow<LevelDesignToolWindow>();     
    }

    public void OnGUI()
    {
        GUILayout.Label("Level Design Tool");
        GUILayout.Space(10);

        GUILayout.BeginVertical();
        ToolBox("Paint brush", PaintBrushWindow.ShowWindow);

        GUILayout.Space(10);
        ToolBox("Area Selector", AreaSelectorWindow.ShowWindow);

        GUILayout.Space(10);
        ToolBox("Change Material", ChangeMaterialWindow.ShowWindow);

        GUILayout.Space(10);
        ToolBox("Builder", BuilderWindow.ShowWindow);

        GUILayout.Space(10);
        ToolBox("Eraser", EraseWindow.ShowWindow);

        GUILayout.EndVertical();
    }

    private void ToolBox(string toolName, UnityAction method)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(toolName);
        if (GUILayout.Button("Use"))
        {
            method.Invoke();
            this.Close();
        }

        GUILayout.EndHorizontal();
    }
}
