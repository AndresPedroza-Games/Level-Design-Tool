using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TilesContainerSO", menuName = "Builder /Tiles Container")]
public class TilesContainerSO : ScriptableObject
{
    public List<GameObject> tilesGameobjects = new List<GameObject>();
}
