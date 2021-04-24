using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatBowl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [ShowInInspector, ReadOnly]
    Vector3 _basePosition;

    Vector3 _mouseDiffPosition;

    public bool isMoving { get; private set; } = false;

    public void Awake()
    {
        _basePosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isMoving = true;
        _mouseDiffPosition = transform.position - eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseScreenPosition = eventData.position;
        mouseScreenPosition.z = _basePosition.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 newPos = mouseWorldPosition; // + _mouseDiffPosition;
        newPos.y = _basePosition.y;
        newPos.z = _basePosition.z;
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMoving = false;
    }
}
