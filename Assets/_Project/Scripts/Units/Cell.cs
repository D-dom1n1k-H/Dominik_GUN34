using System;
using System.Collections.Generic;
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
        private BattleController _battleController; //injected

        [SerializeField, Space(10f)]
        private MeshRenderer focus;
        [SerializeField]
        private MeshRenderer select;

        public bool SelectIsActive { get; private set; } = false;

        private Unit _currentUnit;

        private readonly Dictionary<NeighbourType, Cell> _neighbours = new Dictionary<NeighbourType, Cell>(8);

        public event Action<Cell> OnPointerClickEvent;
        public event Action<Cell> OnShareCellEvent; //передаёт себя в текущий юнит

        private void Awake()
        {
            ValidateDependencies();
        }

        private void Start()
        {
            FindAndFillNearestCells();

            focus.enabled = false;
            select.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
            => OnCellClicked(this);

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnCellEntered(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnCellExited(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Unit>(out var unit))
                return;

            if (_currentUnit != null && _currentUnit != unit)
            {
                UnsubscribeFromUnit(_currentUnit);
                _currentUnit = null;
            }

            if (_currentUnit == null)
            {
                _currentUnit = unit;
                SubscribeToUnit(_currentUnit);
                OnShareCellEvent?.Invoke(this);
                Debug.Log("[Cell] OnShareCellEvent was called");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Unit>(out var unit))
                return;

            if (_currentUnit != null && _currentUnit == unit)
            {
                UnsubscribeFromUnit(_currentUnit);
                _currentUnit = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_neighbours == null) return;
            Gizmos.color = Color.magenta;
            foreach (var kv in _neighbours)
            {
                if (kv.Value == null) continue;
                Gizmos.DrawLine(transform.position, kv.Value.transform.position);
                Gizmos.DrawSphere(kv.Value.transform.position, 0.1f);
            }
        }

        private void OnDisable()
        {
            if (_currentUnit != null)
            {
                UnsubscribeFromUnit(_currentUnit);
                _currentUnit = null;
            }
        }

        public void SetSelect(Material material)
        {
            select.enabled = true;
            select.material = material;

            SelectIsActive = true;
        }

        public void ResetSelect()
        {
            select.enabled = false;
            SelectIsActive = false;
        }

        private void FindAndFillNearestCells()
        {
            _neighbours.Clear();

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

                if (!_neighbours.ContainsKey(type))
                {
                    _neighbours[type] = list[i].cell;

                    if (_neighbours.Count == 8) break;
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

        private void OnCellClicked(Cell cell)
        {
            OnPointerClickEvent?.Invoke(cell);
            _battleController.SetGameStatusModeToSelect(gameObject);
        }

        private void OnCellEntered(Cell cell)
        {
            cell.focus.enabled = true;
        }

        private void OnCellExited(Cell cell)
        {
            cell.focus.enabled = false;
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

        [Inject]
        private void Construct(BattleController battleController)
        {
            _battleController = battleController;
        }

        private void ValidateDependencies()
        {
            if (focus == null)
                throw new NullReferenceException("[Cell] focus is null!");

            if (select == null)
                throw new NullReferenceException("[Cell] select is null!");

            if (_battleController == null)
                throw new NullReferenceException("[Cell] BattleController is could not be injected!");
        }
    }
}