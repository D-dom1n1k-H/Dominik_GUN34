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

        public Team Team { get; private set; }

        [SerializeField, Space(10f)]
        private Material whiteCheckerMaterial;
        [SerializeField]
        private Material blackCheckerMaterial;

        private Cell _currentCell;

        private MeshRenderer _meshRenderer;

        // events for cells
        public event Action<Cell> OnUnitEnter;
        public event Action<Cell> OnUnitExit;
        public event Action<Cell> OnUnitClicked;

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

        public void Move(Vector3 goalPosition, float speed = 3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, speed * Time.deltaTime);
        }
        public void GetCurrentCell(Cell cell) // метод является public для Cell
        {
            _currentCell = cell;
        }

        [Inject]
        private void Construct(BattleController battleController)
        {
            _battleController = battleController;
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
    }
}