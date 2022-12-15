using System;
using UnityEngine;

namespace Mortem.Combat
{
    public class Health : MonoBehaviour
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
            print(transform.name + ": " + health);

            if(IsDead) Die();
        }

        private void Die()
        {
            DeadEvent?.Invoke();
        }
    }
}