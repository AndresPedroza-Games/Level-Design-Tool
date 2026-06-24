using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class EraseWindow : ToolWindowController
{
    private Manager _Manager;

    private LayerMask _LayerMask;

    private string _Tag;
    private float _Radius;

    [MenuItem("Tools/Erase Tool")]
    public static void ShowWindow()
    {
        GetWindow<EraseWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += EraseObject;

        _LayerMask.value = LoadDataInt("Eraser LayerMask");
        _Radius = LoadDataInt("Eraser Radius");
        _Tag = LoadDataString("Eraser Tag");
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= EraseObject;
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);


        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        _Radius = rootVisualElement.Q<Slider>("Radius-Slider").value;

        SaveDataInt("Eraser Radius", _Radius.ConvertTo<int>());

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.eraserToolWindow.CloneTree(rootVisualElement);

        if (_Tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = _Tag;

        if (_Radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = _Radius;
    }

    private void EraseObject(SceneView sceneView)
    {
        if (DetectObject(EventType.MouseDown) != null)
        {
            if (!CheckLayerMask(DetectObject(EventType.MouseDown), _LayerMask, _Tag))
                return;

            _Manager.currentTilesContainer.tilesGameobjects.Remove(GetTileInList(DetectObject(EventType.MouseDown)));
            DestroyImmediate(DetectObject(EventType.MouseDown));
        }
    }

    private GameObject GetTileInList(GameObject tile)
    {
        foreach (GameObject tiles in _Manager.currentTilesContainer.tilesGameobjects)
        {
            if (tile.GetInstanceID() == tiles.GetInstanceID() && _Manager.currentTilesContainer.tilesGameobjects.Contains(tile))
                return tiles;
        }
        return null;
    }

    private void Filters()
    {
        string[] layers = InternalEditorUtility.layers;

        _LayerMask.value = EditorGUILayout.MaskField("", _LayerMask.value, layers);
        //_LayerMask.value = rootVisualElement.Q<MaskField>();
        SaveDataInt("Eraser LayerMas", _LayerMask.value);

        _Tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Eraser Tag", _Tag);
    }
}
