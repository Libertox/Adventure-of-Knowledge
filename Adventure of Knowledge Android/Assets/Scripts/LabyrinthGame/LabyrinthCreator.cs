
using System;
using System.Collections.Generic;
using UnityEngine;


namespace AdventureOfKnowledge.LabyrinthGame
{
    public class LabyrinthCreator:MonoBehaviour
    {
        private List<PathNode> winningPath;

        [SerializeField] private Transform fieldCenter;

        private readonly int startPathPositionX = 0;
        private readonly int startPathPositionY = 0;

        private int endPathPositionX;
        private int endPathPositionY;

        private readonly float spaceBetweenElement = 113;

        private DifficultyLabyrinthGameSettings levelSettings;

        public PipeTile[][] PipeTilesTable { get;private set; }


        public void SetGameSettings(DifficultyLabyrinthGameSettings gameSettings) 
        {
            levelSettings = gameSettings;
            PipeTilesTable = new PipeTile[levelSettings.Width][];

            for(int i = 0; i < levelSettings.Width; i++)
            {
                PipeTilesTable[i] = new PipeTile[levelSettings.Height];
            }
        } 

        public void DrawField()
        {
            CreateWinnigPath();

            float startPositionX = fieldCenter.position.x - levelSettings.Width * 0.5f * spaceBetweenElement;

            float startPositionY = fieldCenter.position.y + levelSettings.Height * 0.5f * spaceBetweenElement;

            InitializeFirstPipeElement(startPositionX, startPositionY);

            SpawnPathPipeElement(startPositionX, startPositionY);

            InitializeEndPiprElement(startPositionX, startPositionY);

            SpawnOtherPipeElement(startPositionX, startPositionY);
    
        }

        private void CreateWinnigPath()
        {
            int width = levelSettings.Width;
            int height = levelSettings.Height;

            Pathfinding pathfinding = new Pathfinding(width, height);

            RandomizeEndPosition();

            int firstBelongPositonXToPath = UnityEngine.Random.Range(0, width * 2 / 5);
            int firstBelongPositonYToPath = UnityEngine.Random.Range(height / 2, height);

            pathfinding.FindPath(startPathPositionX, startPathPositionY, firstBelongPositonXToPath, firstBelongPositonYToPath);

            int secondBelongPositonXToPath = UnityEngine.Random.Range(width / 2, width * 3 / 5);
            int secondBelongPositonYToPath = UnityEngine.Random.Range(0, height / 2);

            pathfinding.FindPath(firstBelongPositonXToPath, firstBelongPositonYToPath, secondBelongPositonXToPath, secondBelongPositonYToPath);

            winningPath = pathfinding.FindPath(secondBelongPositonXToPath, secondBelongPositonYToPath, endPathPositionX, endPathPositionY);

            if (winningPath == null)
            {
                Pathfinding newPathFinding = new Pathfinding(width, height);
                newPathFinding.FindPath(startPathPositionX, startPathPositionY, firstBelongPositonXToPath, firstBelongPositonYToPath);
                winningPath = newPathFinding.FindPath(firstBelongPositonXToPath, firstBelongPositonYToPath, endPathPositionX, endPathPositionY);
            }
        }

        private void RandomizeEndPosition()
        {
            int choose = UnityEngine.Random.Range(0, 2);
            if (choose == 0)
            {
                endPathPositionX = UnityEngine.Random.Range(levelSettings.Width * 4 / 5, levelSettings.Width);
                endPathPositionY = levelSettings.Height - 1;
            }
            else
            {
                endPathPositionX = levelSettings.Width - 1;
                endPathPositionY = UnityEngine.Random.Range(levelSettings.Height / 2, levelSettings.Height);
            }
        }

        private void InitializeEndPiprElement(float startPositionX, float startPositionY)
        {
            RectTransform endPipe = Instantiate(levelSettings.endPipeTile, fieldCenter);
            endPipe.anchoredPosition = new Vector2(startPositionX + spaceBetweenElement * endPathPositionX, startPositionY - spaceBetweenElement * endPathPositionY);
            PipeTile pipeTile =  endPipe.GetComponent<PipeTile>();
            pipeTile.Initialize(endPathPositionX, endPathPositionY);

            if (winningPath[winningPath.Count - 2].positionY == endPathPositionY)
                pipeTile.Rotate();

            PipeTilesTable[endPathPositionX][endPathPositionY] = pipeTile; 
        }

        private void SpawnPathPipeElement(float startPositionX, float startPositionY)
        {
            for (int i = 1; i < winningPath.Count - 1; i++)
            {
                RectTransform rectTransform = null;
                if (winningPath[i - 1].positionX == winningPath[i + 1].positionX || winningPath[i - 1].positionY == winningPath[i + 1].positionY)
                   rectTransform  = Instantiate(levelSettings.GetPipeByType(PipeTile.PipeType.Straight), fieldCenter);
                else
                   rectTransform = Instantiate(levelSettings.GetPipeByType(PipeTile.PipeType.Angle), fieldCenter);
                    

                rectTransform.anchoredPosition = new Vector2(startPositionX + spaceBetweenElement * winningPath[i].positionX, startPositionY - spaceBetweenElement * winningPath[i].positionY);
                PipeTile pipeTile = rectTransform.GetComponent<PipeTile>();
                pipeTile.Initialize(winningPath[i].positionX, winningPath[i].positionY);
                PipeTilesTable[winningPath[i].positionX][winningPath[i].positionY] = pipeTile;
            }
        }

        private void InitializeFirstPipeElement(float startPositionX, float startPositionY)
        {
            RectTransform startpipe = Instantiate(levelSettings.startPipeTile, fieldCenter);
            startpipe.anchoredPosition = new Vector2(startPositionX, startPositionY);
            PipeTile pipeTile = startpipe.GetComponent<PipeTile>();
            pipeTile.Initialize(0, 0);

            if (winningPath[1].positionY == startPathPositionY)
                pipeTile.Rotate();

            PipeTilesTable[0][0] = pipeTile;
        }

        private void SpawnOtherPipeElement(float startPositionX, float startPositionY)
        {
            for (int i = 0; i < levelSettings.Height; i++)
            {
                float posX = startPositionX;
                for (int j = 0; j < levelSettings.Width; j++)
                {
                    if (!CheckPathConatinPoint(j, i))
                    {
                        RectTransform rectTransform = Instantiate(levelSettings.GetRandomPipePrefab(), fieldCenter);
                        rectTransform.anchoredPosition = new Vector2(posX, startPositionY);
                        PipeTile pipeTile = rectTransform.GetComponent<PipeTile>();
                        pipeTile.Initialize(j, i);
                        PipeTilesTable[j][i] = pipeTile;
                    }

                    posX += spaceBetweenElement;

                }
                startPositionY -= spaceBetweenElement;
            }
        }

        private bool CheckPathConatinPoint(int x, int y)
        {
            foreach (PathNode path in winningPath)
            {
                if (path.positionX == x && path.positionY == y)
                    return true;
            }
            return false;
        }

        public PipeTile GetEndPipeTile() => PipeTilesTable[endPathPositionX][endPathPositionY];
    }
}
