
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.LabyrinthGame
{
    [CreateAssetMenu(fileName = "LabyrinthGameSettings", menuName = "ScriptableObject/LabyrinthGameSettings")]
    public class LabyrinthGameSettingsSO:ScriptableObject
    {
        [SerializeField] private List<DifficultyLabyrinthGameSettings> difficultyLevelSettingsList;

        public DifficultyLabyrinthGameSettings GetDifficultyLevelSettings(DifficultyLevel difficultyLevel)
        {
            foreach (var level in difficultyLevelSettingsList)
                if (level.level == difficultyLevel) return level;

            return default;
        }

    }

    [Serializable]
    public class DifficultyLabyrinthGameSettings : DifficultyLevelSettings
    {
        public int Width;
        public int Height;
        public RectTransform startPipeTile;
        public RectTransform endPipeTile;
        [SerializeField] private List<RectTransform> pipePrefabList;

        public RectTransform GetRandomPipePrefab() => pipePrefabList[UnityEngine.Random.Range(0,pipePrefabList.Count)];

        public RectTransform GetPipeByType(PipeTile.PipeType type)
        {
            int randomIndexOfList = UnityEngine.Random.Range(0, pipePrefabList.Count);
            for (int i = 0 ; i < pipePrefabList.Count; i++)
            {
                if (pipePrefabList[randomIndexOfList % pipePrefabList.Count].GetComponent<PipeTile>().HasPipeType(type))
                    return pipePrefabList[randomIndexOfList % pipePrefabList.Count];

                randomIndexOfList++;
            }
          

            return default;
        }

    }
}
