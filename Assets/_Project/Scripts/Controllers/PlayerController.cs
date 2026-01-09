using System;
using Necro.Extra.Enums.CurrentTrain;
using Necro.GamePlay.Units;
using Necro.Interfaces.WorldSpaceCanvasController;
using Necro.World.Battlefield;
using Necro.World.Board.Cell;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = Unity.Mathematics.Random;

namespace Necro.GamePlay.Controllers.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        private Battlefield _battlefield; //injected
        private BattleController _battleController; //injected
        private WorldSpaceCanvasController _worldSpaceCanvasController; //injected
        private CurrentTrain _currentTrain; //injected

        // for movement
        private Cell _currentCell;
        private Cell _targetCell;

        private Unit[] _units;

        private bool _isMovementConfirmed = false;
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
            {
                _currentTrain = CurrentTrain.WhiteTeam;
                _worldSpaceCanvasController.TurnArrowImageToOpositeSide();
            }
            else if (randomValue == 1)
            {
                _currentTrain = CurrentTrain.BlackTeam;
                _worldSpaceCanvasController.TurnArrowImageToOpositeSide();
            }

            Debug.Log("<b>[PlayerController]</b> current train: " + _currentTrain);
        }

        private void OnEnable()
        {
            _battlefield.OnMoveRequestEvent += OnUnitMovementStarted;

            foreach (var t in _units)
            {
                t.OnMoveCompleted += OnUnitMovementEnded;
            }
        }

        private void OnDisable()
        {
            _battlefield.OnMoveRequestEvent -= OnUnitMovementStarted;

            foreach (var t in _units)
            {
                t.OnMoveCompleted -= OnUnitMovementEnded;
            }
        }

        #region Public API

        public CurrentTrain GetCurrentTrain()
        {
            return _currentTrain;
        }

        public bool GetIsMovementConfirmed()
        {
            return _isMovementConfirmed;
        }

        public void ChangeCurrentTrainToOpositeOne()
        {
            if (_currentTrain == CurrentTrain.WhiteTeam)
            {
                _currentTrain = CurrentTrain.BlackTeam;
                _worldSpaceCanvasController.TurnArrowImageToOpositeSide();
            }
            else if (_currentTrain == CurrentTrain.BlackTeam)
            {
                _currentTrain = CurrentTrain.WhiteTeam;
                _worldSpaceCanvasController.TurnArrowImageToOpositeSide();
            }

            Debug.Log($"<b>[PlayerController]</b> CurrentTrain was changed to: {_currentTrain}");
        }

        #endregion

        #region Private Logic

        private void OnUnitMovementStarted(Cell currentCell, Cell targetCell)
        {
            if (currentCell.GetCurrentUnit() != null && _isMovementConfirmed)
            {
                currentCell.GetCurrentUnit().MoveUnitToCell(targetCell);
                UnitIsMoving = true;
                _isMovementConfirmed = false;
                _battleController.BlockPlayerInput();
            }
        }

        private void OnUnitMovementEnded(Unit unit)
        {
            UnitIsMoving = false;
            _battleController.UnblockPlayerInput();
        }

        public void ChangeIsMovementConfirmedField(bool isConfirmed) => _isMovementConfirmed = isConfirmed;

        #endregion

        #region Initialization

        [Inject]
        private void Construct(Battlefield battlefield, BattleController battleController,
            WorldSpaceCanvasController worldSpaceCanvasController, CurrentTrain currentTrain)
        {
            _battlefield = battlefield;
            _battleController = battleController;
            _worldSpaceCanvasController = worldSpaceCanvasController;
            _currentTrain = currentTrain;
        }

        private void ValidateDependencies()
        {
            if (_battlefield == null)
                throw new MissingReferenceException("[PlayerController] _battlefield was not assigned or injected.");

            if (_battleController == null)
                throw new MissingReferenceException(
                    "[PlayerController] _battleController was not assigned or injected.");

            if (_worldSpaceCanvasController == null)
                throw new MissingReferenceException(
                    "[PlayerController] _worldSpaceCanvasController was not assigned or injected.");
        }

        #endregion

        /*
         * PlayerController is used to move checkers and block players input while player is doing that
         */
    }
}