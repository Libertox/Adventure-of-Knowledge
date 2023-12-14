using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MemoryGame
{
    [CreateAssetMenu(fileName = "MemoryGameSettings",menuName = "ScriptableObject/MemoryGameSettings")]
    public class MemoryGameSettingsSO:ScriptableObject
    {
        [SerializeField] private List<DifficultyMemoryGameSettings> difficultyLevelSettingsList;

        [field: SerializeField] public int FieldRow { get; private set; } = 4;

        public DifficultyMemoryGameSettings GetDifficultyLevelSettings(DifficultyLevel difficultyLevel)
        {
            foreach (var level in difficultyLevelSettingsList)
                if(level.level == difficultyLevel) return level;

            return default;
        }
    }

    [Serializable]
    public class DifficultyMemoryGameSettings: DifficultyLevelSettings
    {
        public int FieldDimension;
    }
}
