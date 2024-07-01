using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Choice", menuName = "Quest System/Choice")]
public class ChoiceData : ScriptableObject
{
    [SerializeField] private string _choiceText;
    [SerializeField] private bool _isComplete = false;

    public string ChoiceText { get => _choiceText; set => _choiceText = value; }
    public bool IsComplete { get => _isComplete; set => _isComplete = value; }
}
