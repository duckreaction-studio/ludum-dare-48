using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{
    public class Cat : MonoBehaviour, IPoolable<int, Vector3, IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool;

        [Inject]
        ProjectSettings _projectSettings;

        [SerializeField]
        SkinnedMeshRenderer _skinnedMesh;

        public CatCategorySettings category { get; private set; }

        public void OnSpawned(int id, Vector3 position, IMemoryPool pool)
        {
            _pool = pool;
            gameObject.name = "Cat_" + id;
            transform.position = position;
            category = _projectSettings.GetRandomCategory();
            UpdateColors();
            BroadcastMessage("OnAfterSpawn", SendMessageOptions.DontRequireReceiver);
        }

        private void UpdateColors()
        {
            _skinnedMesh.materials[0].color = category.color1;
            _skinnedMesh.materials[1].color = category.color2;
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<int, Vector3, Cat>
        {
            [Inject]
            public Factory() : base()
            {

            }
        }

        public class Pool : MonoPoolableMemoryPool<int, Vector3, IMemoryPool, Cat>
        {

        }
    }
}