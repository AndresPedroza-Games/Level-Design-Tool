using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BuilderWindow : ToolWindowController
{
    private TilesContainerSO _TileContainer;
    private Manager _Manager;

    private GameObject currentTile;
    private Vector2 scrollPos;


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

    public void OnGUI()
    {
        Heading(rootVisualElement, this);

        _TileContainer = (TilesContainerSO)EditorGUILayout.ObjectField(_TileContainer, typeof(TilesContainerSO), false);
        SaveData(_TileContainer);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

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

        SetButton(rootVisualElement, "Current-Prefabs-Button", () => ChangeWindow(BuilderCurrentTilesWindow.ShowWindow, this));

    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.builderToolWindow.CloneTree(rootVisualElement);
    }

    private void SaveData(TilesContainerSO tilesContainer)
    {
        string dataPath = AssetDatabase.GetAssetPath(tilesContainer);

        SaveDataString("Container Path",dataPath);
    }

    private TilesContainerSO LoadData()
    {
        string dataPath = LoadDataString("Container Path");

        return AssetDatabase.LoadAssetAtPath<TilesContainerSO>(dataPath);
    }

    private void InstantiateTile(GameObject prefab)
    {
        currentTile = Instantiate(prefab, _Manager.spawnPoint.position, Quaternion.identity, _Manager.gameObject.transform);

        BuilderConfirmationWindow.currentTile = this.currentTile;
        ChangeWindow(BuilderConfirmationWindow.ShowWindow, this);

        TilesCustomization.currentTile = this.currentTile;
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
        SetButton(rootVisualElement, "", () => InstantiateTile(currentTileTemplate.tilePrefab));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

}
