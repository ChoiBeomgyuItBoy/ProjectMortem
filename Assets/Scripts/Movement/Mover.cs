using UnityEngine;
using Mortem.Core;
using UnityEngine.AI;

namespace Mortem.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float rotationDamping = 10f;

        private CharacterController controller;
        private ForceReceiver forceReceiver;

        private NavMeshAgent agent;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            forceReceiver = GetComponent<ForceReceiver>();
            agent = GetComponent<NavMeshAgent>();
        }

        public void Move(Vector3 motion)
        {
            controller.Move((motion + forceReceiver.TotalForce) * Time.deltaTime);

            if(!agent) return;

            agent.velocity = controller.velocity;
        }

        public void LookAt(Vector3 direction)
        {
            if(direction == Vector3.zero) return;

            transform.rotation = Quaternion.Lerp
            (
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationDamping * Time.deltaTime
            );
        }
    }
}
