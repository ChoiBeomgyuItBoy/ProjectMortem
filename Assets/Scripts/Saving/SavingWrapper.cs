using UnityEngine;

namespace Mortem.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";

        private SavingSystem savingSystem;

        private void Start()
        {
            savingSystem = GetComponent<SavingSystem>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                savingSystem.Save(defaultSaveFile);
            }
            else if(Input.GetKeyDown(KeyCode.L))
            {
                savingSystem.Load(defaultSaveFile);
            }
        }
    }
}
