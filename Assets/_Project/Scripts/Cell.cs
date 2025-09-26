using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Unit = Korven.Units.Unit;

namespace Korven.Level.Board
{
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private MeshRenderer focus;
        [SerializeField] private MeshRenderer select;

        private Cell UnitsCell;

        public event Action<Cell> OnPointerClickEvent;

        private void Awake()
        {
            if (focus == null)
            {
                Debug.LogError("[Cell] Serializable field _focus of type MeshRenderer is null]");
                return;
            }
            focus.enabled = false;

            if (select == null)
            {
                Debug.LogError("[Cell] Serializable field _select of type MeshRenderer is null");
                return;
            }
            select.enabled = false;

            Unit.OnFindUnitsCellEvent += FindUnitsCurrentCell;
        }

        private void OnDestroy()
        {
            Unit.OnFindUnitsCellEvent -= FindUnitsCurrentCell;
        }

        private void FindUnitsCurrentCell(Cell cell)
        {
            if (cell != null && cell == this)
            {
                UnitsCell = cell;
            }
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
    }
}