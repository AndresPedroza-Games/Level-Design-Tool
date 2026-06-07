using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LevelDesignToolWindow : EditorWindow
{
    private Manager _Manager;

    [MenuItem("Tools/Level Design Tool %L")]
    public static void ShowWindow()
    {
        GetWindow<LevelDesignToolWindow>();
    }

    public void OnGUI()
    {
        ToolBox("Paint-Brush-Button", PaintBrushWindow.ShowWindow);

        ToolBox("Area-Selector-Button", AreaSelectorWindow.ShowWindow);

        ToolBox("Change-Material-Button", ChangeMaterialWindow.ShowWindow);

        ToolBox("Builder-Button", BuilderWindow.ShowWindow);

        ToolBox("Eraser-Button", EraseWindow.ShowWindow);

    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();

        _Manager.levelDesignToolWindow.CloneTree(rootVisualElement);
    }

    private void ToolBox(string toolName, UnityAction method)
    {
        Button button = rootVisualElement.Q<Button>(toolName);

        if(button == null)
        {
            Debug.Log("Button not found");
            return;
        }

        button.clicked += method.Invoke;
        button.clicked += this.Close;
    }
}
