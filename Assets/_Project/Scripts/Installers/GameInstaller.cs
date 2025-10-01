using Korven.Controllers;
using Korven.Data.Settings;
using Korven.Managers;
using UnityEngine;
using Zenject;

namespace Korven.Installers
{
    public class GameInstaller : MonoInstaller
    {
        private Controls _controls;
        [SerializeField]
        private SceneController sceneController;
        [SerializeField]
        private CellPaletteSettings paletteSettings;

        public override void InstallBindings()
        {
            _controls = new Controls();
            ValidateDependencies();

            Container.BindInstance(_controls).AsSingle();

            Container.BindInstance(sceneController).AsSingle();
            Container.BindInstance(paletteSettings).AsSingle();
        }

        private void ValidateDependencies()
        {
            if (_controls == null)
                throw new MissingComponentException("[GameInstaller] _controls not found!");

            if (sceneController == null)
                throw new MissingComponentException("[GameInstaller] sceneController not found!");

            if (paletteSettings == null)
                throw new MissingComponentException("[GameInstaller] paletteSettings not found!");
        }
    }
}