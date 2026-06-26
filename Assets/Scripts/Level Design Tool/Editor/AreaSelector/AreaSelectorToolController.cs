using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CustomEditor(typeof(AreaSelectorTool))]
public class AreaSelectorToolController : ToolsController
{
    public static AreaSelectorToolController Instance;

    private AreaSelectorWindow _AreaSelector;

    public List<Transform> selectedGameobject = new List<Transform>();

    private Dictionary<Modes, UnityAction> _ModeDicctionary;

    public Modes currentMode;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;

        _ModeDicctionary = new Dictionary<Modes, UnityAction>
        {
            {Modes.Move, MoveObject},
            {Modes.Rotate, RotateObject},
            {Modes.Scale, ScaleObject}
        };
    }

    private void OnDisable()
    {
        if (Instance != null)
            Instance = null;
    }

    private void OnSceneGUI()
    {
        if (AreaSelectorWindow.Instance == null)
            return;

        _AreaSelector = AreaSelectorWindow.Instance;

        if (!_AreaSelector.currentSelector.GetComponent<AreaSelectorTool>().freeze)
            MoveTool(_AreaSelector.currentSelector.transform);

        SelectObjects();

        //_ModeDicctionary[currentMode].Invoke();

        SceneView.RepaintAll();
    }

    private void SelectObjects()
    {
        Event onClick = Event.current;

        if (onClick.type == EventType.MouseDown)
        {
            foreach (RaycastHit hit in DetectCollision(_AreaSelector.currentSelector.transform.position, _AreaSelector.radius))
            {
                GameObject gameObject = hit.collider.gameObject;

                if (!CheckLayerMask(gameObject, _AreaSelector.layerMask, _AreaSelector.tag))
                    return;

                gameObject.GetComponent<TilesController>().isSelected = true;

                if(!selectedGameobject.Contains(gameObject.transform))
                    selectedGameobject.Add(gameObject.transform);
            }
        }
    }

    public void DeselectObjects()
    {
        if (selectedGameobject.Count <= 0)
            return;

        for (int i = 0; i < selectedGameobject.Count; i++)
        {
            selectedGameobject[i].GetComponent<TilesController>().isSelected = false;

            selectedGameobject.Remove(selectedGameobject[i].transform);
        }

        _AreaSelector.currentSelector.GetComponent<AreaSelectorTool>().freeze = false;
    }

    private void MoveObject()
    {
        if (selectedGameobject.Count <= 0)
            return;

        Vector3 center = Vector3.zero;

        foreach (Transform transform in selectedGameobject)
        {
            if (transform != null)
                center += transform.position;
        }

        center /= selectedGameobject.Count;

        EditorGUI.BeginChangeCheck();

        Vector3 newCenter = Handles.PositionHandle(center,Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Vector3 delta = newCenter - center;

            Undo.RecordObjects(selectedGameobject.ToArray(), "Move Group");

            foreach (Transform t in selectedGameobject)
            {
                if (t != null)
                    t.position += delta;
            }
        }
    }

    private void RotateObject()
    {
        if (selectedGameobject.Count <= 0)
            return;

        Vector3 center = Vector3.zero;

        foreach (Transform transform in selectedGameobject)
        {
            if (transform != null)
                center += transform.position;
        }

        center /= selectedGameobject.Count;

        Quaternion selectionRotation = Quaternion.identity;

        EditorGUI.BeginChangeCheck();

        Quaternion newRotation = Handles.RotationHandle(selectionRotation, center);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(selectedGameobject.ToArray(), "Rotate Group");

            Quaternion deltaRotation = newRotation * Quaternion.Inverse(selectionRotation);

            foreach (Transform transform in selectedGameobject)
            {
                Vector3 dir = transform.position - center;
                dir = deltaRotation * dir;
                transform.position = center + dir;

                transform.rotation = deltaRotation * transform.rotation;
            }

            selectionRotation = newRotation;
        }
    }

    private void ScaleObject()
    {
        if (selectedGameobject.Count <= 0)
            return;

        Vector3 center = Vector3.zero;

        foreach (Transform transform in selectedGameobject)
        {
            if (transform != null)
                center += transform.position;
        }

        center /= selectedGameobject.Count;

        EditorGUI.BeginChangeCheck();

        Vector3 newCenter = Handles.PositionHandle(center, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Vector3 delta = newCenter - center;

            Undo.RecordObjects(selectedGameobject.ToArray(), "Move Group");

            foreach (Transform t in selectedGameobject)
            {
                if (t != null)
                    t.position += delta;
            }
        }
    }

    public enum Modes
    {
        Move,
        Rotate,
        Scale
    }
}
