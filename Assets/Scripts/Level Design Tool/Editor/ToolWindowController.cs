using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ToolWindowController : EditorWindow
{
    protected void Heading(VisualElement ui, EditorWindow closeWindow)
    {
        SetButton(ui, "Return-Button", () => Return(closeWindow));
    }

    private void Return(EditorWindow closeWindow)
    {
        LevelDesignToolWindow.ShowWindow();
        closeWindow.Close();
    }

    protected void ChangeWindow(UnityAction newWindow, EditorWindow currentWindow)
    {
        newWindow.Invoke();
        currentWindow.Close();
    }    

    protected void SaveDataInt(string name,int index)
    {
        EditorPrefs.SetInt(name,index);
    }

    protected void SaveDataString(string name, string value)
    {
        EditorPrefs.SetString(name, value);
    }

    protected int LoadDataInt(string name)
    {
        return EditorPrefs.GetInt(name);
    }

    protected string LoadDataString(string name)
    {
        return EditorPrefs.GetString(name);
    }

    protected void SetButton(VisualElement ui, string buttonName, UnityAction method)
    {
        Button returnButton = ui.Q<Button>(buttonName);

        if (returnButton == null)
        {
            Debug.Log($"Button {buttonName} not found");
            return;
        }

        returnButton.clicked += method.Invoke;
    }

    protected void UndoAction(VisualElement ui, GameObject currentTool)
    {
        SetButton(ui, "UndoBtn", Undo.PerformUndo);
        Selection.activeGameObject = currentTool;
    }

    protected void RedoAction(VisualElement ui, GameObject currentTool)
    {
        SetButton(ui, "RedoBtn", Undo.PerformRedo);
        Selection.activeGameObject = currentTool;
    }

    protected void ClearUndoRedo()
    {
        Undo.ClearAll();
    }
}
