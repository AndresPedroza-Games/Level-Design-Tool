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

    protected GameObject DetectObject(EventType eventType)
    {
        Event onClick = Event.current;

        if (onClick.type == eventType)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.GetComponent<TilesController>() != null)
            {
                return hit.collider.gameObject;
            }
        }

        return null;
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

    protected bool CheckLayerMask(GameObject gameObject, LayerMask requireLayer, string tag)
    {
        if ((requireLayer.value & (1 << gameObject.layer)) == 0 && gameObject.tag != tag)
        {
            Debug.Log("Layer Mask doens't Match");
            return false;
        }

        return true;
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
}
