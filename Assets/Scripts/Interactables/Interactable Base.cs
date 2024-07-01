using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _multipleUse = false;
    [SerializeField] private bool _isInteractable = true;

    [SerializeField] private string _tooltipMessage = "interact";

    public bool MultipleUse => _multipleUse;
    public bool IsInteractable => _isInteractable;
    public string TooltipMessage => _tooltipMessage;

    public virtual void OnInteract()
    {
        Debug.Log("INTERACTED: " + gameObject.name);

    }
}
