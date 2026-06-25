using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class PaintBrushWindow : ToolWindowController
{
    public static PaintBrushWindow Instance;

    public Color selectedColor;
    public LayerMask layerMask;

    public string tag;
    public float radius;

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

        if (Instance == null)
            Instance = this;
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

        selectedColor = rootVisualElement.Q<ColorField>("Color-Picker").value;
        SaveColor(selectedColor);

        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        radius = rootVisualElement.Q<Slider>("Radius-Slider").value;
        SaveDataInt("Paint Brush Radius", radius.ConvertTo<int>());

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

        if(currentBrush == null)
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
