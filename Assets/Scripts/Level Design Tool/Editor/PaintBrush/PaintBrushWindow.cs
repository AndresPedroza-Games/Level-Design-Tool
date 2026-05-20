using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class PaintBrushWindow : ToolWindowController
{
    private Color _SelectedColor;
    private LayerMask _LayerMask;

    private string _Tag;
    private int _Radius;

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
        _LayerMask.value = LoadDataInt("LayerMask Paint");
        _Radius = LoadDataInt("Paint Brush Radius");
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= PaintTile;
    }

    public void OnGUI()
    {
        Heading("Paint Brush", this);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        _SelectedColor = EditorGUILayout.ColorField("Color", _SelectedColor);
        SaveColor(_SelectedColor);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Radius", EditorStyles.boldLabel);
        _Radius = EditorGUILayout.IntSlider(_Radius,1,10);
        SaveDataInt("Paint Brush Radius", _Radius);
        GUILayout.EndHorizontal();

        Filters();
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
        GUILayout.Space(20);
        GUILayout.Label("Filters", EditorStyles.boldLabel);
        GUILayout.Space(10);

        string[] layers = InternalEditorUtility.layers;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Layer Mask");
        _LayerMask.value = EditorGUILayout.MaskField("", _LayerMask.value, layers);
        SaveDataInt("LayerMask Paint", _LayerMask.value);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tag");
        _Tag = EditorGUILayout.TextArea("");
        EditorGUILayout.EndHorizontal();
    }

    private void OnDestroy()
    {
        //DestroyImmediate(FindFirstObjectByType<PaintBrushTool>().gameObject);
    }
}
