using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{
    public class CatSpawner : GameBehaviour
    {
        [SerializeField]
        int _startCount = 1;
        [SerializeField]
        int _stepIncrease = 2;
        [SerializeField]
        float _distanceBetweenCats = 0.5f;

        int _lastId = 0;

        [Inject]
        Cat.Factory _factory;

        public List<Cat> activeCatList { get; private set; } = new List<Cat>();

        public event EventHandler<int> catCountChanged;

        protected override void Start()
        {
            CreateFirstCats();
            base.Start();
        }

        protected override void OnGameEventReceived(GameEvent gameEvent)
        {
            if (gameEvent.type == GameEventType.LevelUp || gameEvent.type == GameEventType.GameStart)
                AddCats();
        }

        private void CreateFirstCats()
        {
            for (int i = 0; i < _startCount; i++)
            {
                AddCat();
            }
        }


        [ContextMenu("Add cats")]
        public void AddCats()
        {
            for (int i = 0; i < _stepIncrease; i++)
            {
                AddCat();
            }
        }

        [ContextMenu("Reset")]
        public void Reset()
        {
            DestroyAllCats();
            _lastId = 0;
            CreateFirstCats();
        }

        public void DestroyAllCats()
        {
            while (activeCatList.Count > 0)
            {
                RemoveCat(activeCatList[0]);
            }
        }

        public void AddCat()
        {
            var cat = _factory.Create(_lastId, GetCatPosition(_lastId));
            activeCatList.Add(cat);
            ++_lastId;
            InvokeCatCountChanged();
        }

        private void InvokeCatCountChanged()
        {
            catCountChanged?.Invoke(this, activeCatList.Count);
        }

        private Vector3 GetCatPosition(int id)
        {
            if (id == 0)
                return transform.position;
            bool isOdd = id % 2 != 0;
            if (isOdd)
            {
                id++;
            }
            id = id / 2;
            if (!isOdd)
            {
                id = -id;
            }
            Vector3 pos = transform.position;
            pos.x += id * _distanceBetweenCats;
            return pos;
        }

        public void RemoveCat(Cat cat)
        {
            if (activeCatList.Contains(cat))
            {
                activeCatList.Remove(cat);
                cat.Dispose();
                InvokeCatCountChanged();
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (Transform child in transform)
            {
                Gizmos.DrawLine(child.position, child.position + Vector3.up * 3);
            }
        }
    }
}