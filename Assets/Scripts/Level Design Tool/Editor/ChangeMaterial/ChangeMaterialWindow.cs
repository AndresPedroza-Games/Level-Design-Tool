using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ChangeMaterialWindow : ToolWindowController
{
    private Material _SelectedMaterial;
    private Material _NewMaterial;
    private Manager _Manager;

    private LayerMask _LayerMask;

    private string _Tag;

    [MenuItem("Tools/Change Material Window")]
    public static void ShowWindow()
    {
        GetWindow<ChangeMaterialWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += ChangeTileMaterial;
        _SelectedMaterial = AssetDatabase.LoadAssetAtPath<Material>(LoadDataString("Selected Material"));
        _LayerMask.value = LoadDataInt("LayerMask Change Material");
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= ChangeTileMaterial;
    }

    private void OnGUI()
    {
        Heading(rootVisualElement, this);
        GUILayout.Space(20);

        if (_SelectedMaterial == null)
            EditorGUILayout.HelpBox("No Material is selected!", MessageType.Warning);

        MaterialSelector();

        Filters();
    }

    private void MaterialSelector()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Selected Material", EditorStyles.boldLabel);
        _SelectedMaterial = (Material)EditorGUILayout.ObjectField(_SelectedMaterial, typeof(Material), false);
        GUILayout.EndHorizontal();

        SaveDataString("Selected Material", AssetDatabase.GetAssetPath(_SelectedMaterial));
    }

    private void ChangeTileMaterial(SceneView sceneView)
    {
        EventType onClick = EventType.MouseDown;

        if (DetectObject(onClick) != null)
        {
            if (!CheckLayerMask(DetectObject(EventType.MouseDown), _LayerMask, _Tag))
                return;

            GameObject selectedObject = DetectObject(onClick);

            Renderer renderer = selectedObject.GetComponent<Renderer>();

            if (_SelectedMaterial == null)
                _SelectedMaterial = renderer.sharedMaterial;
            else
                renderer.sharedMaterial = _SelectedMaterial;
        }   

    }

    protected void Filters()
    {
        GUILayout.Space(20);
        GUILayout.Label("Filters", EditorStyles.boldLabel);
        GUILayout.Space(10);

        string[] layers = InternalEditorUtility.layers;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Layer Mask");
        _LayerMask.value = EditorGUILayout.MaskField("", _LayerMask.value, layers);
        SaveDataInt("LayerMask Change Material", _LayerMask.value);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tag");
        _Tag = EditorGUILayout.TextArea("");
        EditorGUILayout.EndHorizontal();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.changeMaterialToolWindow.CloneTree(rootVisualElement);
    }
}
