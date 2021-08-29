using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AddPointsEvent : UnityEvent<int>
{

}

public class IntGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public IntGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public AddPointsEvent Response;

    private void OnEnable() => Event.RegisterListener(this);

    private void OnDisable() => Event.UnregisterListener(this);

    public void OnEventRaised(int points) => Response.Invoke(points);
}
