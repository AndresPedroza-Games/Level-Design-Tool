using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ToolWindowController : EditorWindow
{
    protected void Heading(string headingName, EditorWindow closeWindow)
    {
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(headingName, EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Return"))
            Return(closeWindow);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
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

    protected void ChangeWindow(UnityAction closeWindow, EditorWindow currentWindow)
    {
        closeWindow.Invoke();
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
}
