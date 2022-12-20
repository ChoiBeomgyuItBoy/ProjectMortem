using UnityEngine;
using Mortem.Core;
using UnityEngine.AI;
using Mortem.Saving;

namespace Mortem.Movement
{
    public class Mover : MonoBehaviour, ISaveable
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

        public void NavMeshAgentMove(Vector3 motion)
        {
            if(!agent.isOnNavMesh) return;

            agent.velocity = controller.velocity;

            Move(motion);
        }

        public void Move(Vector3 motion)
        {
            if(!controller) return;

            controller.Move((motion + forceReceiver.TotalForce) * Time.deltaTime);
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

        private void UpdateCharacterPosition(Vector3 position)
        {
            if(agent) agent.enabled = false;
            GetComponent<CharacterController>().enabled = false;

            transform.position = position;

            GetComponent<CharacterController>().enabled = true;
            if(agent) agent.enabled = true;
        }

        private bool IsPlayer()
        {
            return gameObject.tag == "Player";
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;

            UpdateCharacterPosition(position.ToVector());
        }
    }
}
