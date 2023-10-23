using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public class FxChecker : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public Action OnFinish;

        private void OnEnable()
        {
            StartCoroutine(CheckIfAlive());
        }

        private IEnumerator CheckIfAlive()
        {
            while (_particleSystem != null)
            {
                yield return new WaitForSeconds(0.5f);

                if (!_particleSystem.IsAlive(true))
                {
                    OnFinish?.Invoke();

                    break;
                }
            }
        }
    }
}