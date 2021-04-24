using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{
    public class CatSpawner : MonoBehaviour
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

        List<Cat> _activeCatList = new List<Cat>();

        public void Start()
        {
            CreateFirstCats();
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
            while (_activeCatList.Count > 0)
            {
                RemoveCat(_activeCatList[0]);
            }
        }

        public void AddCat()
        {
            var cat = _factory.Create(_lastId, GetCatPosition(_lastId));
            _activeCatList.Add(cat);
            ++_lastId;
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
            if (_activeCatList.Contains(cat))
            {
                _activeCatList.Remove(cat);
                cat.Dispose();
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