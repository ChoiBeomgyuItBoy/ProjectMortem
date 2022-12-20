using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Mortem.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

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

            string GUID = serializedProperty.stringValue;

            if(IsEmpty(GUID) || !IsUnique(GUID))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[serializedProperty.stringValue] = this;
        }

        private bool InPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path);
        }

        private bool IsEmpty(string GUID)
        {
            return string.IsNullOrEmpty(GUID);
        }

        private bool IsUnique(string GUID)
        {
            if(!globalLookup.ContainsKey(uniqueIdentifier)) return true;

            if(globalLookup[GUID] == this) return true;

            if(globalLookup[GUID] == null)
            {
                globalLookup.Remove(GUID);
                return true;
            }

            if(globalLookup[GUID].GetUniqueIdentifier() != GUID)
            {
                globalLookup.Remove(GUID);
                return true;
            }

            return false;
        }

#endif

    }
}
