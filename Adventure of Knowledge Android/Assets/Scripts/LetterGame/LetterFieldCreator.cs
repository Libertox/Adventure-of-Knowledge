using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AdventureOfKnowledge.LetterGame
{
    public class LetterFieldCreator:MonoBehaviour
    {
        [SerializeField] private LetterTile questionLetterTilePrefab;
        [SerializeField] private LetterTile resultLetterTilePrefab;
        [SerializeField] private LetterTile answerLetterTilePrefab;

        [SerializeField] private Transform middleOfFieldTransform;
        [SerializeField] private Transform answerPlaceTransform;

        private DifficultyLetterGameSettings difficultyLetterGameSettings;
        private readonly float spaceBetweenTile = 4.2f;

        private ObjectPool<LetterTile> questionLetterPool;
        private ObjectPool<LetterTile> resultLetterPool;
        private ObjectPool<LetterTile> answerLetterPool;

        private List<string> usedQuestionWord = new List<string>();

        private void Awake() => SetupObjectPool();
      
        private void SetupObjectPool()
        {
            questionLetterPool = new ObjectPool<LetterTile>(
                () =>
                {
                    LetterTile letterTile = Instantiate(questionLetterTilePrefab);
                    letterTile.SetObjectPool(questionLetterPool);
                    return letterTile;
                },
                (letterTile) => { letterTile.gameObject.SetActive(true); letterTile.SetNewTileType(TextTile.TileType.Question); },
                (letterTile) => letterTile.gameObject.SetActive(false));

            resultLetterPool = new ObjectPool<LetterTile>(
               () =>
               {
                   LetterTile letterTile = Instantiate(resultLetterTilePrefab);
                   letterTile.SetObjectPool(resultLetterPool);
                   return letterTile;
               },
               (letterTile) => { letterTile.gameObject.SetActive(true); letterTile.SetNewTileType(TextTile.TileType.Result); },
               (letterTile) => letterTile.gameObject.SetActive(false));

            answerLetterPool = new ObjectPool<LetterTile>(
                () =>
                {
                    LetterTile letterTile = Instantiate(answerLetterTilePrefab);
                    letterTile.SetObjectPool(answerLetterPool);
                    return letterTile;
                },
                (letterTile) => { letterTile.gameObject.SetActive(true); letterTile.SetNewTileType(TextTile.TileType.Answer); },
                (letterTile) => letterTile.gameObject.SetActive(false));
        }


        public void SetDifficultyLetterGameSettings(DifficultyLetterGameSettings gameSettings)
        {
            difficultyLetterGameSettings = gameSettings;
        }

        public int DrawField()
        {
            string questionWord = difficultyLetterGameSettings.GetRandomWord();

            while (usedQuestionWord.Contains(questionWord))
                questionWord = difficultyLetterGameSettings.GetRandomWord();

            usedQuestionWord.Add(questionWord);

            List<int> resultIndexList = CreateResultIndexList(questionWord);
            List<char> answerCharacter = CreateAnswerCharacterList(resultIndexList, questionWord);

            int answerCharacterCount = answerCharacter.Count;

            SpawnQuestionTile(questionWord, resultIndexList);

            SpawnAnswerTile(answerCharacter);

            return answerCharacterCount;

        }

        private float SpawnAnswerTile(List<char> answerCharacter)
        {
            List<int> answerTileIndex = CreateAnswerTileIndexList(answerCharacter.Count);
            float answerStartPositionX = answerPlaceTransform.position.x - (difficultyLetterGameSettings.AnswerAmount / 2) * spaceBetweenTile;

            if (difficultyLetterGameSettings.AnswerAmount % 2 == 0) answerStartPositionX += spaceBetweenTile / 2;

            for (int i = 0; i < difficultyLetterGameSettings.AnswerAmount; i++)
            {
                Vector3 spawnPosition = new Vector3(answerStartPositionX, answerPlaceTransform.position.y, answerPlaceTransform.position.z);
                LetterTile letterTile = answerLetterPool.Get();
                letterTile.transform.position = spawnPosition;
                letterTile.SetStartPosition(spawnPosition);


                if (answerTileIndex.Contains(i))
                {
                    int answerCharacterIndex = UnityEngine.Random.Range(0, answerCharacter.Count);
                    letterTile.SetAnswer(answerCharacter[answerCharacterIndex]);
                    answerCharacter.RemoveAt(answerCharacterIndex);
                }
                else
                    letterTile.SetAnswer((char)UnityEngine.Random.Range(65, 90));

                StartCoroutine(letterTile.GameTileVisual.AppearCoroutine());

                answerStartPositionX += spaceBetweenTile;
            }

            return answerStartPositionX;
        }

        private List<int> CreateAnswerTileIndexList(int AnswerNumber)
        {
            List<int> answerTileIndex = new List<int>();

            for (int i = 0; i < AnswerNumber; i++)
            {
                int tileIndex = UnityEngine.Random.Range(0, difficultyLetterGameSettings.AnswerAmount);
                while (answerTileIndex.Contains(tileIndex))
                {
                    tileIndex = UnityEngine.Random.Range(0, difficultyLetterGameSettings.AnswerAmount);
                }
                answerTileIndex.Add(tileIndex);

            }

            return answerTileIndex;
        }

        private void SpawnQuestionTile(string questionWord, List<int> resultIndexList)
        {
            float questionStartPositionX = middleOfFieldTransform.position.x - (questionWord.Length / 2) * spaceBetweenTile;
            if (questionWord.Length % 2 == 0) questionStartPositionX += spaceBetweenTile / 2;

            for (int i = 0; i < questionWord.Length; i++)
            {
                Vector3 spawnPosition = new Vector3(questionStartPositionX, middleOfFieldTransform.position.y, middleOfFieldTransform.position.z);
                LetterTile letterTile = resultIndexList.Contains(i) ? resultLetterPool.Get() : questionLetterPool.Get();

                letterTile.transform.position = spawnPosition;
                letterTile.SetStartPosition(spawnPosition);

                letterTile.SetAnswer(questionWord[i]);

                StartCoroutine(letterTile.GameTileVisual.AppearCoroutine());

                questionStartPositionX += spaceBetweenTile;
            }
        }

        private List<char> CreateAnswerCharacterList(List<int> resultIndexList, string questionWord)
        {
            List<char> result = new List<char>();

            for (int i = 0; i < questionWord.Length; i++)
            {
                if (resultIndexList.Contains(i))
                    result.Add(questionWord[i]);
            }

            return result;
        }

        private List<int> CreateResultIndexList(string questionWord)
        {
            List<int> answerCharacter = new List<int>();

            for (int i = 0; i < difficultyLetterGameSettings.MissingLetterNumber; i++)
            {
                int characterIndex = UnityEngine.Random.Range(0, questionWord.Length);
                while (answerCharacter.Contains(characterIndex))
                {
                    characterIndex = UnityEngine.Random.Range(0, questionWord.Length);
                }
                answerCharacter.Add(characterIndex);

                if (answerCharacter.Count == questionWord.Length) break;
            }

            return answerCharacter;
        }


    }
}
