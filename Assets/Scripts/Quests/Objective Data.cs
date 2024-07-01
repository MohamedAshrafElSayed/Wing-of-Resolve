using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Quest System/Objective")]
public class ObjectiveData : ScriptableObject
{
    [SerializeField] private string _description;
    [SerializeField] private ObjectiveType _type;
    [SerializeField] private List<ChoiceData> _choices = new List<ChoiceData>();
    [SerializeField] private bool _isComplete = false;

    public string Description { get => _description; set => _description = value; }
    public ObjectiveType Type { get => _type; set => _type = value; }
    public List<ChoiceData> Choices { get => _choices; set => _choices = value; }
    public bool IsComplete { get => _isComplete; set => _isComplete = value; }

    public virtual void CheckCompletion()
    {
        
    }
}
