using UnityEngine;
using UnityEngine.SceneManagement;

namespace Korven.Managers
{
    public class SceneController : MonoBehaviour
    {
        public void OpenMainScene()
        {
            SceneManager.LoadScene(0);
            Debug.Log($"[SceneController] {SceneManager.GetActiveScene().name} was successfully loaded!]");
        }

        public void OpenGameScene()
        {
            SceneManager.LoadScene(1);
            Debug.Log($"[SceneController] {SceneManager.GetActiveScene().name} was successfully loaded!]");
        }

        public void OpenGameSceneAddictive()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            Debug.Log($"[SceneController] {SceneManager.GetActiveScene().name} was successfully loaded!]");
        }
    }
}