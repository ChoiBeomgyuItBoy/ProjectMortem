using System.Collections.Generic;
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

        public object Diciionary { get; private set; }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;

            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                string type = saveable.GetType().ToString();

                if(stateDict.ContainsKey(type))
                {
                    saveable.RestoreState(stateDict[type]);
                }
            }
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
