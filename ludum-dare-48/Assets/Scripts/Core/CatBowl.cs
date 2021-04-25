using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class CatBowl : GameBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [ShowInInspector, ReadOnly]
        Vector3 _basePosition;

        Camera _camera;

        public bool isMoving { get; private set; } = false;

        public void Awake()
        {
            _basePosition = transform.position;
            _camera = Camera.main;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_gameState.isStartedOrRunning())
                isMoving = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isMoving)
            {
                Vector3 pointerWorldPosition = PointerScreenPositionToWorld(eventData);
                Vector3 newPos = pointerWorldPosition;
                newPos.y = _basePosition.y;
                newPos.z = _basePosition.z;
                transform.position = newPos;
            }
        }

        private Vector3 PointerScreenPositionToWorld(PointerEventData eventData)
        {
            Vector3 pointerScreenPosition = eventData.position;
            pointerScreenPosition.z = _basePosition.z - _camera.transform.position.z;
            Vector3 worldPosition = _camera.ScreenToWorldPoint(pointerScreenPosition);
            return worldPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isMoving = false;
        }

        protected override void OnGameReset()
        {
            transform.position = _basePosition;
        }
    }
}