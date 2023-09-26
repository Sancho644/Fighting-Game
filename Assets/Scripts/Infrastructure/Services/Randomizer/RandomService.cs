using UnityEngine;

namespace Infrastructure.Services.Randomizer
{
    public class RandomService : IRandomService
    {
        public int Next(int minValue, int maxValue) => 
            Random.Range(minValue, maxValue);
    }
}