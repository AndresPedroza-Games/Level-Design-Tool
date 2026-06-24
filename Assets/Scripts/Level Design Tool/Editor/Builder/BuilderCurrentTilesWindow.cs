using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BuilderCurrentTilesWindow : ToolWindowController
{
    public TilesContainerSO _TileContainer;

    private Manager _Manager;
    private ScrollView _Scroll;

    public static void ShowWindow()
    {
        GetWindow<BuilderCurrentTilesWindow>();
    }

    private void OnGUI()
    {
        Heading(rootVisualElement, this);
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.currentPrefabs.CloneTree(rootVisualElement);

        _Scroll = rootVisualElement.Q<ScrollView>("Prefab-Scroll");

        _TileContainer = FindFirstObjectByType<Manager>().currentTilesContainer;

        if (_TileContainer != null)
            rootVisualElement.Q<ObjectField>("Container-Selector").value = _TileContainer;

        PopulateScroll(_Scroll);

    }

    private void PopulateScroll(ScrollView scrollView)
    {
        if (_TileContainer != null)
        {
            foreach (GameObject prefab in _TileContainer.tilesGameobjects)
            {
                VisualElement card = _Manager.currentPrefabsCard.CloneTree();

                card.style.width = 660;
                card.style.height = 150;
                card.style.marginBottom = 10;
                card.style.marginTop = 10;

                TilesTemplateSO currentTileTemplate = prefab.GetComponent<TilesController>().tileTemplate;

                card.Q<Label>("Name").text = currentTileTemplate.tileName;
                card.Q<Image>("Icon").sprite = currentTileTemplate.tileImg;

                scrollView.Add(card);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Tile Container is missing!", MessageType.Warning);
        }
    }

}
