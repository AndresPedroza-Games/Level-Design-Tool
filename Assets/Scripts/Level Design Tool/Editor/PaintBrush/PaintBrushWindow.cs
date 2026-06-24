using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class PaintBrushWindow : ToolWindowController
{
    private Color _SelectedColor;
    private LayerMask _LayerMask;

    private string _Tag;
    private float _Radius;

    private Manager _Manager;

    [MenuItem("Tools/Paint Brush Tool")]
    public static void ShowWindow()
    {
        GetWindow<PaintBrushWindow>();
        //Instantiate(manager.paintBrushTool);
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += PaintTile;

        _SelectedColor = LoadColor();
        _LayerMask.value = LoadDataInt("Paint Brush LayerMask");
        _Radius = LoadDataInt("Paint Brush Radius");
        _Tag = LoadDataString("Paint Brush Tag");        

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= PaintTile;
    }

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        _SelectedColor = rootVisualElement.Q<ColorField>("Color-Picker").value;
        SaveColor(_SelectedColor);

        rootVisualElement.Q<IntegerField>("Radius-Field").value = rootVisualElement.Q<Slider>("Radius-Slider").value.ConvertTo<int>();
        _Radius = rootVisualElement.Q<Slider>("Radius-Slider").value;
        SaveDataInt("Paint Brush Radius", _Radius.ConvertTo<int>());

        Filters();
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.paintBrushToolWindow.CloneTree(rootVisualElement);

        if (_Tag != null)
            rootVisualElement.Q<TextField>("Tag-Input").value = _Tag;

        if(_SelectedColor != null)
            rootVisualElement.Q<ColorField>("Color-Picker").value = _SelectedColor;

        if (_Radius != 0)
            rootVisualElement.Q<Slider>("Radius-Slider").value = _Radius;
    }

    private void PaintTile(SceneView sceneView)
    {
        if (DetectObject(EventType.MouseDown) != null)
        {
            if (!CheckLayerMask(DetectObject(EventType.MouseDown), _LayerMask, _Tag))
                return;

            TilesCustomization.PaintMaterial(_SelectedColor, DetectObject(EventType.MouseDown), _LayerMask);
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
        string[] layers = InternalEditorUtility.layers;

        _LayerMask.value = EditorGUILayout.MaskField("", _LayerMask.value, layers);
        //_LayerMask.value = rootVisualElement.Q<MaskField>();
        SaveDataInt("Paint Brush LayerMask", _LayerMask.value);

        _Tag = rootVisualElement.Q<TextField>("Tag-Input").value;

        SaveDataString("Paint Brush Tag", _Tag);
    }
}
