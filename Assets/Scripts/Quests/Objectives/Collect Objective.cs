using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collect Objective", menuName = "Quest System/Objective/Collect")]
public class CollectObjective : ObjectiveData
{
    [SerializeField] private GameObject _collectibleObject;
    [SerializeField] private int _requiredCount = 1;

    private int _currentCount = 0;

    public GameObject CollectibleObject { get => _collectibleObject; set => _collectibleObject = value; }
    public int RequiredCount { get => _requiredCount; set => _requiredCount = value; }

    private void OnEnable()
    {
        IsComplete = false;
    }

    // Method to increment the collect count when a collectible is collected
    public void CollectObject()
    {
        _currentCount++;
        CheckCompletion();
    }

    public override void CheckCompletion()
    {
        if (_currentCount >= _requiredCount)
        {
            IsComplete = true;
        }
    }
}
