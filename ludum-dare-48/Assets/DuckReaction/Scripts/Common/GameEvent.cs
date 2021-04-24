using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckReaction.Common
{
    public enum GameEventType
    {
        GAME_START,
        GAME_FINISHED,
        GAME_PAUSE,
        GAME_RESET,
        LEVEL_UP,
        LEVEL_DOWN
    }

    public class GameEvent
    {
        public GameEventType type { get; protected set; }
        public object param { get; protected set; }

        public GameEvent(GameEventType type, object param = null)
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