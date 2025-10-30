using System;
using Necro.AutoGen.Controls;
using Necro.Extra.GameStatus;
using Necro.World.Board.Cell;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Necro.GamePlay.Controllers
{
    public class BattleController : MonoBehaviour
    {
        private Controls _controls; //injected
        private GameStatus _gameStatus; //injected
        
        private GameStatus[] _lastGameStatus =  new GameStatus[1];

        public event Action<GameObject> OnGameStatusModeChangedEvent;
        public event Action OnMovePreformedEvent;

        private void Awake()
        {
            ValidateDependencies();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();

            _controls.Player.Cancel.performed += OnPlayerCancel_performed;
            _controls.Player.Confirm.performed += OnPlayerConfirm_performed;
            _controls.Player.Select.performed += OnPlayerSelect_performed;
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player.Cancel.performed -= OnPlayerCancel_performed;
            _controls.Player.Confirm.performed -= OnPlayerConfirm_performed;
            _controls.Player.Select.performed -= OnPlayerSelect_performed;
        }

        private void OnPlayerSelect_performed(InputAction.CallbackContext obj)
        {
        }

        private void OnPlayerConfirm_performed(InputAction.CallbackContext obj)
        {
            OnMovePreformedEvent?.Invoke();
            Debug.Log("<b>[BattleController]</b> OnMovePreformedEvent was invoked");
        }

        private void OnPlayerCancel_performed(InputAction.CallbackContext obj)
        {
            SetGameStatusModeToLastOne(gameObject);
        }

        public void SetGameStatusModeToLock(GameObject sender)
        {
            _gameStatus = GameStatus.Lock;
            OnGameStatusModeChangedEvent?.Invoke(sender);
            _lastGameStatus[0] = _gameStatus;
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name + 
                      " to 'Lock'");
        }

        public void SetGameStatusModeToSelect(GameObject sender)
        {
            _gameStatus = GameStatus.Select;
            OnGameStatusModeChangedEvent?.Invoke(sender);
            _lastGameStatus[0] = _gameStatus;
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name + 
                      " to 'Select'");
        }

        public void SetGameStatusModeToMove(GameObject sender)
        {
            _gameStatus = GameStatus.Move;
            OnGameStatusModeChangedEvent?.Invoke(sender);
            _lastGameStatus[0] = _gameStatus;
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name + 
                      " to 'Move'");
        }

        public void SetGameStatusModeToAttack(GameObject sender)
        {
            _gameStatus = GameStatus.Attack;
            OnGameStatusModeChangedEvent?.Invoke(sender);
            _lastGameStatus[0] = _gameStatus;
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name + 
                      " to 'Attack'");
        }

        public void SetGameStatusModeToConfirmMove(GameObject sender)
        {
            _gameStatus = GameStatus.ConfirmMove;
            OnGameStatusModeChangedEvent?.Invoke(sender);
            _lastGameStatus[0] = _gameStatus;
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name +
                      " to 'ConfirmMove'");
        }

        public void SetGameStatusModeToConfirmAttack(GameObject sender)
        {
            _gameStatus = GameStatus.ConfirmAttack;
            OnGameStatusModeChangedEvent?.Invoke(sender);
            _lastGameStatus[0] = _gameStatus;
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name +
                      " to 'ConfirmAttack'");
        }

        private void SetGameStatusModeToLastOne(GameObject sender)
        {
            _gameStatus = _lastGameStatus[0];
            OnGameStatusModeChangedEvent?.Invoke(sender);
            
            Debug.Log("<b>[BattleController]</b> GameStatus mode was changed from GameObject " + sender.name +
                      " to LastOne: " + _lastGameStatus[0]);
        }

        [Inject]
        private void Construct(Controls controls, GameStatus gameStatus)
        {
            _controls = controls;
            _gameStatus = gameStatus;
        }

        private void ValidateDependencies()
        {
            if (_controls == null)
                throw new NullReferenceException("[BattleController] _controls could not be injected!");
        }
    }
}