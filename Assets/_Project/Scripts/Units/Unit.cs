using System;
using Necro.Extra.Enums.Team;
using Necro.Extra.Enums.UnitrType;
using Necro.GamePlay.Controllers;
using Necro.World.Board.Cell;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Necro.GamePlay.Units
{
    public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private BattleController _battleController; //injected
        public UnitType UnitType { get; private set; } = UnitType.Default; // injected

        [SerializeField, Space(10f)]
        private Material whiteCheckerMaterial;
        [SerializeField]
        private Material blackCheckerMaterial;
        [SerializeField, Space(10f)]
        private GameObject crown;
        [SerializeField]
        private float movementSpeed = 3f;

        private Cell _currentCell;
        private Cell _targetCell;

        private MeshRenderer _meshRenderer;

        private bool _isMoving = false;

        public Team Team { get; private set; }

        // events for cells
        public event Action<Cell> OnUnitEnter;
        public event Action<Cell> OnUnitExit;
        public event Action<Cell> OnUnitClicked;

        public event Action<Unit> OnMoveCompleted;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();

            ValidateDependencies();
        }

        private void Start()
        {
            if (_meshRenderer.material.name.StartsWith(whiteCheckerMaterial.name))
            {
                Team = Team.White;
            }
            else if (_meshRenderer.material.name.StartsWith(blackCheckerMaterial.name))
            {
                Team = Team.Black;
            }
            else
            {
                Debug.LogError($"<b>[Unit]</b> unit has incorrect material: {_meshRenderer.material.name}");
            }

            crown.SetActive(false);
        }

        private void Update()
        {
            if (_isMoving)
            {
                Move();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnUnitClicked?.Invoke(_currentCell);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnUnitEnter?.Invoke(_currentCell);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnUnitExit?.Invoke(_currentCell);
        }

        #region Public API

        public Cell GetCurrentCell() // used in Cell.cs
        {
            return _currentCell;
        }

        public void SetCurrentCell(Cell cell)
        {
            _currentCell = cell;
        }

        public void MoveUnitToCell(Cell targetCell)
        {
            _targetCell = targetCell;
            _isMoving = true;
        }

        #endregion

        private void Move()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetCell.transform.position + new Vector3(0, 1.215f, 0),
                movementSpeed * Time.deltaTime);

            if (transform.position == _targetCell.transform.position + new Vector3(0, 1.215f, 0))
            {
                _isMoving = false;
                if (OnMoveCompleted != null && _currentCell != null)
                {
                    OnMoveCompleted.Invoke(this);
                }
            }
        }

        public void SetUnitTypeToLady()
        {
            UnitType = UnitType.Lady;
            crown.SetActive(true);
        }

        [Inject]
        private void Construct(BattleController battleController, UnitType unitType)
        {
            _battleController = battleController;
            UnitType = unitType;
        }

        private void ValidateDependencies()
        {
            if (whiteCheckerMaterial == null)
                throw new NullReferenceException("<b>[Unit]</b> whiteCheckerMaterial is null!");

            if (blackCheckerMaterial == null)
                throw new NullReferenceException("<b>[Unit]</b> blackCheckerMaterial is null!");

            if (_battleController == null)
                throw new NullReferenceException("<b>[Unit]</b> BattleController is could not be injected!");
        }
        /*
         * This class is used for checkers that are located on Battlefield, that is made with cells
         */
    }
}