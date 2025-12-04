using System;
using Necro.GamePlay.Controllers;
using UnityEngine;
using Zenject;

namespace Necro.MainScene.MainSceneController
{
    public class MainSceneController : MonoBehaviour
    {
        [Inject]
        private SceneController _sceneController;

        private void Awake()
        {
            ValidateDependencies();

            _sceneController.OpenGameScene();
        }


        private void ValidateDependencies()
        {
            if (_sceneController == null)
                throw new ArgumentNullException(nameof(_sceneController),
                    "<b>[MainSceneController]</b> _sceneController could not bie injected!");
        }
    }
}