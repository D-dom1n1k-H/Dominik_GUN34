using System;
using UnityEngine;
using UnityEngine.UI;

namespace Necro.Config.DefaultSettings
{
    [CreateAssetMenu(
        fileName = "DefaultSettings",
        menuName = "Assets/_Project/Scripts/Settings/DefaultSettings",
        order = 52)]
    public class GameSettings : ScriptableObject
    {
        public GlobalSettings globalSettings;
        public UiSettings uiSettings;
        public CellSettings cellSettings;
        public UnitSettings unitSettings;

        [Serializable]
        public struct GlobalSettings
        {
        }

        [Serializable]
        public struct CellSettings
        {
        }

        [Serializable]
        public struct UnitSettings
        {
            [Range(0.5f, 800f), Space(10f)]
            public float speed;
        }

        [Serializable]
        public struct UiSettings
        {
            [Space(10f), Tooltip("Put in this array sprites and random is going to chose some of them")]
            public Sprite[] avatarSprites;
            public Sprite arrowSprite;
        }
    }
}