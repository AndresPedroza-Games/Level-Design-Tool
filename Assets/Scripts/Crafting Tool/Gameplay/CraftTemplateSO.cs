using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "CraftTemplateSO", menuName = "Crafting/new template")]
public class CraftTemplateSO : ScriptableObject
{
    public string templateName = "Name";
    public List<MaterialTemplateSO> materials;
    public PotionsSO resultPotion;

}
