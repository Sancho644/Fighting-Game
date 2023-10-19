using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public State HeroState;
        public Stats HeroStats;
        public KillData KillData;
        public PurchaseData PurchaseData;
        public AudioData AudioData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
            PurchaseData = new PurchaseData();
            AudioData = new AudioData();
        }
    }
}