using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeMaterialWindow : ToolWindowController
{
    public static ChangeMaterialWindow Instance;

    public GameObject currentSelector;
    public Material selectedMaterial;
    public Material newMaterial;
    private Manager _Manager;

    public LayerMask layerMask;

    public string tag;
    public float radius;

    [MenuItem("Tools/Change Material Window")]
    public static void ShowWindow()
    {
        GetWindow<ChangeMaterialWindow>();
    }

    private void OnEnable()
    {
        selectedMaterial = AssetDatabase.LoadAssetAtPath<Material>(LoadDataString("Selected Material"));

        layerMask.value = LoadDataInt("Change Material LayerMask");
        tag = LoadDataString("Change Material Tag");
        radius = LoadDataInt("Change Material Radius");

        if (Instance == null)
            Instance = this;

    }

    private void OnDisable()
    {
        if (Instance != null)
            Instance = null;

        DestroyImmediate(currentSelector);
    }

    private void OnGUI()
    {
        Heading(rootVisualElement, this);
        GUILayout.Space(20);

        if (selectedMaterial == null)
            EditorGUILayout.HelpBox("No Material is selected!", MessageType.Warning);

        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        radius = rootVisualElement.Q<Slider>("Radius-Slider").value;

        SaveDataInt("Change Material Radius", radius.ConvertTo<int>());

        currentSelector.GetComponent<ToolGizmo>().radius = radius;

        MaterialSelector();

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.changeMaterialToolWindow.CloneTree(rootVisualElement);

        if (tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = tag;

        if(selectedMaterial != null)
            rootVisualElement.Q<ObjectField>("Material-Selector").value = selectedMaterial;

        if (layerMask.value >= 0)
            rootVisualElement.Q<DropdownField>("Layer-Mask-Selection").value = LayerMask.LayerToName(layerMask.value);

        if (currentSelector == null)
        {
            currentSelector = Instantiate(_Manager.changeMaterialPrefab);
            Selection.activeGameObject = currentSelector;
        }

        if (radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = radius;
    }


    private void MaterialSelector()
    {
        rootVisualElement.Q<ObjectField>("Material-Selector").value = selectedMaterial;

        SaveDataString("Selected Material", AssetDatabase.GetAssetPath(selectedMaterial));
    }

    private void Filters()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Layer-Mask-Selection");

        string[] layers = InternalEditorUtility.layers;
        dropdownField.choices = new List<string>(layers);

        int layer = LayerMask.NameToLayer(dropdownField.value);

        layerMask.value |= (1 << layer);

        SaveDataInt("Change Material LayerMask", layer);

        tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Change Material Tag", tag);
    }
}
