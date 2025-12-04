using System;
using UnityEngine;

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
            [Space(6f), Tooltip("White checkers material")]
            public Material whiteUnitMaterial;

            [Tooltip("Black checkers material")]
            public Material blackUnitMaterial;

            [Space(6f), Tooltip("Crown mesh for ladies")]
            public Mesh crownMesh;

            [Tooltip("Crown material")]
            public Material crownMaterial;

            [Range(0.5f, 800f)]
            public float speed;
        }

        [Serializable]
        public struct UiSettings
        {
            [Space(6f), Tooltip("Random avatar will be selected from this list")]
            public Sprite[] avatarSprites;

            [Tooltip("Arrow sprite that indicates the active player")]
            public Sprite arrowSprite;
        }
    }
}