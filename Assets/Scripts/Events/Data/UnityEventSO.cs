using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/UnityEventSO")]
public class UnityEventSO : ScriptableObject
{
    private List<UnityEventListener> listeners = new List<UnityEventListener>();

    public void AddListener(UnityEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(UnityEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        foreach (UnityEventListener listener in listeners)
        {
            listener.EventRaised();
        }
    }
}
