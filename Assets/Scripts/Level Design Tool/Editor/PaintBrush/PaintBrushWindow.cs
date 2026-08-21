using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class PaintBrushWindow : ToolWindowController
{
    public static PaintBrushWindow Instance;

    public Color selectedColor;
    public LayerMask layerMask;

    public string tag;
    public float radius;
    public float opacity;

    private Manager _Manager;
    public GameObject currentBrush;

    [MenuItem("Tools/Paint Brush Tool")]
    public static void ShowWindow()
    {
        GetWindow<PaintBrushWindow>();
    }

    private void OnEnable()
    {
        selectedColor = LoadColor();
        layerMask.value = LoadDataInt("Paint Brush LayerMask");
        radius = LoadDataInt("Paint Brush Radius");
        tag = LoadDataString("Paint Brush Tag");
        opacity = LoadDataInt("Paint Brush Opacity");

        if (Instance == null)
            Instance = this;

        ClearUndoRedo();
    }

    private void OnDisable()
    {
        DestroyImmediate(currentBrush);

        if (Instance != null)
            Instance = null;
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        radius = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        SaveDataInt("Paint Brush Radius", radius.ConvertTo<int>());

        rootVisualElement.Q<IntegerField>("Opacity-Field").value = rootVisualElement.Q<Slider>("Opacity-Slider").value.ConvertTo<int>();
        opacity = rootVisualElement.Q<Slider>("Opacity-Slider").value.ConvertTo<int>();
        SaveDataInt("Paint Brush Opacity", opacity.ConvertTo<int>());

        selectedColor = rootVisualElement.Q<ColorField>("Color-Picker").value;
        selectedColor.a = opacity / 10;
        SaveColor(selectedColor);

        UndoAction(rootVisualElement, currentBrush);
        RedoAction(rootVisualElement, currentBrush);

        currentBrush.GetComponent<ToolGizmo>().radius = radius;

        Filters();
    }


    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.paintBrushToolWindow.CloneTree(rootVisualElement);

        if (tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = tag;

        if(selectedColor != null)
            rootVisualElement.Q<ColorField>("Color-Picker").value = selectedColor;

        if (radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = radius;

        if (layerMask.value >= 0)
            rootVisualElement.Q<DropdownField>("Layer-Mask-Selection").value = LayerMask.LayerToName(layerMask.value);

        if (opacity != 0)
            rootVisualElement.Q<Slider>("Opacity-Slider").value = opacity;

        if (currentBrush == null)
        {
            currentBrush = Instantiate(_Manager.paintBrushPrefab);

            Selection.activeGameObject = currentBrush;
        }
    }

    private void SaveColor(Color color)
    {
        EditorPrefs.SetFloat("Color.r", color.r);
        EditorPrefs.SetFloat("Color.g", color.g);
        EditorPrefs.SetFloat("Color.b", color.b);
        EditorPrefs.SetFloat("Color.a", color.a);
    }

    private Color LoadColor()
    {
        return new Color(EditorPrefs.GetFloat("Color.r"), EditorPrefs.GetFloat("Color.g"), EditorPrefs.GetFloat("Color.b"), EditorPrefs.GetFloat("Color.a"));
    }

    private void Filters()
    {
        DropdownField dropdownField = rootVisualElement.Q<DropdownField>("Layer-Mask-Selection");

        string[] layers = InternalEditorUtility.layers;
        dropdownField.choices = new List<string>(layers);

        int layer = LayerMask.NameToLayer(dropdownField.value);

        layerMask.value |= (1 << layer);

        SaveDataInt("Paint Brush LayerMask", layer);

        tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Paint Brush Tag", tag);
    }
}
