using Korven.Core.Services;
using UnityEngine;
using Zenject;

namespace Korven.Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneService sceneService;

        public override void InstallBindings()
        {
            Container.Bind<SceneService>().FromInstance(sceneService).AsSingle();
        }
    }
}