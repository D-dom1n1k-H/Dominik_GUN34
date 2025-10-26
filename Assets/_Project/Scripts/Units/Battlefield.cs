using System;
using Necro.Config.CellPalleteSettings;
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

        private void Start()
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
                Debug.LogError($"[Battlefield] Cell {cell.gameObject.name} has no MeshRenderer");
            }
            else if (!cell.SelectIsActive)
            {
                cell.SetSelect(_cellPalletSettings.SelectCellMaterial);
            }
            else
            {
                cell.ResetSelect();
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
            if(_battleController == null)
                throw new NullReferenceException("[Battlefield] BattleController is could not be injected!");
        }
    }
}