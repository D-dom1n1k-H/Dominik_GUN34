using Necro.AutoGen.Controls;
using Zenject;

namespace Necro.GamePlay.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        private Controls _controls;
        public override void InstallBindings()
        {
            ValidateDependencies();
            _controls = new Controls();
            
            Container.Bind<Controls>().FromInstance(_controls);
        }

        private void ValidateDependencies()
        {
            
        }
    }
}