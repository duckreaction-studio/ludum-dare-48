using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class CatToy : MonoBehaviour
    {
        [SerializeField]
        private float sqrMaxDistance = 4f;
        [SerializeField]
        private Vector3 _centerOffset = new Vector3(0, 2.5f);
        [SerializeField]
        private float sqrMouveDetection = 0.8f;

        Camera _currentCamera;

        public void Start()
        {
            _currentCamera = Camera.main;
        }

        Vector3 _previousMouseWorldPosition;

        void Update()
        {
            Vector3 center = transform.position + _centerOffset;
            Vector3 mouseWorldPos = GetMouseWorldPosition(center.z);

            if (IsMouseCloseEnough(center, mouseWorldPos))
            {
                if (IsMouseMovingEnough(mouseWorldPos))
                {
                    _previousMouseWorldPosition = mouseWorldPos;
                }
            }
        }

        private bool IsMouseMovingEnough(Vector3 mouseWorldPos)
        {
            return (_previousMouseWorldPosition - mouseWorldPos).sqrMagnitude > sqrMouveDetection;
        }

        private bool IsMouseCloseEnough(Vector3 center, Vector3 mouseWorldPos)
        {
            return (center - mouseWorldPos).sqrMagnitude < sqrMaxDistance;
        }

        private Vector3 GetMouseWorldPosition(float zRef)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = zRef - _currentCamera.transform.position.z;
            var mouseWorldPos = _currentCamera.ScreenToWorldPoint(mousePos);
            return mouseWorldPos;
        }
    }
}