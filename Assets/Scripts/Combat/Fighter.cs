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

            ResetHit();

            CheckEnemyWeapon();
        }

        private void CheckEnemyWeapon()
        {
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
            weaponObject.transform.position = holder.position;
            weaponObject.transform.rotation = holder.rotation;
            weaponObject.transform.parent = holder;
        }

        // Animation Events
        public void Hit()
        {
            weaponObject.GetComponent<Collider>().enabled = true;
        }

        public void ResetHit()
        {
            weaponObject.GetComponent<Collider>().enabled = false;
        }
    }
}
