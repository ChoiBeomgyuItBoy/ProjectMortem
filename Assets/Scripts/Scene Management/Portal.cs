using System.Collections;
using Mortem.Control;
using Mortem.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mortem.SceneManagement
{
    public enum DestinationIdentifier
    {
        A, B, C, D, E
    }

    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        private const float fadeOutTime = 0.5f;
        private const float fadeInTime = 1f;
        private const float fadeWaitTime = 0.5f;

        private SavingWrapper savingWrapper;
        private Fader fader;

        private void Start()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
            fader = FindObjectOfType<Fader>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            if(sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            savingWrapper.Load();
        
            Portal otherPortal = GetOtherPortal();

            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != destination) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<CharacterController>().enabled = false;

            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;

            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
