using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventListener : MonoBehaviour
{
    public UnityEventSO _event;
    public UnityEvent _response;

    private void OnEnable()
    {
        _event.AddListener(this);
    }

    private void OnDisable()
    {
        _event.RemoveListener(this);
    }

    public void EventRaised()
    {
        _response.Invoke();
    }
}

