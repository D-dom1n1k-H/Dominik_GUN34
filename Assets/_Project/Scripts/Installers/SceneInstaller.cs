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

        [SerializeField]
        private DefaultSettings defaultSettings;

        [SerializeField]
        private CellPalletSettings cellPalletSettings;

        [SerializeField]
        private BattleController battleController;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private Battlefield battlefield;

        public override void InstallBindings()
        {
            ValidateDependencies();
            _controls = new Controls();
            _gameStatus = new GameStatus();
            _unitType = new UnitType();
            _currentTrain = new CurrentTrain();

            Container.Bind<Controls>().FromInstance(_controls).AsSingle();
            Container.Bind<CurrentTrain>().FromInstance(_currentTrain).WhenInjectedInto<PlayerController>();
            Container.Bind<GameStatus>().FromInstance(_gameStatus).WhenInjectedInto<BattleController>();
            Container.Bind<UnitType>().FromInstance(_unitType).WhenInjectedInto<Unit>();

            Container.Bind<DefaultSettings>().FromInstance(defaultSettings).AsSingle();
            Container.Bind<CellPalletSettings>().FromInstance(cellPalletSettings).AsSingle();
            Container.Bind<BattleController>().FromInstance(battleController).AsSingle();
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();
            Container.Bind<Battlefield>().FromInstance(battlefield).AsSingle();
        }

        private void ValidateDependencies()
        {
            if (cellPalletSettings == null)
                throw new NullReferenceException("<b>[SceneInstaller]</b> cellPalletSettings is null!");

            if (battleController == null)
                throw new NullReferenceException("<b>[SceneInstaller]</b> battleController is null!");

            if (battlefield == null)
                throw new NullReferenceException("<b>[SceneInstaller]</b> battlefield is null!");

            if (playerController == null)
                throw new NullReferenceException("<b>[SceneInstaller]</b> playerController is null!");
            if (defaultSettings == null)
                throw new NullReferenceException("<b>[SceneInstaller]</b> defaultSettings is null!");
        }
    }
}