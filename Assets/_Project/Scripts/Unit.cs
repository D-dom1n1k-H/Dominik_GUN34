using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Korven.Level.Board;
using Vector3 = UnityEngine.Vector3;

namespace Korven.Units
{
    public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        /* Warning!
         *  If Unity method OnTriggerEnter is not called, the code will not work!
         */
        public Cell cellToMove;     // Cell where Unit will move
        
        private Cell _currentCell;
        private Cell _nextCell;
        
        private float _progress;
        
        [SerializeField] 
        private float moveSpeed = 2f;
        private bool _isMoving = false;
        private bool _setNewPosition = false;
        
        private Vector3 _currPosition;
        private Vector3 _nextPosition;

        private List<Cell> _passedCells;

        public static Action<Cell> OnFindUnitsCellEvent;
        public static Action OnMoveEndCallback;

        private void Awake()
        {
            _passedCells = new List<Cell>();
        }

        private void FixedUpdate()
        {
            if (_isMoving == true)
            {
                Move(cellToMove);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Cell cell))
            {
                _currentCell = cell;
                _passedCells.Add(_currentCell);
                _passedCells[0] = _currentCell;
                _setNewPosition = true;
                _isMoving = true;
            }
        }

        private void Move(Cell cell)
        {
            if (_setNewPosition == true)
            {
                _nextCell = cell;

                _currPosition = _currentCell.transform.position + new Vector3(0f, 2.4f, 0f);
                _nextPosition = _nextCell.transform.position + new Vector3(0f, 2.4f, 0f);

                _progress = 0f;
                _setNewPosition = false;
            }

            if (_nextCell == null)
                return;

            _progress += Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(_currPosition, _nextPosition, _progress);

            if (_progress >= 1f)
            {
                _passedCells.Add(_nextCell);
                _isMoving = false;
                _currentCell = _nextCell;
                _nextCell = null;
                OnMoveEndCallback?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnFindUnitsCellEvent?.Invoke(_currentCell);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnFindUnitsCellEvent?.Invoke(_currentCell);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnFindUnitsCellEvent?.Invoke(_currentCell);
        }
    }
}