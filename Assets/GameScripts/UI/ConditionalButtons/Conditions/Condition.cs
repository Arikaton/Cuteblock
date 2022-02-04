using System;
using UnityEngine;

namespace GameScripts.UI.ConditionalButtons
{
    public abstract class Condition : MonoBehaviour
    {
        public abstract void Check(Action<bool> callback);
    }
}