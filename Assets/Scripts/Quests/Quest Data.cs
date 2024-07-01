using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [SerializeField] private string _questName;
    [SerializeField] private string _questDescription;
    [SerializeField] private QuestType _questType = QuestType.Inactive;
    [SerializeField] private bool _isCompleted = false;
    [SerializeField] private List<ObjectiveData> _objectives = new List<ObjectiveData>();

    public string QuestName { get => _questName; set => _questName = value; }
    public string QuestDescription { get => _questDescription; set => _questDescription = value; }
    public QuestType QuestType { get => _questType; set => _questType = value; }
    public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }
    public List<ObjectiveData> Objectives { get => _objectives; set => _objectives = value; }  
}
