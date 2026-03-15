using Korven.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Korven.Core.Services
{
    public class SceneService : MonoBehaviour
    {
        public void Load(SceneData sceneData,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (sceneData == null)
            {
                Debug.LogError("SceneData is null!");
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(sceneData.SceneName))
            {
                Debug.LogError($"Scene '{sceneData.SceneName}' is not in Build Settings!");
                return;
            }

            SceneManager.LoadScene(sceneData.SceneName, mode);

            Debug.Log($"[SceneService] Scene '{sceneData.SceneName}' loaded ({mode})");
        }
    }
}