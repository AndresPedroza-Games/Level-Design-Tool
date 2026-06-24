using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class AreaSelectorWindow : ToolWindowController
{
    private LayerMask _LayerMask;

    private string _Tag;
    private float _Radius;

    public List<GameObject> selectedObjects = new List<GameObject>();

    Manager _Manager;


    [MenuItem("Tools/Area Selector Tool")]

    public static void ShowWindow()
    {
        GetWindow<AreaSelectorWindow>();
    }

    private void OnEnable()
    {
        _LayerMask.value = LoadDataInt("Area Selector LayerMask");
        _Radius = LoadDataInt("Area Selector Radius");
        _Tag = LoadDataString("Area Selector Tag");
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();  
        _Radius = rootVisualElement.Q<Slider>("Radius-Slider").value;

        SaveDataInt("Area Selector Radius", _Radius.ConvertTo<int>());

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.AreaSelectorToolWindow.CloneTree(rootVisualElement);

        if (_Tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = _Tag;

        if (_Radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = _Radius;

        if (_LayerMask.value >= 0)
            rootVisualElement.Q<DropdownField>("Layer-Mask-Selection").value = LayerMask.LayerToName(_LayerMask.value);
    }

    private void SelectGameObject()
    {
        if (!CheckLayerMask(DetectObject(EventType.MouseDown), _LayerMask, _Tag))
            return;
    }

    private void Filters()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Layer-Mask-Selection");

        string[] layers = InternalEditorUtility.layers;
        dropdownField.choices = new List<string>(layers);

        int layer = LayerMask.NameToLayer(dropdownField.value);

        _LayerMask.value |= (1 << layer);

        SaveDataInt("Area Selector LayerMask", layer);

        _Tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Area Selector Tag", _Tag);
    }

}
