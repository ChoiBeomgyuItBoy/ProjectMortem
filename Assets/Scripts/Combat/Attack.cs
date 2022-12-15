using System;
using UnityEngine;

namespace Mortem.Combat
{
    [Serializable]
    public class Attack
    {
        [field: SerializeField] public AnimationClip AttackAnimation { get; private set; }
        [field: SerializeField] public float TimeBetweenAttacks { get; private set; } = 0.6f;
        [field: SerializeField] public float AttackForce { get; private set; } = 0.3f;
        [field: SerializeField] public int ComboIndex { get; private set; } = -1;
    }
}