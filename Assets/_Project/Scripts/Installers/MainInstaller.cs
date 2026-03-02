using System;
using Korven.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Korven.GamePlay.Installers
{
    public class MainInstaller : MonoInstaller
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
            if (sceneService == null)
                throw new NullReferenceException($"<b>[MainInstaller]</b> sceneService is missing!");
        }
        
        /* Именования модулей namespace'сов:
         * Core - логика фреймворка
         * GamePlay - бои, персонажи, враги
         * World - уровни, спавны, окружение
         * UI - интерфейсы меню
         * Audio - звуки
         * Editor - кастомные окна редактора
         * Tools(под модуль) - дополнительные фичи
         */
    }
}