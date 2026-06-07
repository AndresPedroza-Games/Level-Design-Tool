using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

public class AreaSelectorWindow : ToolWindowController
{
    private LayerMask _LayerMask;

    private string _Tag;
    private int _Radius;

    public List<GameObject> selectedObjects = new List<GameObject>();

    Manager _Manager;


    [MenuItem("Tools/Area Selector Tool")]

    public static void ShowWindow()
    {
        GetWindow<AreaSelectorWindow>();
    }

    private void OnEnable()
    {
        _Radius = LoadDataInt("AreaSelector Radius");
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Radius", EditorStyles.boldLabel);
        _Radius = EditorGUILayout.IntSlider(_Radius, 1, 10);
        SaveDataInt("AreaSelector Radius", _Radius);
        GUILayout.EndHorizontal();

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.AreaSelectorToolWindow.CloneTree(rootVisualElement);
    }

    private void SelectGameObject()
    {
        if (!CheckLayerMask(DetectObject(EventType.MouseDown), _LayerMask, _Tag))
            return;
    } 

    private void Filters()
    {
        GUILayout.Space(20);
        GUILayout.Label("Filters", EditorStyles.boldLabel);
        GUILayout.Space(10);

        string[] layers = InternalEditorUtility.layers;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Layer Mask");
        _LayerMask.value = EditorGUILayout.MaskField("", _LayerMask.value, layers);
        SaveDataInt("Layer Value", _LayerMask.value);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tag");
        _Tag = EditorGUILayout.TextArea("");
        EditorGUILayout.EndHorizontal();
    }

}
