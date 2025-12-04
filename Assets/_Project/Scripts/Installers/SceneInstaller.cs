using System;
using Necro.AutoGen.Controls;
using Necro.Config.CellPalleteSettings;
using Necro.Config.DefaultSettings;
using Necro.Extra.Enums.CurrentTrain;
using Necro.Extra.Enums.UnitrType;
using Necro.Extra.GameStatus;
using Necro.GamePlay.Controllers;
using Necro.GamePlay.Controllers.PlayerController;
using Necro.GamePlay.Units;
using Necro.Interfaces.ConfirmMovementTextController;
using Necro.Interfaces.WorldSpaceCanvasController;
using Necro.World.Battlefield;
using UnityEngine;
using Zenject;

namespace Necro.GamePlay.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        private Controls _controls;
        private GameStatus _gameStatus;
        private UnitType _unitType;
        private CurrentTrain _currentTrain;
        
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private CellPalletSettings cellPalletSettings;

        [SerializeField] private SceneController sceneController;
        [SerializeField] private BattleController battleController;
        [SerializeField] private PlayerController playerController;

        [SerializeField] private ConfirmMovementTextController confirmMovementTextController;
        [SerializeField] private WorldSpaceCanvasController worldSpaceCanvasController;

        [SerializeField] private Battlefield battlefield;

        public override void InstallBindings()
        {
            ValidateDependencies();
            
            _controls = new Controls();
            _gameStatus = new GameStatus();
            _unitType = new UnitType();
            _currentTrain = new CurrentTrain();

            // core bindings
            Container.Bind<Controls>().FromInstance(_controls).AsSingle();
            Container.Bind<CurrentTrain>().FromInstance(_currentTrain).WhenInjectedInto<PlayerController>();
            Container.Bind<GameStatus>().FromInstance(_gameStatus).WhenInjectedInto<BattleController>();
            Container.Bind<UnitType>().FromInstance(_unitType).WhenInjectedInto<Unit>();

            // configuration/settings
            Container.Bind<GameSettings>().FromInstance(gameSettings).AsSingle();
            Container.Bind<CellPalletSettings>().FromInstance(cellPalletSettings).AsSingle();

            // scene objects / controllers
            Container.Bind<BattleController>().FromInstance(battleController).AsSingle();
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();
            Container.Bind<Battlefield>().FromInstance(battlefield).AsSingle();
            Container.Bind<SceneController>().FromInstance(sceneController).AsSingle();

            // UI / interfaces
            Container.Bind<ConfirmMovementTextController>().FromInstance(confirmMovementTextController).AsSingle();
            Container.Bind<WorldSpaceCanvasController>().FromInstance(worldSpaceCanvasController).AsSingle();
        }

        private void ValidateDependencies()
        {
            if (cellPalletSettings == null)
                throw new ArgumentException("[SceneInstaller] 'cellPalletSettings' was not assigned in the Inspector.", nameof(cellPalletSettings));

            if (gameSettings == null)
                throw new ArgumentException("[SceneInstaller] 'gameSettings' was not assigned in the Inspector.", nameof(gameSettings));

            if (sceneController == null)
                throw new ArgumentException("[SceneInstaller] 'sceneController' was not assigned in the Inspector.", nameof(sceneController));

            if (battleController == null)
                throw new ArgumentException("[SceneInstaller] 'battleController' was not assigned in the Inspector.", nameof(battleController));

            if (playerController == null)
                throw new ArgumentException("[SceneInstaller] 'playerController' was not assigned in the Inspector.", nameof(playerController));

            if (battlefield == null)
                throw new ArgumentException("[SceneInstaller] 'battlefield' was not assigned in the Inspector.", nameof(battlefield));

            if (confirmMovementTextController == null)
                throw new ArgumentException("[SceneInstaller] 'confirmMovementTextController' was not assigned in the Inspector.", nameof(confirmMovementTextController));

            if (worldSpaceCanvasController == null)
                throw new ArgumentException("[SceneInstaller] 'worldSpaceCanvasController' was not assigned in the Inspector.", nameof(worldSpaceCanvasController));
        }
    }
}
