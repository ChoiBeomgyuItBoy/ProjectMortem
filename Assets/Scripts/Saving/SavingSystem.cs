using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mortem.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string pathExtension = ".sav";

        private BinaryFormatter formatter;

        private Transform playerTransform;

        private void Start()
        {
            formatter = new BinaryFormatter();

            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print($"Saving to {path}");
            WriteDataToPath(path);
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print($"Loading from {GetPathFromSaveFile(saveFile)}");
            ReadDataFromPath(path);
        }

        private void WriteDataToPath(string path)
        {
            using(FileStream stream = File.Open(path, FileMode.Create))
            {
                formatter.Serialize(stream, new SerializableVector3(playerTransform.position)); 
            }
        }

        private void ReadDataFromPath(string path)
        {
            using(FileStream stream = File.Open(path, FileMode.Open))
            {
                SerializableVector3 position = (SerializableVector3) formatter.Deserialize(stream);

                playerTransform.GetComponent<CharacterController>().enabled = false;
                playerTransform.position = position.ToVector();
                playerTransform.GetComponent<CharacterController>().enabled = true;
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + pathExtension);
        }
    }
}
