using System;
using Necro.Extra.Enums.CurrentTrain;
using Necro.GamePlay.Units;
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
        private CurrentTrain _currentTrain; //injected
        
        [SerializeField]
        private Image arrowImage;

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
                arrowImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (randomValue == 1)
            {
                _currentTrain = CurrentTrain.BlackTeam;
                arrowImage.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            Debug.Log("<b>[PlayerController]</b> current train: " + _currentTrain);
        }
        
        private void OnEnable()
        {
            _battlefield.OnMoveRequestEvent += OnUnitMovementStarted;
            _battleController.OnPlayerMovementConfirmedEvent += ChangeIsMovementConfirmedField;

            foreach (var t in _units)
            {
                t.OnMoveCompleted += OnUnitMovementEnded;
            }
        }

        private void OnDisable()
        {
            _battlefield.OnMoveRequestEvent -= OnUnitMovementStarted;
            _battleController.OnPlayerMovementConfirmedEvent -= ChangeIsMovementConfirmedField;
            
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
                arrowImage.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (_currentTrain == CurrentTrain.BlackTeam)
            {
                _currentTrain = CurrentTrain.WhiteTeam;
                arrowImage.transform.rotation = Quaternion.Euler(0, 0, 0);
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

        private void ChangeIsMovementConfirmedField(bool isConfirmed)
        {
            _isMovementConfirmed = isConfirmed;
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

            if (arrowImage == null)
                throw new NullReferenceException("<b>[PlayerController]</b> arrowImage is null!");
        }
        /*
         * PlayerController is used to move checkers and block players input while is doing that
         */
    }
}