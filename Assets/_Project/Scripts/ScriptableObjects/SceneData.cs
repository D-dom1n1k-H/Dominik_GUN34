using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Korven.Data
{
    [CreateAssetMenu(fileName = "NewSceneData", menuName = "Data/SceneData")]
    public class SceneData : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField]
        private SceneAsset sceneAsset;
#endif

        [SerializeField, HideInInspector]
        private string sceneName;

        public string SceneName => sceneName;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sceneAsset != null)
            {
                sceneName = sceneAsset.name;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}