using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using static AreaSelectorToolController;
using System;

public class AreaSelectorWindow : ToolWindowController
{
    public static AreaSelectorWindow Instance;

    public GameObject currentSelector;
    private Manager _Manager;
    private AreaSelectorToolController _AreaSelectorToolController;

    public LayerMask layerMask;

    public string tag;
    public float radius;
    public string currentMode;

    public List<GameObject> selectedObjects = new List<GameObject>();
    private List<string> modeList = new List<string>();


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
        currentMode = LoadDataString("Area Selector Mode");

        if (Instance == null)
            Instance = this;

        ClearUndoRedo();
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

        _AreaSelectorToolController = AreaSelectorToolController.Instance;
       
        if(_AreaSelectorToolController)
            SetButton(rootVisualElement, "Deselect-Container", _AreaSelectorToolController.DeselectObjects);

        currentSelector.GetComponent<ToolGizmo>().radius = radius;

        SaveDataInt("Area Selector Radius", radius.ConvertTo<int>());

        UndoAction(rootVisualElement, currentSelector);
        RedoAction(rootVisualElement, currentSelector);

        Mode();
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

        if (currentMode != null)
            rootVisualElement.Q<DropdownField>("Mode-Selector").value = currentMode;

        if (currentSelector == null)
        {
            currentSelector = Instantiate(_Manager.areaSelectorPrefab);
            Selection.activeGameObject = currentSelector;
        }
    }

    private void Mode()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Mode-Selector");

        modeList.Add(Modes.Move.ToString());
        modeList.Add(Modes.Rotate.ToString());
        modeList.Add(Modes.Scale.ToString());

        dropdownField.choices = modeList;

        currentMode = dropdownField.value;
        SaveDataString("Area Selector Mode", currentMode);

        if (Enum.TryParse(currentMode, out Modes mode) && _AreaSelectorToolController)
            _AreaSelectorToolController.currentMode = mode;
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
