using System;
using UnityEngine;

namespace Mortem.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        public event Action<CombatTarget> OnTargetDestroyed;

        private void OnDestroy()
        {
            OnTargetDestroyed?.Invoke(this);
        }
    }
}
