using UnityEngine;
using System.IO;
using System;

namespace Mortem.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string pathExtension = ".sav";

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
                Transform playerTransform = GetPlayerTransform();

                byte[] data = SerializeVector(playerTransform.position);

                stream.Write(data, 0, data.Length);
            }
        }

        private void ReadDataFromPath(string path)
        {
            using(FileStream stream = File.Open(path, FileMode.Open))
            {
                Transform playerTransform = GetPlayerTransform();

                byte[] dataBuffer = new byte[stream.Length];
                stream.Read(dataBuffer, 0, dataBuffer.Length);

                playerTransform.GetComponent<CharacterController>().enabled = false;
                playerTransform.position = DeserializeVector(dataBuffer);
                playerTransform.GetComponent<CharacterController>().enabled = true;
            }
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * sizeof(float)];

            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);

            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] dataBuffer)
        {
            Vector3 vector = new Vector3();

            vector.x =  BitConverter.ToSingle(dataBuffer, 0);
            vector.y =  BitConverter.ToSingle(dataBuffer, 4);
            vector.z =  BitConverter.ToSingle(dataBuffer, 8);

            return vector;
        }

        private Transform GetPlayerTransform()
        {
            return GameObject.FindWithTag("Player").transform;
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + pathExtension);
        }
    }
}
