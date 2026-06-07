using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class EraseWindow : ToolWindowController
{
    private Manager _Manager;

    private LayerMask _LayerMask;

    private string _Tag;
    private int _Radius;

    [MenuItem("Tools/Erase Tool")]
    public static void ShowWindow()
    {
        GetWindow<EraseWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += EraseObject;

        _LayerMask.value = LoadDataInt("LayerMask Eraser");
        _Radius = LoadDataInt("Eraser Radius");
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= EraseObject;
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        _Radius = EditorGUILayout.IntSlider(_Radius, 1, 10);

        SaveDataInt("Eraser Radius", _Radius);

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.eraserToolWindow.CloneTree(rootVisualElement);
    }

    private void EraseObject(SceneView sceneView)
    {
        if (DetectObject(EventType.MouseDown) != null)
        {
            if (!CheckLayerMask(DetectObject(EventType.MouseDown), _LayerMask, _Tag))
                return;

            _Manager.tilesContainer.tilesGameobjects.Remove(GetTileInList(DetectObject(EventType.MouseDown)));
            DestroyImmediate(DetectObject(EventType.MouseDown));
        }
    }

    private GameObject GetTileInList(GameObject tile)
    {
        foreach (GameObject tiles in _Manager.tilesContainer.tilesGameobjects)
        {
            if (tile.GetInstanceID() == tiles.GetInstanceID() && _Manager.tilesContainer.tilesGameobjects.Contains(tile))
                return tiles;
        }
        return null;
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
        SaveDataInt("LayerMask Eraser", _LayerMask.value);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tag");
        _Tag = EditorGUILayout.TextArea("");
        EditorGUILayout.EndHorizontal();
    }
}
