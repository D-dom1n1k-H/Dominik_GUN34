using System;
using System.Collections.Generic;
using Korven.Extra;
using Korven.Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Korven.Level.Board
{
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private MeshRenderer focus;
        [SerializeField]
        private MeshRenderer select;
        [NonSerialized]
        public bool IsSelected;
        public int X { get; set; }
        public int Y { get; set; }
        public Dictionary<NeighbourType, Cell> Neighbours { get; } = new();

        private Cell UnitsCell;

        public event Action<Cell> OnPointerClickEvent;

        private void Awake()
        {
            if (ValidateDependencies())
            {
                focus.enabled = false;
                select.enabled = false;
            }

            Unit.OnFindUnitsCellEvent += FindUnitsCurrentCell;
        }

        private void OnDestroy()
        {
            Unit.OnFindUnitsCellEvent -= FindUnitsCurrentCell;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            focus.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            focus.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickEvent?.Invoke(this);
        }

        private void FindUnitsCurrentCell(Cell cell)
        {
            if (cell != null && cell == this)
            {
                UnitsCell = cell;
            }
        }

        private bool ValidateDependencies()
        {
            if (focus == null)
                throw new NullReferenceException("[Cell] Serializable field _focus of type MeshRenderer is null!");

            if (select == null)
                throw new NullReferenceException("[Cell] Serializable field _select of type MeshRenderer is null!");

            return true;
        }

        public void SetSelect(Material mat)
        {
            select.material = mat;
            select.enabled = true;
        }

        public void ResetSelect()
        {
            select.enabled = false;
        }
    }
}