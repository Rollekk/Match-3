using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntGameEvent", menuName = "ScriptableObjects/IntGameEvent")]
public class IntGameEvent : ScriptableObject
{
    private readonly List<IntGameEventListener> eventListeners =
        new List<IntGameEventListener>();

    public void Raise(int points)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(points);
    }

    public void RegisterListener(IntGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(IntGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
