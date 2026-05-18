using UnityEngine;

public class PaintBrushTool : MonoBehaviour
{
    public float radius = 1f;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
