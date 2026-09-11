using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEventChannelSo", menuName = "Scriptable Objects/IntEventChannelSo")]
public class IntEventChannelSo : ScriptableObject
{
    public UnityAction<int> OnEventRaised;

    public void RaisedEvents(int value)
    {
        if(OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
    }
}
