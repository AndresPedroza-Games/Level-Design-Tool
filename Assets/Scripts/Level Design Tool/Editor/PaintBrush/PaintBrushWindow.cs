using UnityEditor;
using UnityEngine;

public class PaintBrushWindow : ToolWindowController
{
    private Color _SelectedColor;

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
    }

    private void PaintTile(SceneView sceneView)
    {
        if (DetectObject(EventType.MouseDown) != null)
            TilesCustomization.PaintMaterial(_SelectedColor, DetectObject(EventType.MouseDown));
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

    private void OnDestroy()
    {
        //DestroyImmediate(FindFirstObjectByType<PaintBrushTool>().gameObject);
    }
}
