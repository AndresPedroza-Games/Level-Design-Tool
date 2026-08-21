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

    public static void PaintMaterial(Color newColor, GameObject tile)
    {
        Renderer renderer = tile.GetComponent<Renderer>();

        Material tileMaterial = renderer.sharedMaterial;

        Undo.RecordObject(renderer, "Changed Color");

        renderer.sharedMaterial = new Material(renderer.sharedMaterial);

        Color currentColor = tileMaterial.color;

        EditorGUI.BeginChangeCheck();

        if (tileMaterial.color != newColor)
            renderer.sharedMaterial.color = newColor;

        if (EditorGUI.EndChangeCheck())
        {
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
