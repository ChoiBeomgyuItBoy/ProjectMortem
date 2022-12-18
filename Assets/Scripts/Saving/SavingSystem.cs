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
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);

            print($"Saving to {GetPathFromSaveFile(saveFile)}");

            using(FileStream stream = File.Open(path, FileMode.Create))
            {
                formatter.Serialize(stream, state); 
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);

            print($"Loading from {GetPathFromSaveFile(saveFile)}");

            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using(FileStream stream = File.Open(path, FileMode.Open))
            {
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {   
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
        }   

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string uniqueIdentifier = saveable.GetUniqueIdentifier();

                if(state.ContainsKey(uniqueIdentifier))
                {
                    saveable.RestoreState(state[uniqueIdentifier]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + pathExtension);
        }
    }
}
