using UnityEngine;

public class TilesController : MonoBehaviour
{
    public bool tileIsPlaced;
    public float placeHolderGap;
    public Vector3Int endPosition;
    public TilesTemplateSO tileTemplate;

    public bool isSelected;

    private void OnDrawGizmos()
    {
        placeHolderGap = 1f;

        if (tileIsPlaced == false)
        {
            Gizmos.color = Color.green;

            endPosition = Vector3Int.RoundToInt(new Vector3(transform.position.x, transform.position.y - placeHolderGap, transform.position.z));

            Gizmos.DrawWireCube(endPosition, transform.localScale);
        }

        Gizmos.color = Color.white;

        if (isSelected)
        {
            Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
        }

    }
}
