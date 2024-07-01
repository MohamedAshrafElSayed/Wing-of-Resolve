using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InteractableData _interactionData = null;
    [SerializeField] private InteractUI _uiInteraction;
    [SerializeField] private float _rayDistance = 0f;
    [SerializeField] private float _raySphereRadius = 0f;
    [SerializeField] private LayerMask _interactableLayer = ~0;

    private Camera _camera;
    private bool _interacting;

    void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        Ray _ray = new Ray(_camera.ViewportToWorldPoint(new Vector3(0.5f,0.5f)), _camera.transform.forward);
        RaycastHit _hitInfo;

        bool _hitSomething = Physics.SphereCast(_ray, _raySphereRadius, out _hitInfo, _rayDistance, _interactableLayer);

        if (_hitSomething)
        {
            InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();

            if (_interactable != null)
            {
                if (_interactionData.IsEmpty())
                {
                    _interactionData.Interactable = _interactable;
                    _uiInteraction.SetTooltip(_interactable.TooltipMessage);
                    _uiInteraction.Show();
                }
                else
                {
                    if (!_interactionData.IsSameInteractable(_interactable))
                    {
                        _interactionData.Interactable = _interactable;
                        _uiInteraction.SetTooltip(_interactable.TooltipMessage);
                        _uiInteraction.Show();
                    }
                }
            }
        }
        else
        {
            _uiInteraction.Hide();
            _interactionData.ResetData();
        }

        Debug.DrawRay(_ray.origin, _ray.direction * _rayDistance, _hitSomething ? Color.green : Color.red);
    }

    public void CheckForInteractableInput()
    {
        if (_interactionData.IsEmpty())
        {
            return;
        }

        if (!_interactionData.Interactable.IsInteractable)
        {
            return;
        }

        _interactionData.Interact();
    }
}
