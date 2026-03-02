using Korven.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Korven.GamePlay.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneService sceneService;
        
        public override void InstallBindings()
        {
            ValidateDependencies();
            
            Container.Bind<SceneService>().FromInstance(sceneService);
        }

        private void ValidateDependencies()
        {
            
        }
    }
}