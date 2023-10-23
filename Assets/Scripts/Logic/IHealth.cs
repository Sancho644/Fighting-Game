using System;
using Infrastructure.Services.Randomizer;

namespace Logic
{
    public interface IHealth
    {
        event Action HealthChanged;
        float Current { get; set; }
        float Max { get; set; }
        void TakeDamage(float damage);
        void Construct(IRandomService randomService);
    }
}