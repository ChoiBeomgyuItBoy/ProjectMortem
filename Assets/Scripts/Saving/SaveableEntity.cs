using UnityEditor;
using UnityEngine;

namespace Mortem.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            print($"Capturing state for {GetUniqueIdentifier()}");
            return null;
        }

        public void RestoreState(object state)
        {
            print($"Restoring state for {GetUniqueIdentifier()}");
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

            if(EmptyProperty(serializedProperty))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private bool InPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path);
        }

        private bool EmptyProperty(SerializedProperty serializedProperty)
        {
            return string.IsNullOrEmpty(serializedProperty.stringValue);
        }
        
#endif

    }
}
