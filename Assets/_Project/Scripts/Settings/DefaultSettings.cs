using System;
using UnityEngine;

namespace Necro.Config.DefaultSettings
{
    [CreateAssetMenu(
        fileName = "DefaultSettings",
        menuName = "Assets/_Project/Scripts/Settings/DefaultSettings",
        order = 52)]
    public class DefaultSettings : ScriptableObject
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
            [Range(0.5f, 10f), Space(10f)]
            public float speed;
        }

        [Serializable]
        public struct UiSettings
        {
        }
    }
}