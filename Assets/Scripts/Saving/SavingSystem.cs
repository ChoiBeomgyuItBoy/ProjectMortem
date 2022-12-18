using UnityEngine;
using System.IO;

namespace Mortem.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string pathExtension = ".sav";

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print($"Saving to {path}");
            WriteHolaMundoToFile(path);
        }

        private void WriteHolaMundoToFile(string path)
        {
            FileStream stream = File.Open(path, FileMode.Create);
           
            stream.WriteByte(0xc2); stream.WriteByte(0xa1); // ¡

            stream.WriteByte(0x48); // H

            stream.WriteByte(0x6f); // o

            stream.WriteByte(0x6c); // l

            stream.WriteByte(0x61); // a

            stream.WriteByte(0x20); //

            stream.WriteByte(0x4d); // M

            stream.WriteByte(0x75); // u

            stream.WriteByte(0x6e);  // n

            stream.WriteByte(0x64); // d

            stream.WriteByte(0x6f); // o

            stream.WriteByte(0x21); // !

            stream.Close();
        }

        public void Load(string saveFile)
        {
            print($"Loading from {GetPathFromSaveFile(saveFile)}");
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + pathExtension);
        }
    }
}
