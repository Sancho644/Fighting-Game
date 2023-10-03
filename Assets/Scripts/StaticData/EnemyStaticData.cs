using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;
        
        [Range(1, 100)]
        public int Hp;
        
        [Range(1f, 30)]
        public float Damage;

        public int MaxLoot;
        
        public int MinLoot;
        
        [Range(0.5f, 1)]
        public float EffectiveDistance;
        
        [Range(0.5f, 1)]
        public float Cleavage;
        
        [Range(0.5f, 10)]
        public float MoveSpeed;

        public AssetReferenceGameObject PrefabReference;
    }
}