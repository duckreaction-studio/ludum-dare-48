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

    float _startSize;
    float _previousAspect;

    void Start()
    {
        if (!currentCamera.orthographic)
            throw new Exception("Camera should be orthographic");
        _startSize = currentCamera.orthographicSize;
        _previousAspect = currentCamera.aspect;
        _spawner.catCountChanged += OnCatCountChanged;
    }

    public void Update()
    {
        if (currentCamera.aspect != _previousAspect)
        {
            _previousAspect = currentCamera.aspect;
            UpdateCameraSize();
        }
    }

    private void OnCatCountChanged(object sender, int e)
    {
        UpdateCameraSize();
    }

    private void UpdateCameraSize()
    {
        if (_spawner.activeCatList != null && _spawner.activeCatList.Count > 0)
        {
            float maxX = _spawner.activeCatList.Select(cat => cat.transform.position.x).Max();
            maxX += _margin;

            float size = currentCamera.GetOrthographicSizeFromWidth(maxX * 2);

            if (size < _startSize)
            {
                AnimateCameraOrthographicSize(_startSize);
            }
            else
            {
                AnimateCameraOrthographicSize(size);
            }
        }
    }

    public void AnimateCameraOrthographicSize(float size)
    {
        currentCamera.DOOrthoSize(size, _animationDuration);
    }
}
