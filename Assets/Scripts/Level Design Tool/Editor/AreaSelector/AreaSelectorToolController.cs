using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AreaSelectorTool))]
public class AreaSelectorToolController : ToolsController
{
    private AreaSelectorWindow _AreaSelector;

    private void OnSceneGUI()
    {
        if (AreaSelectorWindow.Instance == null)
            return;

        _AreaSelector = AreaSelectorWindow.Instance;

        MoveTool(_AreaSelector.currentSelector.transform);
        SelectObjects();

        SceneView.RepaintAll();
    }

    private void SelectObjects()
    {
        Event onClick = Event.current;

        if (onClick.type == EventType.MouseDown)
        {
            foreach (RaycastHit hit in DetectCollision(_AreaSelector.currentSelector.transform.position, _AreaSelector.radius))
            {
                GameObject gameObject = hit.collider.gameObject;

                if (!CheckLayerMask(gameObject, _AreaSelector.layerMask, _AreaSelector.tag))
                    return;

                gameObject.GetComponent<TilesController>().isSelected = true;

            }
        }
    }
}
