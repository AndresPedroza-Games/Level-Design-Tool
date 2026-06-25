using UnityEngine;

public class ToolGizmo : MonoBehaviour
{
    public float radius = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
