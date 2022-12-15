using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Mortem.Combat
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup targetGroup;

        private List<CombatTarget> targets = new List<CombatTarget>();

        public CombatTarget CurrentTarget { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent<CombatTarget>(out CombatTarget target)) return;
              
            AddTarget(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent<CombatTarget>(out CombatTarget target)) return;

            RemoveTarget(target);
        }

        private void AddTarget(CombatTarget target)
        {
            targets.Add(target);
            target.OnTargetDestroyed += RemoveTarget;
        }

        private void RemoveTarget(CombatTarget target)
        {
            if(CurrentTarget == target)
            {
                targetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }

            target.OnTargetDestroyed += RemoveTarget;
            targets.Remove(target);
        }
        
        private Vector2 GetTargetScreenPosition(CombatTarget target)
        {
            return Camera.main.WorldToViewportPoint(target.transform.position);
        }

        private bool IsInScreen(CombatTarget target)
        {
            Vector2 targetPosition = GetTargetScreenPosition(target);

            if(targetPosition.x < 0 || targetPosition.x > 1 || targetPosition.y < 0 ||targetPosition.y > 1)
            {
                return false;
            }
            
            return true;
        }

        private CombatTarget GetClosestTargetToScreen()
        {
            CombatTarget closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach(CombatTarget target in targets)
            {
                if(!IsInScreen(target)) continue;

                Vector2 screenCenterDifference = GetTargetScreenPosition(target) - new Vector2(0.5f, 0.5f);

                if(screenCenterDifference.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = screenCenterDifference.sqrMagnitude;
                }
            }

            return closestTarget;
        }

        public bool SelectCurrentTarget()
        {
            if(targets.Count == 0) return false;

            CombatTarget closestTarget = GetClosestTargetToScreen();

            if(closestTarget == null) return false;

            CurrentTarget = closestTarget;
            targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

            return true;
        }

        public void FaceCurrentTarget(Transform character)
        {
            if(CurrentTarget == null) return;

            Vector3 lookPosition = (CurrentTarget.transform.position - character.position);
            lookPosition.y = 0f;

            character.rotation = Quaternion.LookRotation(lookPosition);
        }

        public void Cancel()
        {
            if(CurrentTarget == null) return;

            targetGroup.RemoveMember(CurrentTarget.transform);
            
            CurrentTarget = null;
        }
    }
}
