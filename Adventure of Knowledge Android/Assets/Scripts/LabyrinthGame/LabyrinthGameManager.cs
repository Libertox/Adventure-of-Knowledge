
using System;
using UnityEngine;

namespace AdventureOfKnowledge.LabyrinthGame
{
    public class LabyrinthGameManager : GameManager
    {

        [SerializeField] private LabyrinthGameSettingsSO labyrinthGameSettingsSO;

        private LabyrinthCreator labyrinthCreator;
        private DifficultyLabyrinthGameSettings difficultyLabyrinthGameSettings;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            gameTimer = GetComponent<GameTimer>();
            labyrinthCreator = GetComponent<LabyrinthCreator>();

            OnGameStarted += LabyrinthGameManager_OnGameStarted;
           
        }

        private void LabyrinthGameManager_OnGameStarted(object sender, OnGameStartedEventArgs e)
        {
            difficultyLabyrinthGameSettings = labyrinthGameSettingsSO.GetDifficultyLevelSettings(e.difficultyLevel);
            labyrinthCreator.SetGameSettings(difficultyLabyrinthGameSettings);
            labyrinthCreator.DrawField();
            FlowWaterNeighbourPipes(0, 0);
        }


        public void CheckNeighborsPipeIsFill(int positionX, int positionY)
        {
            PipeTile selectedPiple = labyrinthCreator.PipeTilesTable[positionX][positionY];
            foreach (PipeTile.Direction direction in selectedPiple.Directions)
            {
                if (IsRightNeighbour(positionX, direction))
                    PassFillingToNeighbour(positionX + 1, positionY, selectedPiple, direction, PipeTile.Direction.Left);

                else if (IsUpNeighbour(positionY, direction))
                    PassFillingToNeighbour(positionX, positionY - 1, selectedPiple, direction, PipeTile.Direction.Down);

                else if (IsLeftNeighbour(positionX, direction))
                    PassFillingToNeighbour(positionX - 1, positionY, selectedPiple, direction, PipeTile.Direction.Right);

                else if (IsDownNeighbour(positionY, direction))
                    PassFillingToNeighbour(positionX, positionY + 1, selectedPiple, direction, PipeTile.Direction.Up);
            }
        }

        private bool IsDownNeighbour(int positionY, PipeTile.Direction direction) => direction == PipeTile.Direction.Down && positionY + 1 < difficultyLabyrinthGameSettings.Height;
       
        private static bool IsLeftNeighbour(int positionX, PipeTile.Direction direction) => direction == PipeTile.Direction.Left && positionX - 1 >= 0;
       
        private static bool IsUpNeighbour(int positionY, PipeTile.Direction direction) => direction == PipeTile.Direction.Up && positionY - 1 >= 0;
        
        private bool IsRightNeighbour(int positionX, PipeTile.Direction direction) => direction == PipeTile.Direction.Right && positionX + 1 < difficultyLabyrinthGameSettings.Width;

        private void PassFillingToNeighbour(int positionX, int positionY, PipeTile selectedPiple, PipeTile.Direction direction, PipeTile.Direction oppositeDirection)
        {
            foreach (var dire in labyrinthCreator.PipeTilesTable[positionX][positionY].Directions)
            {
                if (dire == oppositeDirection)
                {
                    if (labyrinthCreator.PipeTilesTable[positionX][positionY].IsFilled())
                        selectedPiple.Fill(direction,null);
                }
            }
        }

        public void FlowWaterNeighbourPipes(int positionX, int positionY)
        {
            PipeTile selectedPiple = labyrinthCreator.PipeTilesTable[positionX][positionY];
            foreach(var direction in selectedPiple.Directions)
            {
                if(IsRightNeighbour(positionX, direction))
                    FillNeighborsPipe(PipeTile.Direction.Left, positionX + 1, positionY);

                else if (IsUpNeighbour(positionY, direction))
                    FillNeighborsPipe(PipeTile.Direction.Down, positionX, positionY - 1);

                else if (IsLeftNeighbour(positionX, direction))
                    FillNeighborsPipe(PipeTile.Direction.Right,positionX-1, positionY);

                else if (IsDownNeighbour(positionY, direction))
                   FillNeighborsPipe(PipeTile.Direction.Up,positionX, positionY + 1);
            }
        }

        public void OutflowWaterNeighbourPipes(int positionX, int positionY)
        {
            PipeTile selectedPiple = labyrinthCreator.PipeTilesTable[positionX][positionY];

            if(selectedPiple.IsFilled())
                selectedPiple.StopFillCorotuine();
            
            foreach (PipeTile.Direction direction in Enum.GetValues(typeof(PipeTile.Direction)))
            {
                if (!selectedPiple.FillDirectionList.Contains(direction) && selectedPiple.FillDirectionList.Count > 0)
                {
                    if (IsRightNeighbour(positionX, direction))
                        UnFillNeighborsPipe(positionX + 1, positionY, PipeTile.Direction.Left);

                    else if (IsUpNeighbour(positionY, direction))
                        UnFillNeighborsPipe(positionX, positionY - 1, PipeTile.Direction.Down);

                    else if (IsLeftNeighbour(positionX, direction))
                        UnFillNeighborsPipe(positionX - 1, positionY, PipeTile.Direction.Right);

                    else if (IsDownNeighbour(positionY, direction))
                        UnFillNeighborsPipe(positionX, positionY + 1, PipeTile.Direction.Up);
                }
            }
        }

      
        private void UnFillNeighborsPipe(int positionX, int positionY, PipeTile.Direction direction)
        {
            if (labyrinthCreator.PipeTilesTable[positionX][positionY].FillDirectionList.Count > 0 && labyrinthCreator.PipeTilesTable[positionX][positionY].FillDirectionList.Contains(direction))
            {
                if (labyrinthCreator.PipeTilesTable[positionX][positionY].IsFilled())
                {
                    OutflowWaterNeighbourPipes(positionX, positionY);
                    labyrinthCreator.PipeTilesTable[positionX][positionY].Unfill(direction);
                    
                }         
            }
        }

        private void FillNeighborsPipe(PipeTile.Direction direction, int positionX, int positionY)
        {
            foreach (var dire in labyrinthCreator.PipeTilesTable[positionX][positionY].Directions)
            {
                if (dire == direction)
                {
                    if (!labyrinthCreator.PipeTilesTable[positionX][positionY].IsFilled())
                    {
                       labyrinthCreator.PipeTilesTable[positionX][positionY].Fill(direction, () => FlowWaterNeighbourPipes(positionX, positionY));

                        if (labyrinthCreator.PipeTilesTable[positionX][positionY] == labyrinthCreator.GetEndPipeTile())
                            InvokeFinishGameEvent(difficultyLabyrinthGameSettings);
                    }
                }
            }
        }
   
    }
}
