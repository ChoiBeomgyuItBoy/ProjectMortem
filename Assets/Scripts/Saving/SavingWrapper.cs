using System.Collections;
using Mortem.Saving;
using UnityEngine;

namespace Mortem.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
         private SavingSystem savingSystem;
        private Fader fader;

        private const string defaultSaveFile = "save";
        private const float fadeInTime = 1f;

        private IEnumerator Start()
        {
            savingSystem = GetComponent<SavingSystem>();
            fader = FindObjectOfType<Fader>();

            fader.FadeOutInmediate();
            
            yield return(savingSystem.LoadLastScene(defaultSaveFile)); 
            yield return(fader.FadeIn(fadeInTime));
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                Save();
            }
            else if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }
}
