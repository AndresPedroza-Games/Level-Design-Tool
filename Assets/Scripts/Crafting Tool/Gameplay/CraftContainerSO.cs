using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftContainerSO", menuName = "Crafting/Craft Container")]
public class CraftContainerSO : ScriptableObject
{
    public List<CraftTemplateSO> craftTemplates = new List<CraftTemplateSO>();
}
