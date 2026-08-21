using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeMaterialTool))]
public class ChangeMaterialToolController : ToolsController
{
    private ChangeMaterialWindow _MaterialWindow;

    private void OnSceneGUI()
    {
        if (ChangeMaterialWindow.Instance == null)
            return;

        _MaterialWindow = ChangeMaterialWindow.Instance;

        MoveTool(_MaterialWindow.currentSelector.transform);
        ChangeTileMaterial();

        SceneView.RepaintAll();
    }

    private void ChangeTileMaterial()
    {
        Event onClick = Event.current;

        if (onClick.type == EventType.MouseDown)
        {
            foreach (RaycastHit hit in DetectCollision(_MaterialWindow.currentSelector.transform.position, _MaterialWindow.radius))
            {
                GameObject selectedObject = hit.collider.gameObject;

                if (!CheckLayerMask(selectedObject, _MaterialWindow.layerMask, _MaterialWindow.tag))
                    return;

                Renderer renderer = selectedObject.GetComponent<Renderer>();

                Undo.RecordObject(renderer, "Changed Material");

                if (_MaterialWindow.selectedMaterial == null)
                    _MaterialWindow.selectedMaterial = renderer.sharedMaterial;
                else
                    renderer.sharedMaterial = _MaterialWindow.selectedMaterial;
            }
        }

    }
}
