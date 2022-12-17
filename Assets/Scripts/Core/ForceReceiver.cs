using UnityEngine;
using UnityEngine.AI;

namespace Mortem.Core
{
    public class ForceReceiver : MonoBehaviour
    {
        private const float impactDrag = 0.3f;
        private float verticalVelocity;

        private Vector3 impact;
        private Vector3 impactVelocity;
        
        public Vector3 TotalForce => impact + Vector3.up * verticalVelocity;

        private CharacterController controller;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyGravity();
            DampImpact();
        }

        private void ApplyGravity()
        {
            if(verticalVelocity < 0f && controller.isGrounded)
            {
                verticalVelocity = (Physics.gravity.y + 5) * Time.deltaTime;
            }
            else 
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
        }

        private void DampImpact()
        {
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref impactVelocity, impactDrag);

            if(NoImpact())
            {
                ResetImpact();
                ToggleNavMeshAgent(true);
            }
        }

        private bool NoImpact()
        {
            return impact.sqrMagnitude < 0.2f * 0.2f;
        }

        private void ResetImpact()
        {
            impact = Vector3.zero;
        }

        public void AddForce(Vector3 force)
        {
            ToggleNavMeshAgent(false);

            impact += force;
        }

        public void Jump(float jumpForce)
        {
            verticalVelocity += jumpForce;
        }

        public bool IsFalling(float fallingThreshold)
        {
            return controller.velocity.y < fallingThreshold;
        }

        public bool IsGrounded()
        {
            return controller.velocity.y >= -1.5f && controller.isGrounded;   
        }

        public Vector3 GetMomentum()
        {
            return new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        }

        private void ToggleNavMeshAgent(bool state)
        {
            if(!TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)) return;

            agent.enabled = state;
        }
    }
}


