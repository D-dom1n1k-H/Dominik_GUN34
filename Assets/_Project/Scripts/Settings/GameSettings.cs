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
            [Space(6f), Tooltip("White cells color")]
            public Material whiteCellMaterial;
            [Tooltip("Black cells color")]
            public Material blackCellMaterial;
        }

        [Serializable]
        public struct UnitSettings
        {
            [Space(6f), Tooltip("White checkers color")]
            public Material whiteUnitMaterial;
            [Tooltip("Black checkers color")]
            public Material blackUnitMaterial;
            [Space(6f), Tooltip("Lady's crown model")]
            public Mesh crownMesh;
            [Tooltip("Lady's crown color")]
            public Material crownMaterial;
            [Range(0.5f, 800f)]
            public float speed;
        }

        [Serializable]
        public struct UiSettings
        {
            [Space(6f), Tooltip("Put in this array sprites and random is going to chose some of them")]
            public Sprite[] avatarSprites;
            [Tooltip("Sprite that is an arrow that show's whose turn is to play")]
            public Sprite arrowSprite;
        }
    }
}