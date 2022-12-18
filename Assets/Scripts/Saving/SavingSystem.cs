using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Mortem.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string pathExtension = ".sav";

        private BinaryFormatter formatter;

        private void Start()
        {
            formatter = new BinaryFormatter();
        }

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print($"Saving to {path}");
            SerializeData(path);
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print($"Loading from {GetPathFromSaveFile(saveFile)}");
            DeserializeData(path);
        }

        private void SerializeData(string path)
        {
            using(FileStream stream = File.Open(path, FileMode.Create))
            {
                formatter.Serialize(stream, CaptureState()); 
            }
        }

        private void DeserializeData(string path)
        {
            using(FileStream stream = File.Open(path, FileMode.Open))
            {
                RestoreState(formatter.Deserialize(stream));
            }
        }

        private object CaptureState()
        {   
            Dictionary<string, object> stateDict = new Dictionary<string, object>();

            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            return stateDict;
        }   

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;

            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.RestoreState(stateDict[saveable.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + pathExtension);
        }
    }
}
