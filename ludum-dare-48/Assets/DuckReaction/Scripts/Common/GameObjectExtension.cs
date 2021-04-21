using UnityEngine;
using UnityEngine.UI;

namespace DuckReaction.Common
{
    public static class GameObjectExtension
    {
        public static RectTransform GetRectTransform(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.transform.GetRectTransform();
        }

        public static RectTransform GetRectTransform(this Transform transform)
        {
            return (RectTransform)transform;
        }

        public static RectTransform GetRectTransform(this GameObject gameObject)
        {
            return gameObject.transform.GetRectTransform();
        }
    }
}