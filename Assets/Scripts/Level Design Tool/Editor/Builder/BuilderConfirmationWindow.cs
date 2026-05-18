using UnityEditor;
using UnityEngine;

public class BuilderConfirmationWindow : ToolWindowController
{
    public static GameObject currentTile;

    public static void ShowWindow()
    {
        GetWindow<BuilderConfirmationWindow>();
    }

    private void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        Confirm();
        Cancel();
        EditorGUILayout.EndHorizontal();
    }

    private void Confirm()
    {
        if (GUILayout.Button("Place") && currentTile != null)
        {
            currentTile.GetComponent<TilesController>().tileIsPlaced = true;
            currentTile.transform.position = currentTile.GetComponent<TilesController>().endPosition;

            FindFirstObjectByType<Manager>().tilesContainer.tilesGameobjects.Add(currentTile);
            ChangeWindow(BuilderWindow.ShowWindow,this);
        }
    }

    private void Cancel()
    {
        if (GUILayout.Button("Cancel") && !currentTile)
        {
            DestroyImmediate(currentTile);
            ChangeWindow(BuilderWindow.ShowWindow, this);
        }
    }

}
