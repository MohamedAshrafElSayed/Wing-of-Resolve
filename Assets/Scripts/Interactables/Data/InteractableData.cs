using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionDataSO", menuName = "Game Data/InteractionSO")]
public class InteractableData : ScriptableObject
{
    private InteractableBase _interactable;

    public InteractableBase Interactable
    {
        get => _interactable;
        set => _interactable = value;
    }

    public void Interact()
    {
        _interactable.OnInteract();
        ResetData();
    }

    public bool IsSameInteractable(InteractableBase _newInteractable) => _interactable == _newInteractable;
    public bool IsEmpty() => _interactable == null;
    public void ResetData() => _interactable = null;
}
