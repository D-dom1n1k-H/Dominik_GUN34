using Zenject;
using Managers;
using UnityEngine;

namespace Installers
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