using System;
using Necro.Config.DefaultSettings;
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
        private GameSettings _gameSettings; //injected
        public UnitType UnitType { get; private set; } = UnitType.Default; // injected

        private Material _whiteUnitMaterial;
        private Material _blackUnitMaterial;
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

            SetupFromSettings();

            ValidateDependencies();
        }

        private void Start()
        {
            if (this.CompareTag("WhiteUnit"))
            {
                _meshRenderer.material = _whiteUnitMaterial;
                Team = Team.White;
            }
            else if (CompareTag("BlackUnit"))
            {
                _meshRenderer.material = _blackUnitMaterial;
                Team = Team.Black;
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

        public void SetUnitTypeToLady()
        {
            UnitType = UnitType.Lady;
            crown.SetActive(true);
        }

        public void MoveUnitToCell(Cell targetCell)
        {
            _targetCell = targetCell;
            _isMoving = true;
        }

        public void DestroyUnit()
        {
            _isMoving = false;
            Destroy(gameObject);
        }

        #endregion

        #region Private API

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

        #endregion

        #region Initialization

        private void SetupFromSettings()
        {
            _whiteUnitMaterial = _gameSettings.unitSettings.whiteUnitMaterial;
            _blackUnitMaterial = _gameSettings.unitSettings.blackUnitMaterial;

            crown.GetComponent<MeshFilter>().mesh = _gameSettings.unitSettings.crownMesh;
            crown.GetComponent<MeshCollider>().sharedMesh = _gameSettings.unitSettings.crownMesh;
            crown.GetComponent<Renderer>().material = _gameSettings.unitSettings.crownMaterial;

            movementSpeed = _gameSettings.unitSettings.speed;
        }

        [Inject]
        private void Construct(BattleController battleController, UnitType unitType, GameSettings projectSettings)
        {
            _battleController = battleController;
            UnitType = unitType;
            _gameSettings = projectSettings;
        }

        private void ValidateDependencies()
        {
            if (_whiteUnitMaterial == null)
                throw new NullReferenceException("<b>[Unit]</b> whiteCheckerMaterial is null!");

            if (_blackUnitMaterial == null)
                throw new NullReferenceException("<b>[Unit]</b> blackCheckerMaterial is null!");

            if (_battleController == null)
                throw new NullReferenceException("<b>[Unit]</b> _battleController is could not be injected!");

            if (_gameSettings == null)
                throw new NullReferenceException("<b>[Unit]</b> _projectSettings is could not be injected!");
        }

        #endregion

        /*
         * This class is used for checkers that are located on Battlefield, that is made with cells
         */
    }
}