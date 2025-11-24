using System;
using System.Collections.Generic;
using Necro.Config.CellPalleteSettings;
using Necro.Config.DefaultSettings;
using Necro.Extra.Enums.NeighbourType;
using Necro.GamePlay.Controllers;
using Necro.GamePlay.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Necro.World.Board.Cell
{
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private BattleController _battleController; // injected
        private DefaultSettings _projectSettings; //inject
        private CellPalletSettings _cellPalletSettings; // injected

        [SerializeField, Space(10f)]
        private MeshRenderer focus;
        [SerializeField]
        private MeshRenderer select;
        [SerializeField]
        private Material whiteCellMaterial;
        [SerializeField]
        private Material blackCellMaterial;

        private Unit _currentUnit;

        public MeshRenderer Select => select;
        private MeshRenderer _meshRenderer;

        public bool SelectIsActive { get; private set; } = false;

        public readonly Dictionary<NeighbourType, Cell> Neighbours = new Dictionary<NeighbourType, Cell>(8);
        public List<Cell> possibleMovesForLady = new List<Cell>();

        public event Action<Cell> OnPointerClickEvent;

        private void Awake()
        {
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            ValidateDependencies();
        }

        private void Start()
        {
            FindAndFillNearestCells();
            focus.enabled = false;
            select.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData) => OnCellEntered(this);
        public void OnPointerExit(PointerEventData eventData) => OnCellExited(this);
        public void OnPointerClick(PointerEventData eventData) => OnCellClicked(this);

        private void OnTriggerEnter(Collider other)
        {
            var unit = other.GetComponent<Unit>();
            if (unit == null)
                return;

            if (_currentUnit != null && _currentUnit != unit)
            {
                _currentUnit.DestroyUnit();
                UnsubscribeFromUnit(_currentUnit);
                _currentUnit = null;
            }
            else if (_meshRenderer.material.color == blackCellMaterial.color)
            {
                _currentUnit = unit;
                SubscribeToUnit(_currentUnit);
                _currentUnit.SetCurrentCell(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var unit = other.GetComponent<Unit>();
            if (unit == null)
                return;

            if (_currentUnit == unit)
            {
                UnsubscribeFromUnit(_currentUnit);
                _currentUnit = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Neighbours == null) return;
            Gizmos.color = Color.magenta;
            foreach (var kv in Neighbours)
            {
                if (kv.Value == null) continue;
                Gizmos.DrawLine(transform.position, kv.Value.transform.position);
                Gizmos.DrawSphere(kv.Value.transform.position, 0.1f);
            }
        }

        #region Public API

        public Unit GetCurrentUnit() => _currentUnit;
        public void SetCurrentUnit(Unit unit) => _currentUnit = unit;

        public void SetSelect(Material material)
        {
            select.enabled = true;
            select.material = material;
            _battleController.SetGameStatusModeToSelect(gameObject);
            SelectIsActive = true;
        }

        public void ResetSelect()
        {
            select.enabled = false;
            SelectIsActive = false;
        }

        #endregion

        #region Private Logic

        private void FindAndFillNearestCells()
        {
            Neighbours.Clear();
            Cell[] cells = FindObjectsOfType<Cell>();
            List<(Cell cell, float distanceSqr)> list = new List<(Cell, float)>();

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] == this) continue;
                float distance = (cells[i].transform.position - transform.position).sqrMagnitude;
                list.Add((cells[i], distance));
            }

            list.Sort((a, b) => a.distanceSqr.CompareTo(b.distanceSqr));
            int take = Mathf.Min(8, list.Count);

            for (int i = 0; i < take; i++)
            {
                Vector3 offset = list[i].cell.transform.position - transform.position;
                NeighbourType type = GetNeighbourType(offset, 0.5f);

                if (!Neighbours.ContainsKey(type))
                {
                    Neighbours[type] = list[i].cell;
                    if (Neighbours.Count == 8) break;
                }
            }
        }

        private NeighbourType GetNeighbourType(Vector3 offset, float threshold)
        {
            float dx = offset.x;
            float dz = offset.z;

            bool right = dx > threshold;
            bool left = dx < -threshold;
            bool forward = dz > threshold;
            bool back = dz < -threshold;

            if (forward && !right && !left) return NeighbourType.Forward;
            if (back && !right && !left) return NeighbourType.Back;
            if (right && !forward && !back) return NeighbourType.Right;
            if (left && !forward && !back) return NeighbourType.Left;
            if (forward && right) return NeighbourType.ForwardRight;
            if (forward && left) return NeighbourType.ForwardLeft;
            if (back && right) return NeighbourType.BackRight;
            return NeighbourType.BackLeft;
        }

        private void SubscribeToUnit(Unit unit)
        {
            if (unit == null) return;
            unit.OnUnitEnter += OnCellEntered;
            unit.OnUnitExit += OnCellExited;
            unit.OnUnitClicked += OnCellClicked;
        }

        private void UnsubscribeFromUnit(Unit unit)
        {
            if (unit == null) return;
            unit.OnUnitEnter -= OnCellEntered;
            unit.OnUnitExit -= OnCellExited;
            unit.OnUnitClicked -= OnCellClicked;
        }

        private void OnCellClicked(Cell cell) => OnPointerClickEvent?.Invoke(cell);
        private void OnCellEntered(Cell cell) => cell.focus.enabled = true;
        private void OnCellExited(Cell cell) => cell.focus.enabled = false;

        #endregion

        [Inject]
        private void Construct(BattleController battleController, CellPalletSettings cellPalletSettings,
            DefaultSettings defaultSettings)
        {
            _battleController = battleController;
            _cellPalletSettings = cellPalletSettings;
            _projectSettings = defaultSettings;
        }

        private void ValidateDependencies()
        {
            if (focus == null)
                throw new NullReferenceException("<b>[Cell]</b> focus is null!");
            if (select == null)
                throw new NullReferenceException("<b>[Cell]</b> select is null!");
            if (_battleController == null)
                throw new NullReferenceException("<b>[Cell]</b> _battleController is could not be injected!");
            if (_cellPalletSettings == null)
                throw new NullReferenceException("<b>[Cell]</b> _cellPalletSettings is could not be injected!");
            if (_projectSettings == null)
                throw new NullReferenceException("[Cell]</b> _projectSettings is could not be injected!");
        }

        /*
         * Class Cell is used to control cells on Battlefield
         */
    }
}