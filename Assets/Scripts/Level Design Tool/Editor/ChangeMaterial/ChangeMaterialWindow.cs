using UnityEditor;
using UnityEngine;

public class ChangeMaterialWindow : ToolWindowController
{
    private Material _SelectedMaterial;
    private Material _NewMaterial;
    
    [MenuItem("Tools/Change Material Window")]
    public static void ShowWindow()
    {
        GetWindow<ChangeMaterialWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += ChangeTileMaterial;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= ChangeTileMaterial;
    }

    private void OnGUI()
    {
        Heading("Change Material Window", this);
        GUILayout.Space(20);

        if(_SelectedMaterial == null)
            EditorGUILayout.HelpBox("No Material is selected!", MessageType.Warning);
        else
            EditorGUILayout.HelpBox("Place your new material!", MessageType.Warning);
    }

    private void ChangeTileMaterial(SceneView sceneView)
    {
        EventType onClick = EventType.MouseDown;

        if (DetectObject(onClick) != null)
        {
            GameObject selectedObject = DetectObject(onClick);

            Renderer renderer = selectedObject.GetComponent<Renderer>();

            if (_SelectedMaterial == null)
                _SelectedMaterial = renderer.sharedMaterial;
            else
                renderer.sharedMaterial = _SelectedMaterial;
        }   
    }
}
