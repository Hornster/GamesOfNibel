using System;
using UnityEngine.Events;

namespace Assets.Scripts.Common.CustomEvents
{
    /// <summary>
    /// A custom class for inspector-visible events in Unity that accept single integer as argument. 
    /// </summary>
    [Serializable]
    public class IntegerUnityEvent: UnityEvent<int>
    {

    }
}
