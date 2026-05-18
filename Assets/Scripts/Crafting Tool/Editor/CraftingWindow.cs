using UnityEngine;
using UnityEditor;


public class CraftingWindow : EditorWindow
{
    public CraftContainerSO craftContainer;

    [MenuItem("Tools/Crafting Tool")]
    public static void ShowWindow()
    {
        GetWindow<CraftingWindow>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Crafting System Tool");
        craftContainer = (CraftContainerSO)EditorGUILayout.ObjectField(craftContainer, typeof(CraftContainerSO), false);
        ToolBar();

        if (craftContainer == null)
        {
            EditorGUILayout.HelpBox("Craft Container is missing!", MessageType.Warning);
            return;
        }

        GUILayout.Space(50);

        if (craftContainer.craftTemplates.Count > 0)
            for (int template = 0; template < craftContainer.craftTemplates.Count; template++)
            {
                CraftTemplateContainerBox(template);
                GUILayout.Space(40);
            }
    }

    private void ToolBar()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button($"Add new template"))
            craftContainer.craftTemplates.Add(CreateInstance<CraftTemplateSO>());

        if (GUILayout.Button($"Remove template"))
            craftContainer.craftTemplates.Remove(craftContainer.craftTemplates[craftContainer.craftTemplates.Count - 1]);

        if (GUILayout.Button("Empty list"))
            craftContainer.craftTemplates.Clear();

        GUILayout.EndHorizontal();
    }

    private void CraftTemplateContainerBox(int index)
    {
        EditorGUILayout.BeginVertical("Box");

        if (craftContainer.craftTemplates[index] == null)
        {
            EditorGUILayout.HelpBox("Craft Template is missing!", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Craft Name:");
        craftContainer.craftTemplates[index].templateName = EditorGUILayout.TextArea(craftContainer.craftTemplates[index].templateName);
        craftContainer.craftTemplates[index] = (CraftTemplateSO)EditorGUILayout.ObjectField(craftContainer.craftTemplates[index], typeof(CraftTemplateSO), false);
        GUILayout.Space(20);

        EditorGUILayout.LabelField("New Material");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
            craftContainer.craftTemplates[index].materials.Add(CreateInstance<MaterialTemplateSO>());

        if (GUILayout.Button("-"))
            craftContainer.craftTemplates[index].materials.Remove(craftContainer.craftTemplates[index].materials[craftContainer.craftTemplates[index].materials.Count - 1]);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if(craftContainer.craftTemplates[index].materials != null)
            for (int i = 0; i < craftContainer.craftTemplates[index].materials.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{i + 1}. Material name: {craftContainer.craftTemplates[index].materials[i].name}");

                craftContainer.craftTemplates[index].materials[i] = (MaterialTemplateSO)EditorGUILayout.ObjectField(craftContainer.craftTemplates[index].materials[i], typeof(MaterialTemplateSO), false);

                GUILayout.Space(10);

                EditorGUILayout.LabelField("Material Amount");
                string materialAmount = EditorGUILayout.TextArea(craftContainer.craftTemplates[index].materials[i].materialAmount.ToString());
                craftContainer.craftTemplates[index].materials[i].materialAmount = int.Parse(materialAmount);

                if (craftContainer.craftTemplates[index].materials[i].materialAmount <= 0)
                {
                    EditorGUILayout.HelpBox("Materials must be greater than 0!", MessageType.Warning);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20);
            }

        EditorGUILayout.LabelField("Result Potion");
        craftContainer.craftTemplates[index].resultPotion = (PotionsSO)EditorGUILayout.ObjectField(craftContainer.craftTemplates[index].resultPotion, typeof(PotionsSO), false);

        EditorGUILayout.EndVertical();
    }
}
