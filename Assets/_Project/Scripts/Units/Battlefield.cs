using System;
using System.Collections.Generic;
using Necro.Config.CellPalleteSettings;
using Necro.Extra.Enums.CurrentTrain;
using Necro.Extra.Enums.NeighbourType;
using Necro.Extra.Enums.Team;
using Necro.Extra.Enums.UnitrType;
using Necro.GamePlay.Controllers;
using Necro.GamePlay.Controllers.PlayerController;
using Necro.GamePlay.Units;
using Necro.World.Board.Cell;
using UnityEngine;
using Zenject;

namespace Necro.World.Battlefield
{
    public class Battlefield : MonoBehaviour
    {
        private BattleController _battleController; //injected
        private CellPalletSettings _cellPalletSettings; //injected
        private PlayerController _playerController; //injected

        [SerializeField]
        private BorderCells borderCells;
        [SerializeField]
        private GameObject confirmMovementText;

        // for movement
        private Cell _whiteFrCell;
        private Cell _whiteBrCell;
        private Cell _whiteFlCell;
        private Cell _whiteBlCell;

        private Cell _blackFrCell;
        private Cell _blackBrCell;
        private Cell _blackFlCell;
        private Cell _blackBlCell;

        private Cell _targetCell;
        private Cell _pastCell;

        private Cell _pendingTargetCell;
        private Cell _pendingFromCell;

        private bool _unitIsMoving;
        //

        private Cell[] _cells;
        private Unit[] _units;
        private List<Cell> _passedCells;
        public event Action<Cell, Cell> OnMoveRequestEvent; // fromCell, toCell

        private void Awake()
        {
            _cells = FindObjectsOfType<Cell>();
            _units = FindObjectsOfType<Unit>();

            _passedCells = new List<Cell>();

            ValidateDependencies();
        }

        private void OnEnable()
        {
            _battleController.OnPlayerMovementConfirmedEvent += OnPlayerMovementConfirmed;

            foreach (var t in _cells)
            {
                t.OnPointerClickEvent += OnCellClicked;
            }

            foreach (var t in _units)
            {
                t.OnMoveCompleted += OnUnitMoveCompleted;
            }
        }

        private void OnDisable()
        {
            _battleController.OnPlayerMovementConfirmedEvent -= OnPlayerMovementConfirmed;

            foreach (var t in _cells)
            {
                t.OnPointerClickEvent -= OnCellClicked;
            }

            foreach (var t in _units)
            {
                t.OnMoveCompleted -= OnUnitMoveCompleted;
            }
        }

        #region Public API

        public void IsUnitMoving(bool isMoving)
        {
            _unitIsMoving = isMoving;
        }

        public void HidePossibleMoves()
        {
            foreach (var c in _cells)
            {
                c.ResetSelect();
            }
        }

        #endregion

        #region Private Logic

        private void OnCellClicked(Cell cell)
        {
            if (_unitIsMoving) return;

            try
            {
                _passedCells.Add(cell);
                _pastCell = _passedCells[^2]; // ^2 == _passedCells.Count - 2
            }
            catch
            {
            }

            var cellsMeshRender = cell.GetComponentInChildren<MeshRenderer>();

            if (cellsMeshRender == null)
            {
                Debug.LogError($"<b>[Battlefield]</b> Cell {cell.gameObject.name} has no MeshRenderer");
            }
            else if (!cell.SelectIsActive && cell.GetCurrentUnit() != null)
            {
                if (cell.GetCurrentUnit().Team == Team.White)
                {
                    if (_playerController.GetCurrentTrain() == CurrentTrain.WhiteTeam)
                    {
                        TryToMarkPossibleMoves(cell);
                    }
                }
                else if (cell.GetCurrentUnit().Team == Team.Black)
                {
                    if (_playerController.GetCurrentTrain() == CurrentTrain.BlackTeam)
                    {
                        TryToMarkPossibleMoves(cell);
                    }
                }
            }
            else if (cell.SelectIsActive &&
                     cell.Select.material.color ==
                     _cellPalletSettings.SelectCellMaterial.color) // без .color не работало сравнение (==)
            {
                cell.ResetSelect();
                HidePossibleMoves();
            }

            else if (cell.SelectIsActive &&
                     cell.Select.material.color ==
                     _cellPalletSettings.MoveCellMaterial.color) // for movement
            {
                if (cell == null) Debug.LogWarning("<b>[Battlefield]</b> cell is null!");

                switch (cell)
                {
                    // white cells
                    case var c when c == _whiteFrCell:
                    {
                        HandleMoveToCell(_whiteFrCell);

                        break;
                    }

                    case var c when c == _whiteBrCell:
                    {
                        HandleMoveToCell(_whiteBrCell);

                        break;
                    }

                    case var c when c == _whiteFlCell:
                    {
                        HandleMoveToCell(_whiteFlCell);

                        break;
                    }

                    case var c when c == _whiteBlCell:
                    {
                        HandleMoveToCell(_whiteBlCell);

                        break;
                    }

                    // black cells
                    case var c when c == _blackFrCell:
                    {
                        HandleMoveToCell(_blackFrCell);

                        break;
                    }

                    case var c when c == _blackBrCell:
                    {
                        HandleMoveToCell(_blackBrCell);

                        break;
                    }

                    case var c when c == _blackFlCell:
                    {
                        HandleMoveToCell(_blackFlCell);

                        break;
                    }

                    case var c when c == _blackBlCell:
                    {
                        HandleMoveToCell(_blackBlCell);

                        break;
                    }

                    default:
                    {
                        HandleMoveToCell(cell);

                        break;
                    }
                }
            }
        }

