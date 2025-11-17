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

        // for movement
        private Cell _whiteFrCell;
        private Cell _whiteBrCell;

        private Cell _blackFlCell;
        private Cell _blackBlCell;

        private Cell _targetCell;
        private Cell _pastCell;

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
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].OnPointerClickEvent += OnCellClicked;
            }

            for (int i = 0; i < _units.Length; i++)
            {
                _units[i].OnMoveCompleted += OnUnitMoveCompleted;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].OnPointerClickEvent -= OnCellClicked;
            }

            for (int i = 0; i < _units.Length; i++)
            {
                _units[i].OnMoveCompleted += OnUnitMoveCompleted;
            }
        }

        #region Public API

        public void IsUnitMoving(bool isMoving)
        {
            _unitIsMoving = isMoving;
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
            else if (!cell.SelectIsActive && cell.CurrentUnit != null)
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
                TryToHidePossibleMoves(cell);
            }

            else if (cell.SelectIsActive &&
                     cell.Select.material.color ==
                     _cellPalletSettings.MoveCellMaterial.color) // for movement
            {
                if (cell == null) Debug.LogWarning("<b>[Battlefield]</b> cell is null!");

                switch (cell)
                {
                    case var c when c == _whiteFrCell:
                    {
                        TryToHidePossibleMoves(_pastCell);
                        _pastCell.ResetSelect();

                        if (_pastCell.Select.material.color == _cellPalletSettings.SelectCellMaterial.color)
                        {
                            OnMoveRequestEvent?.Invoke(_pastCell, _whiteFrCell);
                        }

                        break;
                    }


                    case var c when c == _whiteBrCell:
                    {
                        TryToHidePossibleMoves(_pastCell);
                        _pastCell.ResetSelect();

                        if (_pastCell.Select.material.color == _cellPalletSettings.SelectCellMaterial.color)
                        {
                            OnMoveRequestEvent?.Invoke(_pastCell, _whiteBrCell);
                        }

                        break;
                    }


                    case var c when c == _blackFlCell:
                    {
                        TryToHidePossibleMoves(_pastCell);
                        _pastCell.ResetSelect();

                        if (_pastCell.Select.material.color == _cellPalletSettings.SelectCellMaterial.color)
                        {
                            OnMoveRequestEvent?.Invoke(_pastCell, _blackFlCell);
                        }

                        break;
                    }

                    case var c when c == _blackBlCell:
                    {
                        TryToHidePossibleMoves(_pastCell);
                        _pastCell.ResetSelect();

                        if (_pastCell.Select.material.color == _cellPalletSettings.SelectCellMaterial.color)
                        {
                            OnMoveRequestEvent?.Invoke(_pastCell, _blackBlCell);
                        }

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
            if (cell.CurrentUnit == null) return;

            var cellsTeam = cell.CurrentUnit.Team;

            foreach (var c in _cells)
            {
                c.ResetSelect();
            }

            cell.SetSelect(_cellPalletSettings.SelectCellMaterial);

            switch (cellsTeam)
            {
                case Team.White:
                    if (cell.GetCurrentUnit().UnitType == UnitType.Default) // normal unit type
                    {
                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardRight, out var fr) && fr != null)
                        {
                            if (fr.CurrentUnit == null)
                            {
                                fr.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _whiteFrCell = fr;
                            }
                            else if (fr.CurrentUnit != null && fr.CurrentUnit.Team == Team.Black)
                            {
                                fr.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackRight, out var br) && br != null)
                        {
                            if (br.CurrentUnit == null)
                            {
                                br.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _whiteBrCell = br;
                            }
                            else if (br.CurrentUnit != null && br.CurrentUnit.Team == Team.Black)
                            {
                                br.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }
                    }
                    else if (cell.GetCurrentUnit().UnitType == UnitType.Lady) // lady unit ype
                    {
                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardRight, out var fr) && fr != null)
                        {
                            if (fr.CurrentUnit == null)
                            {
                                fr.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _whiteFrCell = fr;
                            }
                            else if (fr.CurrentUnit != null && fr.CurrentUnit.Team == Team.Black)
                            {
                                fr.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackRight, out var br) && br != null)
                        {
                            if (br.CurrentUnit == null)
                            {
                                br.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _whiteBrCell = br;
                            }
                            else if (br.CurrentUnit != null && br.CurrentUnit.Team == Team.Black)
                            {
                                br.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackLeft, out var barkLeft) && barkLeft != null)
                        {
                            if (barkLeft.CurrentUnit == null)
                            {
                                barkLeft.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _blackBlCell = barkLeft;
                            }
                            else if (barkLeft.CurrentUnit != null && barkLeft.CurrentUnit.Team == Team.White)
                            {
                                barkLeft.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var forwardLeft) &&
                            forwardLeft != null)
                        {
                            if (forwardLeft.CurrentUnit == null)
                            {
                                forwardLeft.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _blackFlCell = forwardLeft;
                            }
                            else if (forwardLeft.CurrentUnit != null && forwardLeft.CurrentUnit.Team == Team.White)
                            {
                                forwardLeft.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }
                    }

                    break;

                case Team.Black:
                    if (cell.GetCurrentUnit().UnitType == UnitType.Default)
                    {
                        if (cell.Neighbours.TryGetValue(NeighbourType.BackLeft, out var bl) && bl != null)
                        {
                            if (bl.CurrentUnit == null)
                            {
                                bl.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _blackBlCell = bl;
                            }
                            else if (bl.CurrentUnit != null && bl.CurrentUnit.Team == Team.White)
                            {
                                bl.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var fl) && fl != null)
                        {
                            if (fl.CurrentUnit == null)
                            {
                                fl.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _blackFlCell = fl;
                            }
                            else if (fl.CurrentUnit != null && fl.CurrentUnit.Team == Team.White)
                            {
                                fl.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }
                    }
                    else if (cell.GetCurrentUnit().UnitType == UnitType.Lady)
                    {
                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardRight, out var fr) && fr != null)
                        {
                            if (fr.CurrentUnit == null)
                            {
                                fr.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _whiteFrCell = fr;
                            }
                            else if (fr.CurrentUnit != null && fr.CurrentUnit.Team == Team.Black)
                            {
                                fr.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackRight, out var br) && br != null)
                        {
                            if (br.CurrentUnit == null)
                            {
                                br.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _whiteBrCell = br;
                            }
                            else if (br.CurrentUnit != null && br.CurrentUnit.Team == Team.Black)
                            {
                                br.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.BackLeft, out var barkLeft) && barkLeft != null)
                        {
                            if (barkLeft.CurrentUnit == null)
                            {
                                barkLeft.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _blackBlCell = barkLeft;
                            }
                            else if (barkLeft.CurrentUnit != null && barkLeft.CurrentUnit.Team == Team.White)
                            {
                                barkLeft.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }

                        if (cell.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var forwardLeft) &&
                            forwardLeft != null)
                        {
                            if (forwardLeft.CurrentUnit == null)
                            {
                                forwardLeft.SetSelect(_cellPalletSettings.MoveCellMaterial);
                                _blackFlCell = forwardLeft;
                            }
                            else if (forwardLeft.CurrentUnit != null && forwardLeft.CurrentUnit.Team == Team.White)
                            {
                                forwardLeft.SetSelect(_cellPalletSettings.AttackCellMaterial);
                            }
                        }
                    }

                    break;
            }
        }

        private void TryToHidePossibleMoves(Cell cell)
        {
            if (cell.CurrentUnit == null) return;

            var cellsTeam = cell.CurrentUnit.Team;
            switch (cellsTeam)
            {
                case Team.White:
                    if (cell.Neighbours.TryGetValue(NeighbourType.ForwardRight, out var fr) && fr != null)
                        fr.ResetSelect();

                    if (cell.Neighbours.TryGetValue(NeighbourType.BackRight, out var br) && br != null)
                        br.ResetSelect();

                    break;

                case Team.Black:
                    if (cell.Neighbours.TryGetValue(NeighbourType.BackLeft, out var bl) && bl != null)
                        bl.ResetSelect();

                    if (cell.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var fl) && fl != null)
                        fl.ResetSelect();
                    break;
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