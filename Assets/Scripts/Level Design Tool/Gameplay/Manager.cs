using UnityEngine;
using UnityEngine.UIElements;

public class Manager : MonoBehaviour
{
    [Header("UI Visual Elements Tools")]
    public VisualTreeAsset levelDesignToolWindow;
    public VisualTreeAsset paintBrushToolWindow;
    public VisualTreeAsset AreaSelectorToolWindow;
    public VisualTreeAsset changeMaterialToolWindow;
    public VisualTreeAsset builderToolWindow;
    public VisualTreeAsset eraserToolWindow;

    [Header("UI Visual Elements Templates")]
    public VisualTreeAsset header;
    public VisualTreeAsset filters;

    [Header("UI Visual Builder Templates")]
    public VisualTreeAsset currentPrefabs;
    public VisualTreeAsset confirmSelection;

    [Header("Elements In Scene")]
    public TilesContainerSO tilesContainer;

    [Header("Level Design Tools")]
    public GameObject paintBrushTool;
    public GameObject areaSelectorTool;


    [Header("Builder Tool")]
    public Transform spawnPoint;

}
