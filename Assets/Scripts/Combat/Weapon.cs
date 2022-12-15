using System.Collections.Generic;
using UnityEngine;
using Mortem.Core;

namespace Mortem.Combat
{
    public class Weapon : MonoBehaviour
    {
        [field: SerializeField] public Attack[] Combo { get; private set; }

        [SerializeField] private float damage = 1;
        [SerializeField] private float knockBack = 5;

        private CharacterController myController;

        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void Start()
        {
            myController = GetComponentInParent<CharacterController>();
        } 

        private void OnTriggerEnter(Collider other)
        {
            if(other == myController) return;
            if(alreadyCollidedWith.Contains(other)) return;

            alreadyCollidedWith.Add(other);

            ApplyDamage(other);
            ApplyKnockBack(other);
        }

        private void OnTriggerExit(Collider other)
        {
            alreadyCollidedWith.Clear();
        }

        private void ApplyDamage(Collider target)
        {
            if(!target.TryGetComponent<Health>(out Health health)) return;
    
            health.TakeDamage(damage);
        }

        private void ApplyKnockBack(Collider target)
        {
            if(!target.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver)) return;

            Vector3 knockBackDirection = GetKnockBackDirection(target);
    
            forceReceiver.AddForce(knockBackDirection * knockBack);
        }

        private Vector3 GetKnockBackDirection(Collider target)
        {
            return (target.transform.position - myController.transform.position).normalized;
        }
    }
}
