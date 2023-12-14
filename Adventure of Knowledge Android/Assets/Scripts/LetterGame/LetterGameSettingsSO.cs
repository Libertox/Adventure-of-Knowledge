using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace AdventureOfKnowledge.LetterGame
{
    [CreateAssetMenu(fileName = "LetterGameSettings", menuName = "ScriptableObject/LetterGameSettings")]
    public class LetterGameSettingsSO:ScriptableObject
    {
        [SerializeField] private List<DifficultyLetterGameSettings> difficultyLevelSettingsList;

        public DifficultyLetterGameSettings GetDifficultyLevelSettings(DifficultyLevel difficultyLevel)
        {
            foreach (var level in difficultyLevelSettingsList)
                if (level.level == difficultyLevel) return level;

            return default;
        }

        [ContextMenu("ImportWord")]
        public void ImportWordFromFile() 
        {
            GetDifficultyLevelSettings(DifficultyLevel.Easy).ClearWords();
            GetDifficultyLevelSettings(DifficultyLevel.Medium).ClearWords();
            GetDifficultyLevelSettings(DifficultyLevel.Hard).ClearWords();

            using var file = File.OpenText("Assets/Resources/Words.csv");

            file.ReadLine();

            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                var lineElements = line.Split(';');
                if(lineElements[0] != "") GetDifficultyLevelSettings(DifficultyLevel.Easy).AddNewWord(lineElements[0]);
                if (lineElements[1] != "") GetDifficultyLevelSettings(DifficultyLevel.Medium).AddNewWord(lineElements[1]);
                if (lineElements[2] != "") GetDifficultyLevelSettings(DifficultyLevel.Hard).AddNewWord(lineElements[2]);
            }
        }

    }

    [Serializable]
    public class DifficultyLetterGameSettings:DifficultyLevelSettings
    {
        [SerializeField] private List<string> word;
        public int MissingLetterNumber;
        public int AnswerAmount;
        public int MaxStage;

        public string GetRandomWord() => word[UnityEngine.Random.Range(0,word.Count)];

        public void AddNewWord(string newWord) => word.Add(newWord);

        public void ClearWords() => word.Clear();
 
    }
}
