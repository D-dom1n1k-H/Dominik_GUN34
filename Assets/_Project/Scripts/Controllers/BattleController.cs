using System;
using Necro.AutoGen.Controls;
using Necro.Config.DefaultSettings;
using Necro.Extra.GameStatus;
using Necro.Interfaces.WorldSpaceCanvasController;
using Necro.World.Battlefield;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Necro.GamePlay.Controllers
{
    public class BattleController : MonoBehaviour
    {
        private Controls _controls; //injected
        private Battlefield _battlefield; //injected
        private GameStatus _gameStatus; //injected

        private WorldSpaceCanvasController _worldSpaceCanvasController; //injected

        private Sprite[] _avatarImages;
        [SerializeField]
        private Image whitePlayerAvatarImage;
        [SerializeField]
        private Image blackPlayerAvatarImage;


        private GameStatus[] _lastGameStatus = new GameStatus[1];

        public event Action<GameObject> OnGameStatusModeChangedEvent;
        public event Action<bool> OnPlayerMovementConfirmedEvent;

        private void Awake()
        {
            ValidateDependencies();
        }

        private void Start()
        {
            whitePlayerAvatarImage.sprite = _worldSpaceCanvasController.GetRandomAvatarSprite();
            blackPlayerAvatarImage.sprite = _worldSpaceCanvasController.GetRandomAvatarSprite();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();

            _controls.Player.Cancel.performed += OnPlayerCancel_performed;
            _controls.Player.Confirm.performed += OnPlayerConfirm_performed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.Cancel.performed -= OnPlayerCancel_performed;
            _controls.Player.Confirm.performed -= OnPlayerConfirm_performed;
        }

        private void OnPlayerConfirm_performed(InputAction.CallbackContext obj)
        {
            OnPlayerMovementConfirmedEvent?.Invoke(true);
            Debug.Log("<b>[BattleController]</b> OnMovePreformedEvent was invoked");
        }

        private void OnPlayerCancel_performed(InputAction.CallbackContext obj)
        {
            _battlefield.HidePossibleMoves();
            SetGameStatusModeToLastOne(gameObject);
        }

        #region Public API

        public void UnblockPlayerInput()
        {
            _controls.Enable();
            _battlefield.IsUnitMoving(false);
        }

        public void BlockPlayerInput()
        {
            _controls.Disable();
            _battlefield.IsUnitMoving(true);
        }

        public void SetGameStatusModeToLock(GameObject sender)
        {
            if (_lastGameStatus[0] != GameStatus.Lock)
            {
                _gameStatus = GameStatus.Lock;
                OnGameStatusModeChangedEvent?.Invoke(sender);
                _lastGameStatus[0] = _gameStatus;

                Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                          + sender.name + " to 'Lock'</color>");
            }
        }

        public void SetGameStatusModeToSelect(GameObject sender)
        {
            if (_lastGameStatus[0] != GameStatus.Select)
            {
                _gameStatus = GameStatus.Select;
                OnGameStatusModeChangedEvent?.Invoke(sender);
                _lastGameStatus[0] = _gameStatus;

                Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                          + sender.name + " to 'Select'</color>");
            }
        }

        public void SetGameStatusModeToMove(GameObject sender)
        {
            if (_lastGameStatus[0] != GameStatus.Move)
            {
                _gameStatus = GameStatus.Move;
                OnGameStatusModeChangedEvent?.Invoke(sender);
                _lastGameStatus[0] = _gameStatus;

                Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                          + sender.name + " to 'Move'</color>");
            }
        }

        public void SetGameStatusModeToAttack(GameObject sender)
        {
            if (_lastGameStatus[0] != GameStatus.Attack)
            {
                _gameStatus = GameStatus.Attack;
                OnGameStatusModeChangedEvent?.Invoke(sender);
                _lastGameStatus[0] = _gameStatus;

                Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                          + sender.name + " to 'Attack'</color>");
            }
        }

        public void SetGameStatusModeToConfirmMove(GameObject sender)
        {
            if (_lastGameStatus[0] != GameStatus.ConfirmMove)
            {
                _gameStatus = GameStatus.ConfirmMove;
                OnGameStatusModeChangedEvent?.Invoke(sender);
                _lastGameStatus[0] = _gameStatus;

                Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                          + sender.name + " to 'ConfirmMove'</color>");
            }
        }

        public void SetGameStatusModeToConfirmAttack(GameObject sender)
        {
            if (_lastGameStatus[0] != GameStatus.ConfirmMove)
            {
                _gameStatus = GameStatus.ConfirmAttack;
                OnGameStatusModeChangedEvent?.Invoke(sender);
                _lastGameStatus[0] = _gameStatus;

                Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                          + sender.name + " to 'ConfirmAttack'</color>");
            }
        }

        public void SetGameStatusModeToLastOne(GameObject sender)
        {
            _gameStatus = _lastGameStatus[0];
            OnGameStatusModeChangedEvent?.Invoke(sender);

            Debug.Log("<color=yellow><b>[BattleController]</b> GameStatus mode was changed from GameObject "
                      + sender.name + " to LastOne: " + _lastGameStatus[0] + "</color>");
        }

        #endregion

        #region Initialization

        [Inject]
        private void Construct(Controls controls, GameStatus gameStatus, Battlefield battlefield,
            WorldSpaceCanvasController worldSpaceCanvasController)
        {
            _controls = controls;
            _gameStatus = gameStatus;
            _battlefield = battlefield;
            _worldSpaceCanvasController = worldSpaceCanvasController;
        }

        private void ValidateDependencies()
        {
            if (_controls == null)
                throw new NullReferenceException("<b>[BattleController]</b> _controls could not be injected!");

            if (_battlefield == null)
                throw new NullReferenceException("<b>[BattleController]</b> _battlefield could not be injected!");

            if (whitePlayerAvatarImage == null)
                throw new NullReferenceException("<b>[BattleController]</b> whitePlayerAvatarImage is null!");

            if (blackPlayerAvatarImage == null)
                throw new NullReferenceException("<b>[BattleController]</b> blackPlayerAvatarImage is null!");

            if (_worldSpaceCanvasController == null)
                throw new NullReferenceException(
                    "<b>[BattleController]</b> _worldSpaceCanvasController could not be injected!");
        }

        #endregion

        /*
         * Class BattleController should be used to handle the players input, and it gives API to change enum GameStatus
         */
    }
}