        private void OnUnitMoveCompleted(Unit unit)
        {
            if (unit.GetCurrentCell().Select.material.color == _cellPalletSettings.MoveCellMaterial.color)
            {
                _battleController.SetGameStatusModeToMove(gameObject);

                // this code tets UnitType to Lady(дамка) 
                if (unit.Team == Team.White)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (unit.GetCurrentCell() == borderCells.blackTeamCells[i])
                        {
                            unit.SetUnitTypeToLady();
                        }
                    }
                }
                else if (unit.Team == Team.Black)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (unit.GetCurrentCell() == borderCells.whiteTeamCells[i])
                        {
                            unit.SetUnitTypeToLady();
                        }
                    }
                }
            }
            else if (unit.GetCurrentCell().Select.material.color == _cellPalletSettings.AttackCellMaterial.color)
            {
                _battleController.SetGameStatusModeToAttack(gameObject);
            }

            _playerController.ChangeCurrentTrainToOpositeOne();
        }

        private void TryToMarkPossibleMoves(Cell cell)
        {
            if (cell.GetCurrentUnit() == null) return;

            var cellsTeam = cell.GetCurrentUnit().Team;

            HidePossibleMoves();

            _whiteFrCell = _whiteBrCell = _whiteFlCell = _whiteBlCell = null;
            _blackFrCell = _blackBrCell = _blackFlCell = _blackBlCell = null;

            cell.SetSelect(_cellPalletSettings.SelectCellMaterial);

            switch (cellsTeam)
            {
                case Team.White:
                    if (cell.GetCurrentUnit().UnitType == UnitType.Default) // normal unit type
                    {
                        TryMarkDirection(cell, NeighbourType.ForwardRight, NeighbourType.ForwardRight, Team.Black,
                            ref _whiteFrCell);
                        TryMarkDirection(cell, NeighbourType.BackRight, NeighbourType.BackRight, Team.Black,
                            ref _whiteBrCell);

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackLeft, out var backLeft))
                        {
                            if (backLeft.GetCurrentUnit() != null && backLeft.GetCurrentUnit().Team == Team.Black)
                            {
                                if (backLeft.Neighbours.TryGetValue(NeighbourType.BackLeft, out var bL)
                                    && bL.GetCurrentUnit() == null)
                                {
                                    backLeft.SetSelect(_cellPalletSettings.AttackCellMaterial);
                                    bL.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                }
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var forwardLeft))
                        {
                            if (forwardLeft.GetCurrentUnit() != null && forwardLeft.GetCurrentUnit().Team == Team.Black)
                            {
                                if (forwardLeft.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var fL) &&
                                    fL.GetCurrentUnit() == null)
                                {
                                    forwardLeft.SetSelect(_cellPalletSettings.AttackCellMaterial);
                                    fL.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                }
                            }
                        }
                    }
                    else if (cell.GetCurrentUnit().UnitType == UnitType.Lady) // lady unit type
                    {
                        MarkLadyMoves(cell, NeighbourType.ForwardRight, Team.Black);
                        MarkLadyMoves(cell, NeighbourType.BackRight, Team.Black);
                        MarkLadyMoves(cell, NeighbourType.ForwardLeft, Team.Black);
                        MarkLadyMoves(cell, NeighbourType.BackLeft, Team.Black);
                    }

                    break;

                case Team.Black:
                    if (cell.GetCurrentUnit().UnitType == UnitType.Default) // normal unit type
                    {
                        TryMarkDirection(cell, NeighbourType.BackLeft, NeighbourType.BackLeft, Team.White,
                            ref _blackBlCell);
                        TryMarkDirection(cell, NeighbourType.ForwardLeft, NeighbourType.ForwardLeft, Team.White,
                            ref _blackFlCell);

                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardRight, out var forwardRight))
                        {
                            if (forwardRight.GetCurrentUnit() != null &&
                                forwardRight.GetCurrentUnit().Team == Team.White)
                            {
                                if (forwardRight.Neighbours.TryGetValue(NeighbourType.ForwardRight, out var fR)
                                    && fR.GetCurrentUnit() == null)
                                {
                                    forwardRight.SetSelect(_cellPalletSettings.AttackCellMaterial);
                                    fR.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                }
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackRight, out var backRight))
                        {
                            if (backRight.GetCurrentUnit() != null && backRight.GetCurrentUnit().Team == Team.White)
                            {
                                if (backRight.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var bR) &&
                                    bR.GetCurrentUnit() == null)
                                {
                                    backRight.SetSelect(_cellPalletSettings.AttackCellMaterial);
                                    bR.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                }
                            }
                        }
                    }
                    else if (cell.GetCurrentUnit().UnitType == UnitType.Lady) // lady unit type
                    {
                        MarkLadyMoves(cell, NeighbourType.ForwardRight, Team.White);
                        MarkLadyMoves(cell, NeighbourType.BackRight, Team.White);
                        MarkLadyMoves(cell, NeighbourType.ForwardLeft, Team.White);
                        MarkLadyMoves(cell, NeighbourType.BackLeft, Team.White);
                    }

                    break;
            }
        }

        private void MarkLadyMoves(Cell originCell, NeighbourType direction, Team enemyTeam)
        {
            var currentCell = originCell;

            while (true)
            {
                if (!currentCell.Neighbours.TryGetValue(direction, out var nextCell) || nextCell == null)
                    break;

                var unit = nextCell.GetCurrentUnit();

                if (unit == null)
                {
                    nextCell.SetSelect(_cellPalletSettings.MoveCellMaterial);
                    currentCell = nextCell;
                    continue;
                }

                if (!nextCell.Neighbours.TryGetValue(direction, out var jumpCell) || jumpCell == null)
                    break;

                if (jumpCell.GetCurrentUnit() == null)
                {
                    nextCell.SetSelect(_cellPalletSettings.AttackCellMaterial);
                    jumpCell.SetSelect(_cellPalletSettings.MoveCellMaterial);
                }

                break;
            }
        }

        private bool TryMarkDirection(Cell originCell, NeighbourType step, NeighbourType jump, Team enemyTeam,
            ref Cell outTargetCell)
        {
            if (originCell == null) return false;
            if (!originCell.Neighbours.TryGetValue(step, out var stepCell) || stepCell == null) return false;

            var stepUnit = stepCell.GetCurrentUnit();

            if (stepUnit == null)
            {
                stepCell.SetSelect(_cellPalletSettings.MoveCellMaterial);
                outTargetCell = stepCell;
                return true;
            }

            if (stepUnit != null && stepUnit.Team == enemyTeam)
            {
                stepCell.Neighbours.TryGetValue(jump, out var jumpCell);

                if (jumpCell == null) return false;

                if (jumpCell.GetCurrentUnit() == null && stepCell.GetCurrentUnit().Team == enemyTeam)
                {
                    stepCell.SetSelect(_cellPalletSettings.AttackCellMaterial);
                    jumpCell.SetSelect(_cellPalletSettings.MoveCellMaterial);
                    outTargetCell = jumpCell;
                    return true;
                }
            }

            return false;
        }

        private void OnPlayerMovementConfirmed(bool isConfirmed)
        {
            if (_pendingTargetCell != null && _pendingFromCell != null && isConfirmed == true)
            {
                confirmMovementText.SetActive(false);
                OnMoveRequestEvent?.Invoke(_pendingFromCell, _pendingTargetCell);
                _pendingTargetCell = null;
                _pendingFromCell = null;

                HidePossibleMoves();
                _battleController.SetGameStatusModeToMove(gameObject);
            }
            else if (isConfirmed == false)
            {
                _pendingTargetCell = null;
                _pendingFromCell = null;
                HidePossibleMoves();
                _battleController.SetGameStatusModeToLastOne(gameObject);
            }
        }

        private void HandleMoveToCell(Cell targetCell)
        {
            if (_pastCell.Select.material.color == _cellPalletSettings.SelectCellMaterial.color)
            {
                confirmMovementText.SetActive(true);
                _pendingFromCell = _pastCell;
                _pendingTargetCell = targetCell;
                _battleController.SetGameStatusModeToConfirmMove(gameObject);
            }
        }

        #endregion

        [Inject]
        private void Construct(CellPalletSettings cellPalletSettings, BattleController battleController,
            PlayerController playerController)
        {
            _cellPalletSettings = cellPalletSettings;
            _battleController = battleController;
            _playerController = playerController;
        }

        private void ValidateDependencies()
        {
            if (_battleController == null)
                throw new NullReferenceException("<b>[Battlefield]</b> BattleController could not be injected!");

            if (_cellPalletSettings == null)
                throw new NullReferenceException("<b>[Battlefield]</b> _cellPalletSettings could not be injected!");

            if (_playerController == null)
                throw new NullReferenceException("<b>[Battlefield]</b> _playerController could not be injected!");

            if (confirmMovementText == null)
                throw new NullReferenceException("<b>[Battlefield]</b> _confirmMovementText is null!");
        }
        /*
         * Battlefields task is mark Cells and send event to PlayerController if player is going to move checker
         */

        [Serializable]
        private struct BorderCells
        {
            public Cell[] whiteTeamCells;
            public Cell[] blackTeamCells;
        }
    }
}