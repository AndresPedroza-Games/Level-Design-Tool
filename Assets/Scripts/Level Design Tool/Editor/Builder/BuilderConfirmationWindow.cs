using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BuilderConfirmationWindow : ToolWindowController
{
    public static GameObject currentTile;
    private Manager _Manager;

    public static void ShowWindow()
    {
        GetWindow<BuilderConfirmationWindow>();
    }

    private void OnGUI()
    {
        SetButton(rootVisualElement, "Confirm", Confirm);
        SetButton(rootVisualElement, "Cancel", Cancel);
    }

    public void CreateGUI()
    {
        _Manager = FindFirstObjectByType<Manager>();
        _Manager.confirmSelection.CloneTree(rootVisualElement);
    }

    private void Confirm()
    {
        if (currentTile != null)
        {
            currentTile.GetComponent<TilesController>().tileIsPlaced = true;
            currentTile.transform.position = currentTile.GetComponent<TilesController>().endPosition;

            _Manager.tilesContainer.tilesGameobjects.Add(currentTile);
            ChangeWindow(BuilderWindow.ShowWindow, this);
        }
    }

    private void Cancel()
    {
        if (currentTile)
        {
            DestroyImmediate(currentTile);
            ChangeWindow(BuilderWindow.ShowWindow, this);
        }
    }

}
