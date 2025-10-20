using System;
using System.Collections.Generic;
using Necro.Extra.Enums.NeighbourType;
using Necro.Extra.Enums.Team;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Necro.World.Board.Cell
{
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Team team;

        [SerializeField, Space(20f)]
        private MeshRenderer focus;
        [SerializeField]
        private MeshRenderer select;

        private readonly Dictionary<NeighbourType, Cell> _neighbours = new Dictionary<NeighbourType, Cell>(8);

        public event Action<Cell> OnPointerClickEvent;

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
            => OnPointerClickEvent?.Invoke(this);

        public void OnPointerEnter(PointerEventData eventData)
        {
            focus.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            focus.enabled = false;
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


        public void SetSelect(Material material)
        {
            select.enabled = true;
            select.material = material;
        }

        public void ResetSelect()
        {
            select.enabled = false;
        }

        private void FindAndFillNearestCells()
        {
            _neighbours.Clear();

            Cell[] cells = FindObjectsOfType<Cell>();
            List<(Cell cell, float distanceSqr)> list = new List<(Cell, float)>();

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] == this) continue; // пропускаем саму себя и не считаем соседом
                float distance = (cells[i].transform.position - transform.position).sqrMagnitude;
                list.Add((cells[i], distance));
            }

            // сортируем клетки по возрастанию расстояния от текущей клетки
            list.Sort((a, b) => a.distanceSqr.CompareTo(b.distanceSqr));
            int take = Mathf.Min(8, list.Count); // берём 8 ближайших клеток

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

        private void ValidateDependencies()
        {
            if (focus == null)
                throw new NullReferenceException("[Cell] focus is null!");

            if (select == null)
                throw new NullReferenceException("[Cell] select is null!");
        }
    }
}