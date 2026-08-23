using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class BuilderWindow : ToolWindowController
{
    private TilesContainerSO _TileContainer;
    private Manager _Manager;

    private GameObject currentTile;
    private ScrollView _Scroll;

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

        _TileContainer = (TilesContainerSO)rootVisualElement.Q<ObjectField>("Container-Selector").value;

        if (_TileContainer == null)
            return;

        if (_TileContainer.GetType() != typeof(TilesContainerSO))
        {
            EditorGUILayout.HelpBox("Tile Container is wrong!", MessageType.Warning);
            return;
        }

        SaveData(_TileContainer);

        SetButton(rootVisualElement, "Current-Prefabs-Button", () => ChangeWindow(BuilderCurrentTilesWindow.ShowWindow, this));
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.builderToolWindow.CloneTree(rootVisualElement);

        _Scroll = rootVisualElement.Q<ScrollView>("Prefab-Scroll");

        ObjectField tileContainerField = rootVisualElement.Q<ObjectField>("Container-Selector");

        if (_TileContainer != null)
            tileContainerField.value = _TileContainer;

        tileContainerField.RegisterValueChangedCallback(action => {_TileContainer = action.newValue as TilesContainerSO;PopulateScroll(_Scroll);});

        PopulateScroll(_Scroll);
    }

    private void PopulateScroll(ScrollView scrollView)
    {
        scrollView.Clear();

        if (_TileContainer != null)
        {
            foreach (GameObject prefab in _TileContainer.tilesGameobjects)
            {
                VisualElement card = _Manager.tilePrefabsCard.CloneTree();

                card.style.width = 660;
                card.style.height = 150;
                card.style.marginBottom = 10;
                card.style.marginTop = 10;

                TilesTemplateSO currentTileTemplate = prefab.GetComponent<TilesController>().tileTemplate;

                card.Q<Label>("Name").text = currentTileTemplate.tileName;
                card.Q<Image>("Icon").sprite = currentTileTemplate.tileImg;

                SetButton(card, "Instantiate", () => InstantiateTile(prefab));

                scrollView.Add(card);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Tile Container is missing!", MessageType.Warning);
        }
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
        currentTile = Instantiate(prefab, _Manager.spawnPoint.position, Quaternion.identity);

        BuilderConfirmationWindow.currentTile = this.currentTile;
        ChangeWindow(BuilderConfirmationWindow.ShowWindow, this);

        TilesCustomization.currentTile = this.currentTile;
    }

}
