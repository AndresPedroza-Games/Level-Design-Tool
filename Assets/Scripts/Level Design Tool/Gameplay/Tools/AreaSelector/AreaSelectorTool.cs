using UnityEngine;

public class AreaSelectorTool : MonoBehaviour
{
    [Header("Area Selector config")]
    public float radius = 1f;


    public Manager manager;

    private void OnDrawGizmos()
    {
        //if (manager.elementsInScene != null)
        //    foreach (GameObject tile in manager.elementsInScene)
        //    {
        //        Gizmos.color = Color.white;
        //        Gizmos.DrawWireCube(tile.transform.position, tile.transform.localScale);
        //    }
    }
}
