using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckReaction.Common
{
    public class GameEvent
    {
        public string type { get; protected set; }
        public object param { get; protected set; }

        public GameEvent(string type, object param = null)
        {
            this.type = type;
            this.param = param;
        }

        public T GetParam<T>()
        {
            return (T)param;
        }
    }
}