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
}
