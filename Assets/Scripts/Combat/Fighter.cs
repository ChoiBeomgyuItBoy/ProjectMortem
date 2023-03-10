using System.Collections;
using UnityEngine;

namespace Mortem.Combat
{
    public class Fighter : MonoBehaviour
    {
        [field: SerializeField] public Weapon Weapon { get; private set; }
        [SerializeField] private Transform weaponHandHolder;
        [SerializeField] private Transform weaponBackHolder;

        private GameObject weaponObject;


        private void Start()
        {
            if(Weapon == null) return;

            weaponObject = Instantiate(Weapon.gameObject);

            CheckEnemyWeapon();

            ResetHit();
        }

        private void CheckEnemyWeapon()
        {
            if(Weapon == null) return;
            
            if(GetComponent<CombatTarget>())
            {
                weaponObject.layer = LayerMask.NameToLayer("EnemyWeapon");
            }
        }

        public void UnequipWeapon()
        {
            if(Weapon == null) return;

            UpdateWeaponPosition(weaponBackHolder);
        }

        public void EquipWeapon()
        {
            if(Weapon == null) return;

            UpdateWeaponPosition(weaponHandHolder);
        }

        private void UpdateWeaponPosition(Transform holder)
        {
            weaponObject.transform.parent = holder;
            weaponObject.transform.position = holder.position;
            weaponObject.transform.rotation = holder.rotation;
        }

        // Animation Events
        public void Hit()
        {
            if(Weapon == null) return;

            weaponObject.GetComponent<Collider>().enabled = true;
        }

        public void ResetHit()
        {
            if(Weapon == null) return;
            
            weaponObject.GetComponent<Collider>().enabled = false;
        }
    }
}
