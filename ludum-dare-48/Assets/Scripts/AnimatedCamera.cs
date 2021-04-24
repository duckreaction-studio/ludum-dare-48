using Core;
using DG.Tweening;
using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AnimatedCamera : MonoBehaviour
{
    [SerializeField]
    float _margin = 2f;
    [SerializeField]
    float _animationDuration = 0.5f;

    [Inject]
    CatSpawner _spawner;

    Camera _currentCamera;
    public Camera currentCamera
    {
        get
        {
            if (_currentCamera == null)
            {
                _currentCamera = GetComponent<Camera>();
            }
            return _currentCamera;
        }
    }

    Vector3 _startPosition;

    void Start()
    {
        _startPosition = currentCamera.transform.position;
        _spawner.catCountChanged += OnCatCountChanged;
    }

    private void OnCatCountChanged(object sender, int e)
    {
        float maxX = _spawner.activeCatList.Select(cat => cat.transform.position.x).Max();
        maxX += _margin;

        float horizontalAngle = currentCamera.GetHorizontalAngle(true);

        float dist = Mathf.Tan(horizontalAngle * Mathf.Deg2Rad) * maxX;

        Debug.Log("Angle " + horizontalAngle + " dist " + dist);

        if (dist < Mathf.Abs(_startPosition.z))
        {
            AnimateCameraToPosition(_startPosition);
        }
        else
        {
            var pos = transform.position;
            pos.z = _startPosition.z < 0 ? -dist : dist;
            AnimateCameraToPosition(pos);
        }
    }

    public void AnimateCameraToPosition(Vector3 pos)
    {
        transform.DOMove(pos, _animationDuration).SetEase(Ease.OutCubic);
    }
}
