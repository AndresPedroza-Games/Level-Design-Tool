using UnityEngine;

public class AreaSelectorTool : ToolGizmo
{
    private void OnDisable()
    {
        base.freeze = false;
    }
}
