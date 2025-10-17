using System;
using Korven.GamePlay.Controllers;
using UnityEngine;
using Zenject;

namespace Korven.GamePlay.Installers
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
                throw new NullReferenceException($"<b>[MainInstaller]</b> SceneController is missing!");
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