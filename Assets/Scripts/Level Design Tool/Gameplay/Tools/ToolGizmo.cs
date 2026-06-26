using UnityEngine;

public class ToolGizmo : MonoBehaviour
{
    public float radius = 1f;
    public bool freeze;

    private void OnDrawGizmos()
    {
        if (!freeze)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
