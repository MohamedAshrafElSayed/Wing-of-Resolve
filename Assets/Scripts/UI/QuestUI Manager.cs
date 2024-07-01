using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public GameObject questHolder;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questDescriptionText;
    public GameObject objectiveHolder;
    public GameObject objectiveDescriptionPrefab;

    private QuestManager questManager;

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    public void UpdateQuestUI()
    {
        // Clear existing objectives in the UI
        foreach (Transform child in objectiveHolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Check if there is an active quest
        QuestData activeQuest = questManager.Quests.Find(quest => quest.QuestType == QuestType.Active);

        if (activeQuest != null)
        {
            questHolder.SetActive(true);

            // Update quest name and description
            questNameText.text = activeQuest.QuestName;
            questDescriptionText.text = activeQuest.QuestDescription;

            // Instantiate objectives
            foreach (ObjectiveData objective in activeQuest.Objectives)
            {
                GameObject objectiveDescription = Instantiate(objectiveDescriptionPrefab, objectiveHolder.transform);
                TextMeshProUGUI objectiveText = objectiveDescription.GetComponent<TextMeshProUGUI>();
                if (objectiveText != null)
                {
                    objectiveText.text = objective.Description;
                    if(objective.IsComplete == true)
                    {
                        objectiveText.fontStyle = FontStyles.Strikethrough;
                    }
                }
            }
        }
        else
        {
            questHolder.SetActive(false);
        }
    }
}