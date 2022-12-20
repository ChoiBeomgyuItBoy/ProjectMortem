using System;
using Mortem.Saving;
using UnityEngine;

namespace Mortem.Combat
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float health = 100f;

        public bool IsDead => health <= 0;

        public event Action DamageEvent;
        public event Action DeadEvent;

        public void TakeDamage(float damage)
        {
            if(IsDead) return;

            DamageEvent?.Invoke();

            health = Mathf.Max(health - damage, 0f);

            if(IsDead) DeadEvent?.Invoke();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float) state;

            if(IsDead) DeadEvent?.Invoke();
        }
    }
}