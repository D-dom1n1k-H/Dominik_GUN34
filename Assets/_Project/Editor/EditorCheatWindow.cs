using Necro.AutoGen.EditorControls;
using Necro.GamePlay.Controllers.PlayerController;
using Necro.World.Battlefield;
using Necro.World.Board.Cell;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Necro.Editor.EditorWindow.EditorCheatWindow
{
    public class EditorCheatWindow : UnityEditor.EditorWindow
    {
        private EditorControls _editorControls;
        private PlayerController _playerController;
        private Battlefield _battleField;

        private Cell _currentCell;

        private const string UxmlPath = "Assets/_Project/Editor/CheatWindowText.uxml";

        [MenuItem("Netologia/Windows/EditorCheatWindow")]
        public static void ShowWindow() => GetWindow<EditorCheatWindow>();

        private void OnEnable()
        {
            _editorControls = new EditorControls();

            VisualizeWindow();

            ValidateDependencies();

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            _editorControls.Disable();
            _editorControls.Cheats.NextTurn.performed -= CheatsNextTurn_performed;
            _editorControls.Cheats.Kill.performed -= CheatsKill_performed;
        }

        private void CheatsNextTurn_performed(InputAction.CallbackContext obj)
        {
            if (_playerController == null)
            {
                Debug.LogWarning(
                    $"{nameof(EditorCheatWindow)}: _playerController is null and Editor 'NextTurn' cheat can not work!");
            }
            else
            {
                _playerController.ChangeCurrentTrainToOpositeOne();
            }
        }

        private void CheatsKill_performed(InputAction.CallbackContext obj)
        {
            if (_battleField == null)
            {
                Debug.LogWarning(
                    $"{nameof(EditorCheatWindow)}: _battleField is null and Editor 'Kill' cheat can not work!");
            }
            else
            {
                try
                {
                    _currentCell = _battleField.GetCurrentCell();
                    _currentCell.GetCurrentUnit().DestroyUnit();
                }
                catch
                {
                    Debug.LogWarning($"{nameof(EditorCheatWindow)}: Unit is not selected!");
                }
            }
        }

        private void VisualizeWindow()
        {
            rootVisualElement.Clear();

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);

            if (visualTree != null)
            {
                visualTree.CloneTree(rootVisualElement);
            }
            else
            {
                rootVisualElement.Add(new Label("UXML not found at: " + UxmlPath));
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                _playerController = Object.FindObjectOfType<PlayerController>();
                _battleField = Object.FindObjectOfType<Battlefield>();

                _editorControls.Enable();
                _editorControls.Cheats.NextTurn.performed += CheatsNextTurn_performed;
                _editorControls.Cheats.Kill.performed += CheatsKill_performed;
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _editorControls.Disable();
                _editorControls.Cheats.NextTurn.performed -= CheatsNextTurn_performed;
                _editorControls.Cheats.Kill.performed -= CheatsKill_performed;
            }
        }
        
        private void ValidateDependencies()
        {
            if (_playerController == null)
                Debug.LogError($"{nameof(EditorCheatWindow)}: _playerController is null");

            if (_battleField == null)
                Debug.LogError($"{nameof(EditorCheatWindow)}: _battleField is null");
        }
    }
}