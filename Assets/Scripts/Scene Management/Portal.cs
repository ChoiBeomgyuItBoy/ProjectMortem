using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mortem.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = 0;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
