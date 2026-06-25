using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Collections.Generic;

public class EraseWindow : ToolWindowController
{
    public static EraseWindow Instance;

    public Manager manager;
    public GameObject currentEraser;

    public LayerMask layerMask;

    public string tag;
    public float radius;

    [MenuItem("Tools/Erase Tool")]
    public static void ShowWindow()
    {
        GetWindow<EraseWindow>();
    }

    private void OnEnable()
    {
        layerMask.value = LoadDataInt("Eraser LayerMask");
        radius = LoadDataInt("Eraser Radius");
        tag = LoadDataString("Eraser Tag");

        if (Instance == null)
            Instance = this;
    }

    private void OnDisable()
    {
        if (Instance != null)
            Instance = null;

        DestroyImmediate(currentEraser);
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);


        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        radius = rootVisualElement.Q<Slider>("Radius-Slider").value;

        currentEraser.GetComponent<ToolGizmo>().radius = radius;

        SaveDataInt("Eraser Radius", radius.ConvertTo<int>());

        Filters();
    }

    public void CreateGUI()
    {
        manager = FindFirstObjectByType<Manager>();
        manager.eraserToolWindow.CloneTree(rootVisualElement);

        if (tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = tag;

        if (radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = radius;

        if (layerMask.value >= 0)
            rootVisualElement.Q<DropdownField>("Layer-Mask-Selection").value = LayerMask.LayerToName(layerMask.value);

        if (currentEraser == null)
        {
            currentEraser = Instantiate(manager.eraserPrefab);
            Selection.activeGameObject = currentEraser;
        }
    }

    private void Filters()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Layer-Mask-Selection");

        string[] layers = InternalEditorUtility.layers;
        dropdownField.choices = new List<string>(layers);

        int layer = LayerMask.NameToLayer(dropdownField.value);

        layerMask.value |= (1 << layer);

        SaveDataInt("Eraser LayerMas", layer);

        tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Eraser Tag", tag);
    }
}
