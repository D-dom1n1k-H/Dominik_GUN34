using Zenject;
using UnityEngine;
using Korven.Managers;

namespace Korven.Installers
{
    public class GameInstaller : MonoInstaller
    {
        private Controls _controls;

        [SerializeField]
        private SceneController _sceneController;

        public override void InstallBindings()
        {
            _controls = new Controls();
            _controls.Game.Enable();
            Container.BindInstance(_controls).AsSingle();

            Container.BindInstance(_sceneController).AsSingle();
        }
    }
}