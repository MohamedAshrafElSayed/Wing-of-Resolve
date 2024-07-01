using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitTime Objective", menuName = "Quest System/Objective/WaitTime")]
public class WaitTimeObjective : ObjectiveData
{
    [SerializeField] private float _requiredTime;
    private float _currentTime = 0f;
    private bool _isWaiting = false;

    public float RequiredTime { get => _requiredTime; set => _requiredTime = value; }

    private void OnEnable()
    {
        IsComplete = false;
    }

    public override void CheckCompletion()
    {
        // If not already waiting, start waiting
        if (!_isWaiting)
        {
            _currentTime = 0f;
            _isWaiting = true;
        }

        // Increment the current time
        _currentTime += Time.deltaTime;

        // Check if the current time exceeds the target time
        if (_currentTime >= _requiredTime)
        {
            IsComplete = true;
            _isWaiting = false;
        }
    }
}
