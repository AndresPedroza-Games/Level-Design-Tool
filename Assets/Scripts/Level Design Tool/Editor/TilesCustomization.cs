using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(TilesController))]
public class TilesCustomization : Editor
{
    public static GameObject currentTile;

    private void OnSceneGUI()
    {
        //DrawHandler();
    }

    public static void PaintMaterial(Color newColor, GameObject tile, LayerMask layerMask)
    {

        Renderer renderer = tile.GetComponent<Renderer>();

        Material tileMaterial = renderer.sharedMaterial;

        renderer.sharedMaterial = new Material(renderer.sharedMaterial);

        Color currentColor = tileMaterial.color;

        EditorGUI.BeginChangeCheck();

        if (tileMaterial.color != newColor)
            renderer.sharedMaterial.color = newColor;

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tile, "Changed Color");
            renderer.sharedMaterial.color = currentColor;
        }
    }

    private void DrawHandler()
    {
        if (currentTile == null)
            return;

        Transform transform = currentTile.transform;

        EditorGUI.BeginChangeCheck();

        Vector3 newPos = Handles.PositionHandle(transform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(transform, "Changed Position");
            transform.position = newPos;
        }
    }

}
