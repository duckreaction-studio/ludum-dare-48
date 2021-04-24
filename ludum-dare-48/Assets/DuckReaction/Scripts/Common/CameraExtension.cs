using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckReaction.Common
{
    public static class CameraExtension
    {
        public static float GetHorizontalAngle(this Camera camera, bool half = false)
        {
            float vFov = camera.fieldOfView;
            if (half)
                vFov *= 0.5f;
            float vFOVrad = vFov * Mathf.Deg2Rad;
            float cameraHeightAt1 = Mathf.Tan(vFOVrad * 0.5f);
            return Mathf.Atan(cameraHeightAt1 * camera.aspect) * 2f * Mathf.Rad2Deg;
        }

        public static float GetOrthographicSizeFromWidth(this Camera camera, float width)
        {
            return (width * 0.5f) / camera.aspect;
        }
    }
}