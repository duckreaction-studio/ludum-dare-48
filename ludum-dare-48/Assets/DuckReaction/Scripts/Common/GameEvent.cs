using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckReaction.Common
{
    public enum GameEventType
    {
        GamePreStart,
        GameStart,
        GameOver,
        GamePause,
        GameReset,
        GameResume,
        GameStateChanged,
        LevelUp,
        LevelDown,
        Other
    }

    public class GameEvent
    {
        protected int _type;
        public string typeAsString { get; protected set; }
        public GameEventType type
        {
            get
            {
                return (GameEventType)_type;
            }
        }
        public object param { get; protected set; }

        public GameEvent(Enum type, object param = null)
        {
            typeAsString = type.ToString("g");
            _type = (int)((object)type);
            this.param = param;
        }

        public bool Is(Enum type)
        {
            return _type == (int)((object)type);
        }

        public T GetType<T>() where T : Enum
        {
            return (T)(object)_type;
        }

        public T GetParam<T>()
        {
            return (T)param;
        }
    }
}