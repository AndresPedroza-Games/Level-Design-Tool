using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TilesTemplateSO", menuName = "Builder /Tiles Template")]
public class TilesTemplateSO : ScriptableObject
{
    public string tileName;
    public GameObject tilePrefab;
    public Sprite tileImg;
}
