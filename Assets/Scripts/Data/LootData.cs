using System;

namespace Data
{
    [Serializable]
    public class LootData
    {
        public LootPieceDataDictionary LootPiecesOnScene = new LootPieceDataDictionary();

        public int Collected;
        public Action Changed;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
        
        public void Add(int loot)
        {
            Collected += loot;
            Changed?.Invoke();
        }
    }
}