using Necro.World.Board.Cell;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Necro.GamePlay.Units
{
    public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Cell _currentCell = null;
        
        private void Awake()
        {
            ValidateDependencies();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }


        [Inject]
        private void Construct()
        {
        }

        private void ValidateDependencies()
        {
        }
    }
}