using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace Core
{
    public class CatToy : MonoBehaviour
    {
        [SerializeField]
        float sqrMaxDistance = 4f;
        [SerializeField]
        Vector3 _centerOffset = new Vector3(0, 2.5f);
        [SerializeField]
        float sqrMouveDetection = 0.8f;
        [SerializeField]
        Rig _rig;
        [SerializeField]
        Transform _target;
        [SerializeField]
        Vector3 _targetOffset = new Vector3(0, 0, -3f);
        [SerializeField]
        float _rigAnimationDuration = 0.5f;

        Camera _currentCamera;

        Vector3 _previousMouseWorldPosition;
        float _rigWeight;

        CatAI _ai;
        CatAI ai
        {
            get
            {
                if (_ai == null)
                {
                    _ai = GetComponent<CatAI>();
                }
                return _ai;
            }
        }

        public void Start()
        {
            _currentCamera = Camera.main;
        }

        public void PointerPosition(InputAction.CallbackContext context)
        {
            if (_currentCamera)
            {
                Vector3 center = transform.position + _centerOffset;
                Vector3 mouseWorldPos = GetMouseWorldPosition(context.ReadValue<Vector2>(), center.z);
                _target.position = mouseWorldPos + _targetOffset;
                if (IsMouseCloseEnough(center, mouseWorldPos))
                {
                    AnimateRigWeight(1);
                    if (IsMouseMovingEnough(mouseWorldPos))
                    {
                        _previousMouseWorldPosition = mouseWorldPos;
                        ai.MouseIsDetected();
                    }
                }
                else
                {
                    AnimateRigWeight(0);
                }
            }
        }

        private void AnimateRigWeight(int newRigWeight)
        {
            if (_rigWeight != newRigWeight)
            {
                _rigWeight = newRigWeight;
                DOTween.To(() => _rig.weight, x => _rig.weight = x, _rigWeight, _rigAnimationDuration);
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

        private Vector3 GetMouseWorldPosition(Vector3 mousePos, float zRef)
        {
            mousePos.z = zRef - _currentCamera.transform.position.z;
            var mouseWorldPos = _currentCamera.ScreenToWorldPoint(mousePos);
            return mouseWorldPos;
        }
    }
}