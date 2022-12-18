using UnityEditor;
using UnityEngine;

namespace Mortem.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

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

            if(serializedProperty.stringValue == "")
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private bool InPrefabMode()
        {
            return string.IsNullOrEmpty(gameObject.scene.path);
        }
#endif
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
    }
}
