using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EraserTool))]
public class EraserToolController : ToolsController
{
    private EraseWindow _Eraser;

    private void OnSceneGUI()
    {
        if (EraseWindow.Instance == null)
            return;

        _Eraser = EraseWindow.Instance;

        MoveTool(_Eraser.currentEraser.transform);
        EraseObjects();

        SceneView.RepaintAll();
    }

    private void EraseObjects()
    {
        Event onClick = Event.current;

        if (onClick.type == EventType.MouseDown)
        {
            foreach (RaycastHit hit in DetectCollision(_Eraser.currentEraser.transform.position, _Eraser.radius))
            {
                GameObject gameObject = hit.collider.gameObject;

                if (!CheckLayerMask(gameObject, _Eraser.layerMask, _Eraser.tag))
                    return;

                _Eraser.manager.currentTilesContainer.tilesGameobjects.Remove(GetTileInList(gameObject));
                DestroyImmediate(gameObject);
            }
        }
    }

    private GameObject GetTileInList(GameObject tile)
    {
        foreach (GameObject tiles in _Eraser.manager.currentTilesContainer.tilesGameobjects)
        {
            if (tile.GetInstanceID() == tiles.GetInstanceID() && _Eraser.manager.currentTilesContainer.tilesGameobjects.Contains(tile))
                return tiles;
        }
        return null;
    }
}
