using Mortem.Combat;
using Mortem.StateMachine.AI;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Mortem.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        private CharacterController controller;
        private NavMeshAgent agent;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
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

        private void UpdateCharacterPosition(Vector3 position)
        {
            if(TryGetComponent<AIStateMachine>(out AIStateMachine stateMachine))
            {
                stateMachine.CancelCurrentAction();
            }

            controller.enabled = false;
            if(agent) agent.enabled = false;

            transform.position = position;

            controller.enabled = true;
            if(agent) agent.enabled = true;
        }

#if UNITY_EDITOR

        private void Update()
        {
            if(InPrefabMode()) return;
            
            if(Application.IsPlaying(gameObject)) return;

            UdpateGUID();
        }

        private void UdpateGUID()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("uniqueIdentifier");

            if(GUIDIsEmpty(serializedProperty))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private bool InPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path);
        }

        private bool GUIDIsEmpty(SerializedProperty serializedProperty)
        {
            return string.IsNullOrEmpty(serializedProperty.stringValue);
        }

#endif

    }
}
