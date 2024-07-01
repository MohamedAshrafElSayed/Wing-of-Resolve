using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    private List<QuestData> _quests = new List<QuestData>();
    public UnityEvent _questUpdate;
    public List<QuestData> Quests { get => _quests; set => _quests = value; }

    private void Update()
    {
        foreach(QuestData data in Quests)
        {
            if(data.QuestType == QuestType.Active)
            {
                CheckObjectiveCompletion(data);
            }
        }    
    }

    public void CheckObjectiveCompletion(QuestData quest)
    {
        foreach (ObjectiveData objective in quest.Objectives)
        {
            objective.CheckCompletion();
            if (objective.IsComplete == true)
            {
                CheckQuestCompletion(quest);
            }
        }
    }

    private void Awake()
    {
        Debug.Log("Awake");
        ResetQuests();
    }

    private void Start()
    {
        Debug.Log("Start");
        ResetQuests();
    }

    // Method to reset all quest types to Inactive
    public void ResetQuests()
    {
        foreach (QuestData quest in Quests)
        {
            Debug.Log(quest.QuestType);
            quest.QuestType = QuestType.Inactive;
            Debug.Log(quest.QuestType);
        }
    }

    // Method to add a new quest
    public QuestData AddQuest(string questName, string description)
    {
        QuestData newQuest = ScriptableObject.CreateInstance<QuestData>();
        newQuest.QuestName = questName;
        newQuest.QuestDescription = description;
        newQuest.QuestType = QuestType.Inactive;        
        Quests.Add(newQuest);
        return newQuest;
    }

    // Method to remove a quest
    public void RemoveQuest(QuestData quest)
    {
        Quests.Remove(quest);
    }   

    // Method to start a quest (change its type to Active)
    public void StartQuest(QuestData quest)
    {
        quest.QuestType = QuestType.Active;
        _questUpdate?.Invoke();
    }

    // Method to complete a quest (change its type to Completed)
    public void CompleteQuest(QuestData quest)
    {
        quest.QuestType = QuestType.Completed;
        _questUpdate?.Invoke();
    }

    // Method to check if all objectives in a quest are completed
    public bool CheckQuestCompletion(QuestData quest)
    {
        bool allObjectivesCompleted = true;
        foreach (ObjectiveData objective in quest.Objectives)
        {
            if (!objective.IsComplete)
            {
                allObjectivesCompleted = false;
                break;
            }
        }
        if (allObjectivesCompleted)
        {
            CompleteQuest(quest);
            return true;
        }
        return allObjectivesCompleted;
    }

    // Method to add an objective to a quest
    public ObjectiveData AddObjective(QuestData quest, string description, int index, ObjectiveType type)
    {
        ObjectiveData newObjective = null;

        switch (type)
        {
            case ObjectiveType.PressKey:
                newObjective = ScriptableObject.CreateInstance<PressKeyObjective>();
                newObjective.Type = ObjectiveType.PressKey;
                break;
            case ObjectiveType.Collect:
                newObjective = ScriptableObject.CreateInstance<CollectObjective>();
                newObjective.Type = ObjectiveType.Collect;
                break;
            case ObjectiveType.WaitTime:
                newObjective = ScriptableObject.CreateInstance<WaitTimeObjective>();
                newObjective.Type = ObjectiveType.WaitTime;
                break;
            case ObjectiveType.TalkToNPC:
                newObjective = ScriptableObject.CreateInstance<TalkToNPCObjective>();
                newObjective.Type = ObjectiveType.TalkToNPC;
                break;
            default:
                Debug.LogError("Unknown objective type: " + type);
                break;
        }

        if (newObjective != null)
        {
            newObjective.Description = description;

            // Additional setup based on the objective type
            if (newObjective is PressKeyObjective pressObjective)
            {
                // Set default KeyCode for CheckPressObjective
                pressObjective.KeyToPress = KeyCode.Space;
            }
            else if (newObjective is CollectObjective collectObjective)
            {
                // Set default values for CollectObjective
                collectObjective.CollectibleObject = null;
                collectObjective.RequiredCount = 1; 
            }
            else if (newObjective is WaitTimeObjective waitObjective)
            {
                // Set default value for WaitTimeObjective
                waitObjective.RequiredTime = 5f; 
            }
            else if (newObjective is TalkToNPCObjective npcObjective)
            {
                // Set default NPC object for TalkToNPCObjective
                npcObjective.TargetNPC = null; 
            }

            return newObjective;
        }
        return null;
    }

    // Method to remove an objective from a quest
    public void RemoveObjective(QuestData quest, ObjectiveData objective)
    {
        quest.Objectives.Remove(objective);
    }

    // Method to add a choice to an objective
    public ChoiceData AddChoice(ObjectiveData objective, string choiceText, string questName, int index)
    {
        ChoiceData newChoice = ScriptableObject.CreateInstance<ChoiceData>();
        newChoice.ChoiceText = choiceText;
        return newChoice;
    }

    // Method to remove a choice from an objective
    public void RemoveChoice(ObjectiveData objective, ChoiceData choice)
    {
        objective.Choices.Remove(choice);
    }

    // Method to get the name of the quest associated with an objective
    public string GetQuestName(ObjectiveData objective)
    {
        foreach (QuestData quest in Quests)
        {
            if (quest.Objectives.Contains(objective))
            {
                return quest.QuestName;
            }
        }
        return null;
    }
}