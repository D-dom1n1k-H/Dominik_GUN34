using System;
using Necro.AutoGen.Controls;
using Necro.Config.CellPalleteSettings;
using Necro.Extra.GameStatus;
using Necro.GamePlay.Controllers;
using Necro.World.Battlefield;
using UnityEngine;
using Zenject;

namespace Necro.GamePlay.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        private Controls _controls;
        private GameStatus _gameStatus;
        
        [SerializeField]
        private CellPalletSettings cellPalletSettings;
        
        [SerializeField]
        private BattleController battleController;
        
        [SerializeField]
        private Battlefield battlefield;
        
        public override void InstallBindings()
        {
            ValidateDependencies();
            _controls = new Controls();
            _gameStatus = new GameStatus();

            Container.Bind<Controls>().FromInstance(_controls).AsSingle();
            Container.Bind<GameStatus>().FromInstance(_gameStatus).WhenInjectedInto<BattleController>();
            
            Container.Bind<CellPalletSettings>().FromInstance(cellPalletSettings).AsSingle();
            Container.Bind<BattleController>().FromInstance(battleController).AsSingle();
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
        }
    }
}