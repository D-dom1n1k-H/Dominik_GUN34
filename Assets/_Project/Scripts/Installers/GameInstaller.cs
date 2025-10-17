using Zenject;

namespace Korven.GamePlay.Installers
{
    public class GameInstaller : MonoInstaller
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