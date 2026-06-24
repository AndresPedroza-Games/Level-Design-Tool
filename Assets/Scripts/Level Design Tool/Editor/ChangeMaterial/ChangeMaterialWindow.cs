using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

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

        _LayerMask.value = LoadDataInt("Change Material LayerMask");

        _Tag = LoadDataString("Change Material Tag");
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

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.changeMaterialToolWindow.CloneTree(rootVisualElement);

        if (_Tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = _Tag;

        if(_SelectedMaterial != null)
            rootVisualElement.Q<ObjectField>("Material-Selector").value = _SelectedMaterial;

        if (_LayerMask.value >= 0)
            rootVisualElement.Q<DropdownField>("Layer-Mask-Selection").value = LayerMask.LayerToName(_LayerMask.value);

    }


    private void MaterialSelector()
    {
        _SelectedMaterial = (Material)rootVisualElement.Q<ObjectField>("Material-Selector").value;

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

    private void Filters()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Layer-Mask-Selection");

        string[] layers = InternalEditorUtility.layers;
        dropdownField.choices = new List<string>(layers);

        int layer = LayerMask.NameToLayer(dropdownField.value);

        _LayerMask.value |= (1 << layer);

        SaveDataInt("Change Material LayerMask", layer);

        _Tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Change Material Tag", _Tag);
    }
}
