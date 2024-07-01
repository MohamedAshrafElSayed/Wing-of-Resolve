using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckPress Objective", menuName = "Quest System/Objective/CheckPress")]
public class PressKeyObjective : ObjectiveData
{
    [SerializeField] private KeyCode _keyToPress;
    public KeyCode KeyToPress { get => _keyToPress; set => _keyToPress = value; }

    private void OnEnable()
    {
        IsComplete = false;
    }

    public override void CheckCompletion()
    {
        if (Input.GetKeyDown(_keyToPress))
        {
            IsComplete = true;
        }
    }
}
