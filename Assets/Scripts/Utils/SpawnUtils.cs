using UnityEngine;

namespace Utils
{
    public class SpawnUtils
    {
        private const string Container = "###SPAWNER###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = Container)
        {
            GameObject container = GameObject.Find(containerName);
            if (container == null)
                container = new GameObject(containerName);

            return Object.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }
    }
}