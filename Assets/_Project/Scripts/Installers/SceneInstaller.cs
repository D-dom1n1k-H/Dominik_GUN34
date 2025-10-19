using Zenject;

namespace Korven.GamePlay.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ValidateDependencies();
        }

        private void ValidateDependencies()
        {
            
        }
    }
}