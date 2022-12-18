using UnityEngine;

namespace Mortem.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;

        private static bool alreadySpawned = false;

        private void Awake()
        {
            if(alreadySpawned) return;

            SpawnPersistentObject();

            alreadySpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);

            DontDestroyOnLoad(persistentObject);
        }
    }
}
