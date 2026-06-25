using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class AreaSelectorWindow : ToolWindowController
{
    public static AreaSelectorWindow Instance;

    public LayerMask layerMask;

    public string tag;
    public float radius;

    public List<GameObject> selectedObjects = new List<GameObject>();

    public GameObject currentSelector;
    private Manager _Manager;


    [MenuItem("Tools/Area Selector Tool")]

    public static void ShowWindow()
    {
        GetWindow<AreaSelectorWindow>();
    }

    private void OnEnable()
    {
        layerMask.value = LoadDataInt("Area Selector LayerMask");
        radius = LoadDataInt("Area Selector Radius");
        tag = LoadDataString("Area Selector Tag");

        if (Instance == null)
            Instance = this;
    }

    private void OnDisable()
    {
        if (Instance != null)
            Instance = null;

        DestroyImmediate(currentSelector);
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();  
        radius = rootVisualElement.Q<Slider>("Radius-Slider").value;

        currentSelector.GetComponent<ToolGizmo>().radius = radius;

        SaveDataInt("Area Selector Radius", radius.ConvertTo<int>());

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.AreaSelectorToolWindow.CloneTree(rootVisualElement);

        if (tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = tag;

        if (radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = radius;

        if (layerMask.value >= 0)
            rootVisualElement.Q<DropdownField>("Layer-Mask-Selection").value = LayerMask.LayerToName(layerMask.value);

        if (currentSelector == null)
        {
            currentSelector = Instantiate(_Manager.areaSelectorPrefab);
            Selection.activeGameObject = currentSelector;
        }
    }

    private void Filters()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Layer-Mask-Selection");

        string[] layers = InternalEditorUtility.layers;
        dropdownField.choices = new List<string>(layers);

        int layer = LayerMask.NameToLayer(dropdownField.value);

        layerMask.value |= (1 << layer);

        SaveDataInt("Area Selector LayerMask", layer);

        tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Area Selector Tag", tag);
    }

}
