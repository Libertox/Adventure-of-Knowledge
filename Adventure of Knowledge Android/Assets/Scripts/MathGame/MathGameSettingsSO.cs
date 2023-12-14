using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MathGame
{
    [CreateAssetMenu(fileName = "MathGameSettings", menuName = "ScriptableObject/MathGameSettings")]
    public class MathGameSettingsSO: ScriptableObject
    {
        [SerializeField] private List<DifficultyMathGameSettings> difficultyLevelSettingsList;

        public DifficultyMathGameSettings GetDifficultyLevelSettings(DifficultyLevel difficultyLevel)
        {
            foreach (var level in difficultyLevelSettingsList)
                if (level.level == difficultyLevel) return level;

            return default;
        }

    }

    [Serializable]
    public class DifficultyMathGameSettings : DifficultyLevelSettings
    {
        [Serializable]
        public struct ArithmeticOperator
        {
            public int numberRange;
            public string arithmeticOperator;
        }

        public ArithmeticOperator[] arithmeticOperator;
        public int maxArithmeticExpression;

        public int GetNumberRangeFromArithmeticOperator(string Operator)
        {
            foreach(var item in arithmeticOperator)
            {
                if(item.arithmeticOperator == Operator) 
                    return item.numberRange;
            }
            return 0;
        }
    }
}
