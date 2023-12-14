
using System;
using System.Collections.Generic;


namespace AdventureOfKnowledge
{
    [Serializable]
    public class DifficultyLevelSettings
    {
        public DifficultyLevel level;
        public float PointLossTime;
        public int PointLossMove;
        public int MaxPointToEarn;
        public int MinPointToEarn;


    }
}
