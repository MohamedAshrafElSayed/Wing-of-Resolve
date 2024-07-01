using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkToNPC Objective", menuName = "Quest System/Objective/Talk To NPC")]
public class TalkToNPCObjective : ObjectiveData
{
    [SerializeField] private GameObject _targetNPC;
    public GameObject TargetNPC { get => _targetNPC; set => _targetNPC = value; }

    private void OnEnable()
    {
        IsComplete = false;
    }

    public override void CheckCompletion()
    {
        if (_targetNPC != null && _targetNPC.activeInHierarchy)
        {
            IsComplete = true;
        }
    }
}
