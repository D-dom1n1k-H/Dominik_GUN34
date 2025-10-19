using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Necro.World.Board.Cell
{
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private MeshRenderer focus;
        [SerializeField]
        private MeshRenderer select;

        public event Action<Cell> OnPointerClickEvent;

        private void Awake()
        {
            ValidateDependencies();

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

        public void SetSelect(Material material)
        {
            select.enabled = true;
            select.material = material;
        }

        public void ResetSelect()
        {
            select.enabled = false;
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