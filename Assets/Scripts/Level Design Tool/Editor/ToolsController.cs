using UnityEditor;
using UnityEngine;

public class ToolsController : Editor
{
    protected void MoveTool(Transform tool)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            tool.transform.position = point;
        }
    }

    protected RaycastHit[] DetectCollision(Vector3 origin, float radius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.down * 0.1f);

        return hits;
    }

    protected bool CheckLayerMask(GameObject gameObject, LayerMask requireLayer, string tag)
    {
        bool layerMatch = (requireLayer.value & (1 << gameObject.layer)) != 0;
        bool tagMatch = false;

        if (!string.IsNullOrEmpty(tag))
        {
            tagMatch = gameObject.CompareTag(tag);
        }

        if (!layerMatch && !tagMatch)
        {
            Debug.Log("Layer Mask doens't Match");
            return false;
        }

        return true;
    }

}
