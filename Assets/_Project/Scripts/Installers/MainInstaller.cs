using System;
using Zenject;
using Korven.Managers;
using UnityEngine;

namespace Korven.Installers
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneController sceneController;

        public override void InstallBindings()
        {
            ValidateDependencies();

            Container.Bind<SceneController>().FromInstance(sceneController);
        }

        private void ValidateDependencies()
        {
            if (sceneController == null)
                throw new NullReferenceException("[MainInstaller] sceneController not found!]");
        }
    }
}