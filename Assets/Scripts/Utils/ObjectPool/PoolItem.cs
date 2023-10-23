using System;
using UnityEngine;

namespace Utils.ObjectPool
{
    public class PoolItem : MonoBehaviour
    {
        [SerializeField] private FxChecker _checker;

        public event Action OnRestart;

        private int _id;
        private Pool _pool;

        private void Start()
        {
            _checker.OnFinish += Release;
        }

        public void Restart()
        {
            OnRestart?.Invoke();
        }

        private void Release()
        {
            _pool.Release(_id, this);
        }

        public void Retain(int id, Pool pool)
        {
            _id = id;
            _pool = pool;
        }
    }
}