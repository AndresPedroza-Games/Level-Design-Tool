using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PaintBrushTool))]
public class PaintBrushController : ToolsController
{
    public static PaintBrushController paintBrushController;
    
    private PaintBrushWindow _Brush;

    private void OnSceneGUI()
    {
        //if (PaintBrushWindow.Instance == null)
        //    return;

        _Brush = PaintBrushWindow.Instance;

        MoveTool(_Brush.currentBrush.transform);
        PaintTile();

        SceneView.RepaintAll();
    }

    private void PaintTile()
    {
        Event onClick = Event.current;

        if (onClick.type == EventType.MouseDown)
        {
            foreach (RaycastHit hit in DetectCollision(_Brush.currentBrush.transform.position, _Brush.radius))
            {
                GameObject gameObject = hit.collider.gameObject;

                if (!CheckLayerMask(gameObject, _Brush.layerMask, _Brush.tag))
                    return;

                TilesCustomization.PaintMaterial(_Brush.selectedColor, gameObject);

                EditorUtility.SetDirty(gameObject);
            }
        }
    }

}
