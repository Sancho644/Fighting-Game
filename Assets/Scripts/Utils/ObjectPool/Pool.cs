using System.Collections.Generic;
using UnityEngine;

namespace Utils.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<PoolItem>> _items = new Dictionary<int, Queue<PoolItem>>();

        private static Pool _instance;

        public static Pool Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("###MAIN_POOL###");
                    _instance = go.AddComponent<Pool>();
                }

                return _instance;
            }
        }

        public GameObject Get(GameObject go, Vector3 position)
        {
            int id = go.GetInstanceID();
            Queue<PoolItem> queue = RequireQueue(id);

            if (queue.Count > 0)
            {
                PoolItem pooledItem = queue.Dequeue();
                pooledItem.transform.position = position;
                pooledItem.gameObject.SetActive(true);
                pooledItem.Restart();

                return pooledItem.gameObject;
            }

            GameObject instance = SpawnUtils.Spawn(go, position, gameObject.name);
            PoolItem poolItem = instance.GetComponent<PoolItem>();
            
            poolItem.Retain(id, this);

            return instance;
        }

        public void Release(int id, PoolItem poolItem)
        {
            Queue<PoolItem> queue = RequireQueue(id);
            queue.Enqueue(poolItem);
            
            poolItem.gameObject.SetActive(false);
        }

        private Queue<PoolItem> RequireQueue(int id)
        {
            if (!_items.TryGetValue(id, out var queue))
            {
                queue = new Queue<PoolItem>();
                _items.Add(id, queue);
            }

            return queue;
        }
    }
}