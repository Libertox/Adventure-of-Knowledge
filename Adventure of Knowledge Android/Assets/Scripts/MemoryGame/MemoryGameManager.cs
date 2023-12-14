using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MemoryGame
{
    public class MemoryGameManager:GameManager
    {
        private const float ANSWER_CHECKED_TIME = 1.5f;
        private readonly float spaceBetweenTile = 3f;

        [SerializeField] private MemoryTile memoryTilePrefab;
        [SerializeField] private Transform fieldBeginTransform;

        [SerializeField] private MemoryTileReverseSO memoryTileReverseSO;
        [SerializeField] private MemoryGameSettingsSO memoryGameSettings;
      
        private MemoryTile firstMemoryTileSelceted;
        private MemoryTile secondMemoryTileSelected;

        private int completePair;
        private DifficultyMemoryGameSettings difficultyMemoryGameSettings;

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            gameTimer = GetComponent<GameTimer>();

            OnGameStarted += MemoryGameManager_OnGameStarted;
        }

        private void MemoryGameManager_OnGameStarted(object sender, OnGameStartedEventArgs e) 
        {
            difficultyMemoryGameSettings = memoryGameSettings.GetDifficultyLevelSettings(e.difficultyLevel);
            DrawField();
        } 
       
        private void DrawField()
        {
            int halfFieldRow = memoryGameSettings.FieldRow / 2;
            List<MemoryTile> memoryTileList = new List<MemoryTile>();

            Vector2 startPointOfField = new Vector2(fieldBeginTransform.position.x - difficultyMemoryGameSettings.FieldDimension / halfFieldRow * spaceBetweenTile,
                fieldBeginTransform.position.y + halfFieldRow * spaceBetweenTile);

            float fieldCoordinateY = startPointOfField.y;
            for (int i = 0; i < memoryGameSettings.FieldRow; i++)
            {
                float fieldCoordinateX = startPointOfField.x;
                for (int j = 0; j < difficultyMemoryGameSettings.FieldDimension; j++)
                {
                    MemoryTile memoryTile = Instantiate(memoryTilePrefab, new Vector3(fieldCoordinateX, fieldCoordinateY, fieldBeginTransform.position.z), Quaternion.identity);
                    memoryTileList.Add(memoryTile);
                    fieldCoordinateX += spaceBetweenTile;     
                }
                fieldCoordinateY -= spaceBetweenTile;
            }
            
            SetupMemoryTile(memoryTileList);
        }

        private void SetupMemoryTile(List<MemoryTile> memoryTileList)
        {

            List<int> usedReverseList = new List<int>();
            while (memoryTileList.Count != 0)
            {
                int reverseIndex = RandomReverseListIndex(usedReverseList);
                usedReverseList.Add(reverseIndex);

                InitializeMemoryTile(reverseIndex, memoryTileList);
            }
        }

        private int RandomReverseListIndex(List<int> usedReverseList)
        {
            int reverseIndex = UnityEngine.Random.Range(0, memoryTileReverseSO.MemoryTileReverseList.Count);

            while (usedReverseList.Contains(reverseIndex))
                reverseIndex = UnityEngine.Random.Range(0, memoryTileReverseSO.MemoryTileReverseList.Count);

            return reverseIndex;
        }

        private void InitializeMemoryTile(int reverseIndex, List<MemoryTile> memoryTileList)
        {
            int tilePair = 2;
            for (int i = 0; i < tilePair; i++)
            {
                int memoryTileIndex = UnityEngine.Random.Range(0, memoryTileList.Count);
                memoryTileList[memoryTileIndex].Initialize(reverseIndex, memoryTileReverseSO.MemoryTileReverseList[reverseIndex]);
                memoryTileList.RemoveAt(memoryTileIndex);
            }
        }

        public void SelectMemoryTile(MemoryTile memoryTile)
        {
            if (!firstMemoryTileSelceted)
                 firstMemoryTileSelceted = memoryTile;

            else if (!secondMemoryTileSelected && firstMemoryTileSelceted != memoryTile)
                 secondMemoryTileSelected = memoryTile;

            if(IsBothTileSelected())
                StartCoroutine(WaitForCheckTileCoroutine());
        }

        private IEnumerator WaitForCheckTileCoroutine()
        {      
            yield return new WaitForSeconds(ANSWER_CHECKED_TIME);
            IncreaseMoveNumber();

            if (IsTheSameTile())
            {
                InvokeCorrectAnswerEvent();
                firstMemoryTileSelceted.Dissolve();
                secondMemoryTileSelected.Dissolve();
               
                completePair++;

                if (CheckCompleteGame())
                    InvokeFinishGameEvent(difficultyMemoryGameSettings);
            }
            else
            {
                InvokeWrongAnswerEvent();
                firstMemoryTileSelceted.Cover();
                secondMemoryTileSelected.Cover();             
            }
            ResetSelectTile();

        }

        public bool IsBothTileSelected() => firstMemoryTileSelceted && secondMemoryTileSelected;

        private bool IsTheSameTile() => firstMemoryTileSelceted.Index == secondMemoryTileSelected.Index;

        private void ResetSelectTile()
        {
            firstMemoryTileSelceted = null;
            secondMemoryTileSelected = null;
        }

        private bool CheckCompleteGame() => completePair >= memoryGameSettings.FieldRow * difficultyMemoryGameSettings.FieldDimension * 0.5;
    }

}
