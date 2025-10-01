using System;
using System.Collections.Generic;
using Korven.Data.Settings;
using Korven.Extra;
using Korven.Level.Board;
using UnityEngine;
using Zenject;

namespace Korven.Managers.CellManager
{
    public class CellManager : MonoBehaviour
    {
        private List<Cell> _cells;
        private Dictionary<Vector2Int, Cell> _cellMap;

        [Inject]
        private CellPaletteSettings _cellPaletteSettings;

        public Action<Cell> OnCellClicked;

        private void Awake()
        {
            FindNeighbourCells(); // This method is making neighbour some cells and is adding event OnCellClicked
        }

        private void FindNeighbourCells()
        {
            _cells = new List<Cell>(FindObjectsOfType<Cell>());
            _cellMap = new Dictionary<Vector2Int, Cell>();

            (Vector2Int offset, NeighbourType type)[]
                directions =
                {
                    (new Vector2Int(-1, 0), NeighbourType.Left),
                    (new Vector2Int(1, 0), NeighbourType.Right),
                    (new Vector2Int(0, 1), NeighbourType.Up),
                    (new Vector2Int(0, -1), NeighbourType.Down),

                    (new Vector2Int(-1, 1), NeighbourType.UpLeft),
                    (new Vector2Int(1, 1), NeighbourType.UpRight),
                    (new Vector2Int(-1, -1), NeighbourType.DownLeft),
                    (new Vector2Int(1, -1), NeighbourType.DownRight),
                };

            foreach (var cell in _cells)
            {
                cell.OnPointerClickEvent += OnCellClicked;
                cell.OnPointerClickEvent += HandleCellClicked;
                _cellMap[new Vector2Int(cell.X, cell.Y)] = cell;

                foreach (var direction in directions)
                {
                    var key = new Vector2Int(cell.X + direction.offset.x, cell.Y + direction.offset.y);
                    if (_cellMap.TryGetValue(key, out var neighbor))
                    {
                        cell.Neighbours[direction.type] = neighbor;
                    }
                }
            }
            /*
             * В ЗД "Работа с системой пользовательского ввода" сказано, что в CellManager должно быть:
             * "Находит всех юнитов в сцене и для каждого юнита находит клетку,
             * на которой тот стоит - задает связь в свойства Unit.Cell и Cell.Unit"
             * Но всё это у меня уже есть, хоть и не в CellManager, у Cell есть поле "UnitsCell" а у Unit "_currentCell"
             */
        }

        private void HandleCellClicked(Cell cell)
        {
            if (!cell.IsSelected)
            {
                cell.SetSelect(_cellPaletteSettings.SelectCellMaterial);
                cell.IsSelected = true;
            }
            else
            {
                cell.ResetSelect();
                cell.IsSelected = false;
            }
        }
    }
}