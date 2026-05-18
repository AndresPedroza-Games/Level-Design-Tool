using UnityEditor;
using UnityEngine;

public class BuilderCurrentTilesWindow : ToolWindowController
{
    public TilesContainerSO _TileContainer;

    public static void ShowWindow()
    {
        GetWindow<BuilderCurrentTilesWindow>();
    }

    private void OnGUI()
    {
        _TileContainer = FindFirstObjectByType<Manager>().tilesContainer;

        GUILayout.Space(20);
        Heading("Current Prefabs in Scene", this);

        if (_TileContainer != null)
        {
            for (int index = 0; index < _TileContainer.tilesGameobjects.Count; index++)
            {
                CurrentTilesInScene(index);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Add the tile container in the manager!", MessageType.Warning);
            return;
        }
    }

    private void CurrentTilesInScene(int index)
    {
        TilesTemplateSO currentTileTemplate = _TileContainer.tilesGameobjects[index].GetComponent<TilesController>().tileTemplate;

        EditorGUILayout.BeginHorizontal("Box");

        GUILayout.Box(currentTileTemplate.tileImg);

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label(currentTileTemplate.tileName);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

}
