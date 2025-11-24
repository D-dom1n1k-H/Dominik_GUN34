using System;
using Necro.Extra.Enums.CurrentTrain;
using Necro.GamePlay.Units;
using Necro.World.Battlefield;
using Necro.World.Board.Cell;
using UnityEngine;
using Zenject;
using Random = Unity.Mathematics.Random;

namespace Necro.GamePlay.Controllers.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        private Battlefield _battlefield; //injected
        private BattleController _battleController; //injected
        private CurrentTrain _currentTrain; //injected

        // for movement
        private Cell _currentCell;
        private Cell _targetCell;

        private Unit[] _units;
        
        private Random _random = new Random();

        public bool UnitIsMoving { get; private set; }

        private void Awake()
        {
            _units = FindObjectsOfType<Unit>();

            ValidateDependencies();
        }

        private void Start()
        {
            var randomValue = (byte)UnityEngine.Random.Range(0, 2);

            if (randomValue == 0) 
                _currentTrain = CurrentTrain.WhiteTeam;
            else if (randomValue == 1)
                _currentTrain = CurrentTrain.BlackTeam;

            Debug.Log("<b>[PlayerController]</b> current train: " + _currentTrain);
        }

        private void OnEnable()
        {
            _battlefield.OnMoveRequestEvent += OnUnitMovementStarted;

            for (int i = 0; i < _units.Length; i++)
            {
                _units[i].OnMoveCompleted += OnUnitMovementEnded;
            }
        }

        private void OnDisable()
        {
            _battlefield.OnMoveRequestEvent -= OnUnitMovementStarted;
        }

        #region Public API

        public CurrentTrain GetCurrentTrain()
        {
            return _currentTrain;
        }

        public void ChangeCurrentTrainToOpositeOne()
        {
            if (_currentTrain == CurrentTrain.WhiteTeam)
            {
                _currentTrain = CurrentTrain.BlackTeam;
            }
            else if (_currentTrain == CurrentTrain.BlackTeam)
            {
                _currentTrain = CurrentTrain.WhiteTeam;
            }
            
            Debug.Log($"<b>[PlayerController]</b> CurrentTrain was changed to: {_currentTrain}");
        }
        
        #endregion

        #region Private Logic

        private void OnUnitMovementStarted(Cell currentCell, Cell targetCell)
        {
            if (currentCell.GetCurrentUnit() != null)
            {
                currentCell.GetCurrentUnit().MoveUnitToCell(targetCell, false);
                UnitIsMoving = true;
                _battleController.BlockPlayerInput();
            }
        }

        private void OnUnitMovementEnded(Unit unit)
        {
            UnitIsMoving = false;
            _battleController.UnblockPlayerInput();
        }

        #endregion

        [Inject]
        private void Construct(Battlefield battlefield, BattleController battleController, CurrentTrain currentTrain)
        {
            _battlefield = battlefield;
            _battleController = battleController;
            _currentTrain = currentTrain;
        }

        private void ValidateDependencies()
        {
            if (_battlefield == null)
                throw new NullReferenceException("<b>[PlayerController]</b> _battlefield could not be injected!");

            if (_battleController == null)
                throw new NullReferenceException("<b>[PlayerController]</b> _battlefield could not be injected!");
        }
        /*
         * PlayerController is used to move checkers and block players input while is doing that
         */
    }
}