using System;
using Necro.World.Battlefield;
using Necro.World.Board.Cell;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Necro.GamePlay.Controllers.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        private Battlefield _battlefield; //injected

        // for movement
        private Cell _currentCell;
        private Cell _targetCell;

        private bool _isMoving = false;

        private void Awake()
        {
            ValidateDependencies();
        }

        private void Update()
        {
            if (_isMoving)
            {
                _currentCell.CurrentUnit.Move(_targetCell);
            }
        }

        private void OnEnable()
        {
            _battlefield.OnMoveRequestEvent += OnUnitMovementStarted;
        }

        private void OnDisable()
        {
            _battlefield.OnMoveRequestEvent -= OnUnitMovementStarted;
        }

        private void OnUnitMovementStarted(Cell currentCell, Cell targetCell)
        {
            Debug.Log("<b>[PlayerController]</b> OnUnitMovementStarted method was called");
            var keyboard = Keyboard.current;

            _isMoving = true;
        }

        [Inject]
        private void Construct(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        private void ValidateDependencies()
        {
            if (_battlefield == null)
                throw new NullReferenceException("<b>[PlayerController]</b> _battlefield could not be injected!");
        }
    }
}