using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [Header("Elements In Scene")]
    public TilesContainerSO tilesContainer;

    [Header("Level Design Tools")]
    public GameObject paintBrushTool;
    public GameObject areaSelectorTool;


    [Header("Builder Tool")]
    public Transform spawnPoint;

}
