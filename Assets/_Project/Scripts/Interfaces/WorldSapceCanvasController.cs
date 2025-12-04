using System;
using System.Collections.Generic;
using Necro.Config.DefaultSettings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Necro.Interfaces.WorldSpaceCanvasController
{
    public class WorldSpaceCanvasController : MonoBehaviour
    {
        private GameSettings _gameSettings; //injected

        [SerializeField]
        private Image arrowImage; // Это не храниться в GameSettings, потому-что Unity не давала проставить ссылку туда 
        private Sprite _arrowSprite;

        private Sprite[] _avatarSprites;
        private List<Sprite> _availableSprites;

        private void Awake()
        {
            SetupFromSettings();

            for (int i = 0; i < _availableSprites.Count; i++)
            {
                var rnd = UnityEngine.Random.Range(i, _availableSprites.Count);
                (_availableSprites[i], _availableSprites[rnd]) =
                    (_availableSprites[rnd], _availableSprites[i]); // (a, b) = (b, a);
            }

            ValidateDependencies();
        }

        #region Public API

        public Sprite GetRandomAvatarSprite()
        {
            if (_availableSprites.Count == 0)
            {
                Debug.LogWarning("<b>[WorldSpaceCanvasController]</b> No avatar sprites available!");
                return null;
            }

            var sprite = _availableSprites[0];
            _availableSprites.RemoveAt(0);
            return sprite;
        }

        public void TurnArrowImageToOpositeSide()
        {
            if (arrowImage.transform.rotation.eulerAngles.y >= 180)
            {
                arrowImage.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                arrowImage.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        #endregion

        private void SetupFromSettings()
        {
            _arrowSprite = _gameSettings.uiSettings.arrowSprite;
            arrowImage.sprite = _arrowSprite;

            _avatarSprites = _gameSettings.uiSettings.avatarSprites;

            _availableSprites = new List<Sprite>(_avatarSprites);
        }

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        private void ValidateDependencies()
        {
            if (_gameSettings == null)
                throw new ArgumentException("[WorldSpaceCanvasController] _gameSettings was not assigned or injected.",
                    nameof(_gameSettings));

            if (arrowImage == null)
                throw new ArgumentException(
                    "[WorldSpaceCanvasController] arrowImage was not assigned in the Inspector.", nameof(arrowImage));
        }
    }
}