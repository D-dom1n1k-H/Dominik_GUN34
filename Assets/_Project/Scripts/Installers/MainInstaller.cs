using Zenject;
using Korven.Managers;
using UnityEngine;

namespace Korven.Installers
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private SceneController _sceneController;

        public override void InstallBindings()
        {
            Container.Bind<SceneController>().FromInstance(_sceneController);
        }
    }
}