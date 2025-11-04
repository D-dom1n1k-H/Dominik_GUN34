using System;
using Necro.Config.CellPalleteSettings;
using Necro.Extra.Enums.NeighbourType;
using Necro.Extra.Enums.Team;
using Necro.GamePlay.Controllers;
using Necro.World.Board.Cell;
using UnityEngine;
using Zenject;

namespace Necro.World.Battlefield
{
    public class Battlefield : MonoBehaviour
    {
        private BattleController _battleController; //injected
        private CellPalletSettings _cellPalletSettings; //injected

        private Cell[] _cells;

        // for movement
        private Cell _whiteFrCell;
        private Cell _whiteBrCell;

        private Cell _blackFlCell;
        private Cell _blackBlCell;

        private Cell _targetCell;
        public event Action<Cell, Cell> OnMoveRequestEvent; // fromCell, toCell

        private void Awake()
        {
            _cells = FindObjectsOfType<Cell>();
            ValidateDependencies();
        }

        private void Update()
        {
        }

        private void OnEnable()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].OnPointerClickEvent += OnCellClicked;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].OnPointerClickEvent -= OnCellClicked;
            }
        }

        private void OnCellClicked(Cell cell)
        {
            var cellsMeshRender = cell.GetComponentInChildren<MeshRenderer>();

            if (cellsMeshRender == null)
            {
                Debug.LogError($"<b>[Battlefield]</b> Cell {cell.gameObject.name} has no MeshRenderer");
            }
            else if (!cell.SelectIsActive && cell.CurrentUnit != null)
            {
                TryToMarkPossibleMoves(cell);
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
                Debug.Log("<b>[Battlefield]</b> cell.Select.material == _cellPalletSettings.MoveCellMaterial");

                switch (cell)
                {
                    case var c when c == _whiteFrCell:

                        if (cell == null) Debug.LogWarning("<b>[Battlefield]</b> cell is null!");
                        if (_whiteFrCell == null) Debug.LogWarning("<b>[Battlefield]</b> _whiteFrCell is null!");
                        else
                        {
                            OnMoveRequestEvent.Invoke(cell, _whiteFrCell);
                        }

                        break;

                    case var c when c == _whiteBrCell:

                        if (cell == null) Debug.LogWarning("<b>[Battlefield]</b> cell is null!");
                        if (_whiteBrCell == null) Debug.LogWarning("<b>[Battlefield]</b> _whiteBrCell is null!");
                        else
                        {
                            OnMoveRequestEvent.Invoke(cell, _whiteBrCell);
                        }

                        break;

                    case var c when c == _blackFlCell:

                        if (cell == null) Debug.LogWarning("<b>[Battlefield]</b> cell is null!");
                        if (_blackFlCell == null) Debug.LogWarning("<b>[Battlefield]</b> _blackFlCell is null!");
                        else
                        {
                            OnMoveRequestEvent.Invoke(cell, _blackFlCell);
                        }

                        break;

                    case var c when c == _blackBlCell:

                        if (cell == null) Debug.LogWarning("<b>[Battlefield]</b> cell is null!");
                        if (_blackBlCell == null) Debug.LogWarning("<b>[Battlefield]</b> _blackBlCell is null!");
                        else
                        {
                            OnMoveRequestEvent.Invoke(cell, _blackBlCell);
                        }

                        break;
                }
            }

            Debug.Log("<b>[Battlefield]</b> OnCellClicked method was called");
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

                    break;

                case Team.Black:
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

                    break;
            }

            Debug.Log("<b>[Battlefield]</b> TryToMarkPossibleMoves method was called");
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

        [Inject]
        private void Construct(CellPalletSettings cellPalletSettings, BattleController battleController)
        {
            _cellPalletSettings = cellPalletSettings;
            _battleController = battleController;
        }

        private void ValidateDependencies()
        {
            if (_battleController == null)
                throw new NullReferenceException("<b>[Battlefield]</b> BattleController could not be injected!");

            if (_cellPalletSettings == null)
                throw new NullReferenceException("<b>[Battlefield]</b> _cellPalletSettings could not be injected!");
        }
    }
}