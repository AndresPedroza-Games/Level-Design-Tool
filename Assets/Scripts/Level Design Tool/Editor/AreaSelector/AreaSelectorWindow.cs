using UnityEngine;
using UnityEditor;

public class AreaSelectorWindow : ToolWindowController
{
    [MenuItem("Tools/Area Selector Tool")]

    public static void ShowWindow()
    {
        Manager manager = FindFirstObjectByType<Manager>();
        GetWindow<AreaSelectorWindow>();

        GameObject areaSelector = Instantiate(manager.areaSelectorTool);
        areaSelector.GetComponent<AreaSelectorTool>().manager = manager;
    }

    public void OnGUI()
    {
        Heading("Area Selector", this);

    }

    private void ToolConfig(string configName)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(configName);
        GUILayout.HorizontalSlider(0.5f,0f,1f);
        GUILayout.EndHorizontal();
    }

    private void OnDestroy()
    {
        DestroyImmediate(FindFirstObjectByType<AreaSelectorTool>().gameObject);
    }

}
