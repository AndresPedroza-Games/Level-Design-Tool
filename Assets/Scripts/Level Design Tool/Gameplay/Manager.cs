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
    public VisualTreeAsset tilePrefabsCard;
    public VisualTreeAsset currentPrefabsCard;
    public VisualTreeAsset confirmSelection;

    [Header("Elements In Scene")]
    public TilesContainerSO currentTilesContainer;

    [Header("Level Design Tools")]
    public GameObject paintBrushPrefab;
    public GameObject eraserPrefab;
    public GameObject changeMaterialPrefab;
    public GameObject areaSelectorPrefab;

    [Header("Builder Tool")]
    public Transform spawnPoint;

    [Header("Grid Settings")]
    public int gridSize = 1000;
    public float cellSize = 1f;


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;

    //    float halfSize = gridSize * cellSize * 0.5f;

    //    for (int x = -gridSize / 2; x <= gridSize / 2; x++)
    //    {
    //        float xPos = x * cellSize;

    //        Gizmos.DrawLine(new Vector3(xPos, 0, -halfSize),new Vector3(xPos, 0, halfSize));
    //    }

    //    for (int z = -gridSize / 2; z <= gridSize / 2; z++)
    //    {
    //        float zPos = z * cellSize;

    //        Gizmos.DrawLine(new Vector3(-halfSize, 0, zPos),new Vector3(halfSize, 0, zPos));
    //    }

    //    Gizmos.DrawLine(new Vector3(-halfSize, 0, 0),new Vector3(halfSize, 0, 0));

    //    Gizmos.DrawLine(new Vector3(0, 0, -halfSize),new Vector3(0, 0, halfSize)); 
    //}

}
