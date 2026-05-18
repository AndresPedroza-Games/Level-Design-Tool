using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements.Experimental;

public class EraseWindow : ToolWindowController
{
    private Manager manager;

    [MenuItem("Tools/Erase Tool")]
    public static void ShowWindow()
    {
        GetWindow<EraseWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += EraseObject;
        manager = FindFirstObjectByType<Manager>();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= EraseObject;
    }

    public void OnGUI()
    {
        Heading("Erase Tool", this);
    }

    private void EraseObject(SceneView sceneView)
    {
        if(DetectObject(EventType.MouseDown) != null)
        {
            manager.tilesContainer.tilesGameobjects.Remove(GetTileInList(DetectObject(EventType.MouseDown)));
            DestroyImmediate(DetectObject(EventType.MouseDown));
        }
    }

    private GameObject GetTileInList(GameObject tile)
    {
        foreach (GameObject tiles in manager.tilesContainer.tilesGameobjects)
        {
            if (tile.GetInstanceID() == tiles.GetInstanceID() && manager.tilesContainer.tilesGameobjects.Contains(tile))
                return tiles;
        }
        return null;
    }
}
