using UnityEngine;
using Mortem.Core;
using UnityEngine.AI;
using Mortem.Saving;
using Mortem.StateMachine.AI;

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

        private void UpdateCharacterPosition(Vector3 position)
        {
            if(TryGetComponent<AIStateMachine>(out AIStateMachine stateMachine))
            {
                stateMachine.CancelCurrentAction();

                if(agent) agent.enabled = false;
                if(controller) controller.enabled = false;
                
                transform.position = position;

                if(agent) agent.enabled = true;
                if(controller) controller.enabled = true;
            }
            else
            {
                if(controller) controller.enabled = false;

                transform.position = position;
                
                if(controller) controller.enabled = true;
            }
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
