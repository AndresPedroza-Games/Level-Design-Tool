using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;

public class BuilderWindow : ToolWindowController
{
    private TilesContainerSO _TileContainer;
    private Manager manager;

    private GameObject currentTile;


    [MenuItem("Tools/Builder Window %M")]
    public static void ShowWindow()
    {
        GetWindow<BuilderWindow>();
    }

    private void OnEnable()
    {
        if(_TileContainer == null)
            _TileContainer = LoadData();
    }

    private void OnGUI()
    {
         manager = FindFirstObjectByType<Manager>();

        Heading("Builder", this);
        GUILayout.Space(20);

        _TileContainer = (TilesContainerSO)EditorGUILayout.ObjectField(_TileContainer, typeof(TilesContainerSO), false);
        SaveData(_TileContainer);
        GUILayout.Space(20);

        EditorGUILayout.BeginVertical("box");

        if (_TileContainer != null)
        {
            for (int index = 0; index < _TileContainer.tilesGameobjects.Count; index++)
            {
                BuilderTilesContainerBox(index);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Tile Container is missing!", MessageType.Warning);
        }
        
        GUILayout.Space(20);
        CurrentTilesInScene();

        EditorGUILayout.EndVertical();

    }

    private void SaveData(TilesContainerSO tilesContainer)
    {
        string dataPath = AssetDatabase.GetAssetPath(tilesContainer);

        EditorPrefs.SetString("Container Path", dataPath);
    }

    private TilesContainerSO LoadData()
    {
        string dataPath = EditorPrefs.GetString("Container Path");

        return AssetDatabase.LoadAssetAtPath<TilesContainerSO>(dataPath);
    }

    private void InstantiateTile(GameObject prefab)
    {
        if (GUILayout.Button("Instantiate"))
        {
            currentTile = Instantiate(prefab, manager.spawnPoint.position, Quaternion.identity, manager.gameObject.transform);

            BuilderConfirmationWindow.currentTile = this.currentTile;
            ChangeWindow(BuilderConfirmationWindow.ShowWindow, this);

            TilesCustomization.currentTile = this.currentTile;
        }
    }

    private void BuilderTilesContainerBox(int index)
    {
        if (_TileContainer.tilesGameobjects[index] == null)
            return;

        TilesTemplateSO currentTileTemplate = _TileContainer.tilesGameobjects[index].GetComponent<TilesController>().tileTemplate;

        EditorGUILayout.BeginHorizontal("Box");

        GUILayout.Box(currentTileTemplate.tileImg, GUILayout.Width(100), GUILayout.Height(100));

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label(currentTileTemplate.tileName);
        InstantiateTile(currentTileTemplate.tilePrefab);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

    private void CurrentTilesInScene()
    {
        if(GUILayout.Button("Current Prefabs"))
        {
            ChangeWindow(BuilderCurrentTilesWindow.ShowWindow, this);
        }
    }
}
