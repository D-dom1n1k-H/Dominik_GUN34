using System;
using Necro.Config.CellPalleteSettings;
using Necro.Extra.Enums.NeighbourType;
using Necro.Extra.Enums.Team;
using Necro.GamePlay.Controllers;
using Necro.World.Board.Cell;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Necro.World.Battlefield
{
    public class Battlefield : MonoBehaviour
    {
        private BattleController _battleController; //injected
        private CellPalletSettings _cellPalletSettings; //injected

        private Cell[] _cells;

        private void Awake()
        {
            _cells = FindObjectsOfType<Cell>();
            ValidateDependencies();
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
                Debug.LogError($"<b>[BattleController]</b> Cell {cell.gameObject.name} has no MeshRenderer");
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

            Debug.Log("<b>[BattleController]</b> OnCellClicked method was called");
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
                        }
                        else if(fr.CurrentUnit != null && fr.CurrentUnit.Team == Team.Black)
                        {
                            fr.SetSelect(_cellPalletSettings.AttackCellMaterial);
                        }
                    }

                    if (cell.Neighbours.TryGetValue(NeighbourType.BackRight, out var br) && br != null)
                    {
                        if (br.CurrentUnit == null)
                        {
                            br.SetSelect(_cellPalletSettings.MoveCellMaterial);
                        }
                        else if(br.CurrentUnit != null && br.CurrentUnit.Team == Team.Black)
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
                        }
                        else if(bl.CurrentUnit != null && bl.CurrentUnit.Team == Team.White)
                        {
                            bl.SetSelect(_cellPalletSettings.AttackCellMaterial);
                        }
                    }

                    if (cell.Neighbours.TryGetValue(NeighbourType.ForwardLeft, out var fl) && fl != null)
                    {
                        if (fl.CurrentUnit == null)
                        {
                            fl.SetSelect(_cellPalletSettings.MoveCellMaterial);
                        }
                        else if(fl.CurrentUnit != null && fl.CurrentUnit.Team == Team.White)
                        {
                            fl.SetSelect(_cellPalletSettings.AttackCellMaterial);
                        }
                    }

                    break;
            }

            Debug.Log("<b>[BattleController]</b> TryToMarkPossibleMoves method was called");
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
                throw new NullReferenceException("<b>[BattleController]</b> BattleController could not be injected!");

            if (_cellPalletSettings == null)
                throw new NullReferenceException(
                    "<b>[BattleController]</b> _cellPalletSettings could not be injected!");
        }
    }
}