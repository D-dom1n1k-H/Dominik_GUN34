using System;
using Necro.Extra.Enums.Team;
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
        
        private Team _team;

        [SerializeField, Space(10f)]
        private Material whiteCheckerMaterial;
        [SerializeField]
        private Material blackCheckerMaterial;
        
        private Cell[]  _cells;
        private Cell _currentCell = null;

        private MeshRenderer _meshRenderer;
        
        // events for cells
        public event Action<Cell> OnUnitEnter;
        public event Action<Cell> OnUnitExit;
        public event Action<Cell> OnUnitClicked;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _cells = FindObjectsOfType<Cell>();
            ValidateDependencies();
        }

        private void Start()
        {
            if (_meshRenderer.material.name.StartsWith(whiteCheckerMaterial.name))
            {
                _team = Team.White;
            }
            else if (_meshRenderer.material.name.StartsWith(blackCheckerMaterial.name))
            {
                _team = Team.Black;
            }
            else
            {
                Debug.LogError($"[Unit] unit has incorrect material: {_meshRenderer.material.name}");
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

        private void OnEnable()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].OnShareCellEvent += GetCurrentCell;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].OnShareCellEvent -= GetCurrentCell;
            }
        }

        private void GetCurrentCell(Cell cell) { _currentCell = cell; }

        [Inject]
        private void Construct(BattleController battleController)
        {
            _battleController = battleController;
        }

        private void ValidateDependencies()
        {
            if (whiteCheckerMaterial == null)
                throw new NullReferenceException("[Unit] whiteCheckerMaterial is null!");

            if (blackCheckerMaterial == null)
                throw new NullReferenceException("[Unit] blackCheckerMaterial is null!");
            
            if(_battleController == null)
                throw new NullReferenceException("[Unit] BattleController is could not be injected!");
        }
    }
}