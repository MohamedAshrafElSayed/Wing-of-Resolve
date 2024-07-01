using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.AccessControl;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{
    private QuestManager questManager;
    private string _questName = "";
    private SerializedProperty questUpdateEventProperty;
    private List<bool> foldouts = new List<bool>();
    private string saveFolderPath = "Assets/Scripts/Quests/Data";

    private void OnEnable()
    {
        questManager = (QuestManager)target;
        questUpdateEventProperty = serializedObject.FindProperty("_questUpdate"); 
        InitializeFoldouts();
        LoadQuests();
    }

    private void InitializeFoldouts()
    {
        foldouts.Clear();
        for (int i = 0; i < questManager.Quests.Count; i++)
        {
            foldouts.Add(false);
        }
    }

    private void LoadQuests()
    {
        string saveFolderPath = "Assets/Scripts/Quests/Data";
        string[] questAssetPaths = AssetDatabase.FindAssets("t:QuestData", new[] { saveFolderPath });
        foreach (string assetPath in questAssetPaths)
        {
            QuestData quest = AssetDatabase.LoadAssetAtPath<QuestData>(AssetDatabase.GUIDToAssetPath(assetPath));
            if (quest != null && !questManager.Quests.Contains(quest))
            {
                questManager.Quests.Add(quest);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // Display UnityEvent field in the inspector
        EditorGUILayout.PropertyField(questUpdateEventProperty);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Quests", EditorStyles.boldLabel);

        // Display current quests
        for (int i = 0; i < questManager.Quests.Count; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "Quest " + (i + 1), true, EditorStyles.foldout);
            if (GUILayout.Button("Remove"))
            {
                RemoveQuest(i);
                continue; 
            }
            EditorGUILayout.EndHorizontal();

            if (foldouts[i])
            {
                EditorGUI.indentLevel++;
                QuestData quest = questManager.Quests[i];

                // Quest name
                EditorGUILayout.LabelField("Name");
                quest.QuestName = EditorGUILayout.TextField(quest.QuestName);

                // Quest description
                EditorGUILayout.LabelField("Description");
                quest.QuestDescription = EditorGUILayout.TextArea(quest.QuestDescription, GUILayout.Height(50));

                // Quest type
                quest.QuestType = (QuestType)EditorGUILayout.EnumPopup("Type", quest.QuestType);

                // Display objectives list for each quest
                DisplayObjectives(quest);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        // Add new quest button
        EditorGUILayout.Space();

        // Quest name
        _questName = EditorGUILayout.TextField("Quest Name: ", _questName);

        if (GUILayout.Button("Add Quest"))
        {
            AddQuest(_questName);
            foldouts.Add(true);
        }

        // Save changes
        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayObjectives(QuestData quest)
    {
        EditorGUILayout.LabelField("Objectives", EditorStyles.boldLabel);
        
        for (int j = 0; j < quest.Objectives.Count; j++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Objective " + (j + 1), EditorStyles.boldLabel);
            if (GUILayout.Button("Remove"))
            {
                RemoveObjective(quest, j);
                continue;
            }
            EditorGUILayout.EndHorizontal();
            ObjectiveData objective = quest.Objectives[j];

            // Objective description
            EditorGUILayout.LabelField("Description");
            objective.Description = EditorGUILayout.TextField(objective.Description);

            // Display objective type dropdown
            objective.Type = (ObjectiveType)EditorGUILayout.EnumPopup("Type", objective.Type);

            // Display additional properties based on the objective type
            switch (objective.Type)
            {
                case ObjectiveType.PressKey:
                    PressKeyObjective pressKeyObjective = (PressKeyObjective)objective;
                    pressKeyObjective.KeyToPress = (KeyCode)EditorGUILayout.EnumPopup("Key To Press", pressKeyObjective.KeyToPress);
                    break;
                case ObjectiveType.Collect:
                    CollectObjective collectObjective = (CollectObjective)objective;
                    collectObjective.CollectibleObject = (GameObject)EditorGUILayout.ObjectField("Collectible Object", collectObjective.CollectibleObject, typeof(GameObject), true);
                    collectObjective.RequiredCount = EditorGUILayout.IntField("Required Count", collectObjective.RequiredCount);
                    break;
                case ObjectiveType.WaitTime:
                    WaitTimeObjective waitTimeObjective = (WaitTimeObjective)objective;
                    waitTimeObjective.RequiredTime = EditorGUILayout.FloatField("Target Time", waitTimeObjective.RequiredTime);
                    break;
                case ObjectiveType.TalkToNPC:
                    TalkToNPCObjective talkToNPCObjective = (TalkToNPCObjective)objective;
                    talkToNPCObjective.TargetNPC = (GameObject)EditorGUILayout.ObjectField("NPC Object", talkToNPCObjective.TargetNPC , typeof(GameObject), true);
                    break;
                default:
                    Debug.LogError("Unknown objective type: " + objective.Type);
                    break;
            }

            // Objective completion status
            objective.IsComplete = EditorGUILayout.Toggle("Completed", objective.IsComplete);
            
            // Display choices for each objective
            DisplayChoices(objective);

            EditorGUILayout.EndVertical();
        }

        // Add new objective button
        EditorGUILayout.Space();
        if (GUILayout.Button("Add Objective"))
        {
            AddObjective(quest);
        }
    }

    private void DisplayChoices(ObjectiveData objective)
    {
        EditorGUILayout.LabelField("Choices", EditorStyles.boldLabel);
        int k = 0;
        for (; k < objective.Choices.Count; k++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Choice " + (k + 1), EditorStyles.boldLabel);
            if (GUILayout.Button("Remove"))
            {
                RemoveChoice(objective, k);
                continue;
            }
            EditorGUILayout.EndHorizontal();
            ChoiceData choice = objective.Choices[k];

            // Choice text
            choice.ChoiceText = EditorGUILayout.TextField(choice.ChoiceText);

            // Choice completion status
            choice.IsComplete = EditorGUILayout.Toggle("Completed", choice.IsComplete);

            EditorGUILayout.EndVertical();
        }

        // Add new choice button
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Choice", GUILayout.Width(100)))
        {
            AddChoice(objective,k);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddQuest(string questName)
    {
        // Ensure the quest name is not empty
        if (!string.IsNullOrEmpty(questName))
        {
            // Add the quest with the entered name
            QuestData newQuest = questManager.AddQuest(questName, "Quest Description");
            string questDirectory = Path.Combine(saveFolderPath, questName);
            if (!Directory.Exists(questDirectory))
            {
                Directory.CreateDirectory(questDirectory);
            }
            string questPath = Path.Combine(questDirectory, questName + ".asset");
            AssetDatabase.CreateAsset(newQuest, questPath);
            AssetDatabase.SaveAssets();
            foldouts.Add(true);
        }
        else
        {
            Debug.LogWarning("Quest name cannot be empty.");
        }
        _questName = "";
    }

    private void RemoveQuest(int index)
    {
        if (index >= 0 && index < questManager.Quests.Count)
        {
            string questFolderPath = Path.Combine(saveFolderPath, questManager.Quests[index].QuestName);
            string questPath = AssetDatabase.GetAssetPath(questManager.Quests[index]);

            AssetDatabase.DeleteAsset(questPath);

            // Delete objectives folder and its contents
            if (Directory.Exists(questFolderPath))
            {
                string[] objectivesFiles = Directory.GetFiles(questFolderPath);
                foreach (string file in objectivesFiles)
                {
                    File.Delete(file);
                }

                Directory.Delete(questFolderPath);
            }
            questManager.RemoveQuest(questManager.Quests[index]);
            foldouts.RemoveAt(index);
        }
    }

    private void AddObjective(QuestData quest)
    {
        string description = "New Objective";
        int index = quest.Objectives.Count + 1;
        ObjectiveData newObjective = null;

        // Ask for the type of the objective
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < System.Enum.GetValues(typeof(ObjectiveType)).Length; i++)
        {
            ObjectiveType currentType = (ObjectiveType)i; 
            menu.AddItem(new GUIContent(currentType.ToString()), false, () =>
            {
                newObjective = questManager.AddObjective(quest, description, index, currentType);
                // Create the directory path for the objective
                string objectiveDirectory = Path.Combine(saveFolderPath, quest.QuestName);
                if (!Directory.Exists(objectiveDirectory))
                {
                    Directory.CreateDirectory(objectiveDirectory);
                }

                // Create the asset path for the objective
                string objectivePath = Path.Combine(objectiveDirectory, "Objective " + index + ".asset");

                // Check if the asset already exists
                if (!AssetDatabase.LoadAssetAtPath<ObjectiveData>(objectivePath))
                {
                    // Create the asset if it doesn't exist
                    AssetDatabase.CreateAsset(newObjective, objectivePath);
                    AssetDatabase.SaveAssets();
                    quest.Objectives.Add(newObjective);
                }
                else
                {
                    // Asset already exists, return null or handle the error as needed
                    Debug.LogError("Objective asset already exists at path: " + objectivePath);
                }
            });
        }
        menu.ShowAsContext();
    }

    private void RemoveObjective(QuestData quest, int objectiveIndex)
    {
        if (objectiveIndex >= 0 && objectiveIndex < quest.Objectives.Count)
        {
            string objectivePath = AssetDatabase.GetAssetPath(quest.Objectives[objectiveIndex]);
            AssetDatabase.DeleteAsset(objectivePath);
            questManager.RemoveObjective(quest, quest.Objectives[objectiveIndex]);
        }
    }

    private void AddChoice(ObjectiveData objective, int index)
    {
        ChoiceData newChoice = null;
        newChoice = questManager.AddChoice(objective, "New Choice", questManager.GetQuestName(objective), (index + 1));

        // Create the directory path for the choice
        string choiceDirectory = Path.Combine(saveFolderPath, questManager.GetQuestName(objective), objective.name);
        if (!Directory.Exists(choiceDirectory))
        {
            Directory.CreateDirectory(choiceDirectory);
        }

        // Create the asset path for the choice
        string choicePath = Path.Combine(choiceDirectory, "Choice " + (index + 1) + ".asset");

        // Check if the asset already exists
        if (!AssetDatabase.LoadAssetAtPath<ChoiceData>(choicePath))
        {
            // Create the asset if it doesn't exist
            AssetDatabase.CreateAsset(newChoice, choicePath);
            AssetDatabase.SaveAssets();
            objective.Choices.Add(newChoice);
        }
        else
        {
            // Asset already exists, return null or handle the error as needed
            Debug.LogError("Choice asset already exists at path: " + choicePath);
        }
    }

    private void RemoveChoice(ObjectiveData objective, int choiceIndex)
    {
        if (choiceIndex >= 0 && choiceIndex < objective.Choices.Count)
        {
            string choicePath = AssetDatabase.GetAssetPath(objective.Choices[choiceIndex]);
            AssetDatabase.DeleteAsset(choicePath);
            questManager.RemoveChoice(objective, objective.Choices[choiceIndex]);
        }
    }
}