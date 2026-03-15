using Korven.Core.ScriptableObjects;
using Korven.Core.Services;
using UnityEngine;
using Zenject;

namespace Korven.Systems.Managers
{
    public sealed class GameSceneRunner : MonoBehaviour
    {
        [SerializeField]
        private SceneData gameSceneData;

        private SceneService _sceneService;

        [Inject]
        private void Construct(SceneService sceneService)
        {
            _sceneService = sceneService;
        }

        private void Start()
        {
            _sceneService.Load(gameSceneData);
        }
    }
}