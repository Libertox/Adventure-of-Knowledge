using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MathGame
{
    public class MathGameManager : GameManager
    {
        private const float NEW_STAGE_LOAD_TIME = 1f;

        [SerializeField] private MathGameSettingsSO mathGameSettingsSO;

        [SerializeField] private TextTileVisual firstNumberTile;
        [SerializeField] private TextTileVisual secondNumberTile;
        [SerializeField] private TextTileVisual arithmeticOperatorTile;

        [SerializeField] private List<MathTile> answersTile;

        private DifficultyMathGameSettings difficultyMathGameSettings;
        private int expressionResult;
        private int stage;

        protected override void Awake()
        {
            base.Awake();

            OnGameStarted += MemoryGameManager_OnGameStarted;
            OnCorrectAnswer += MathGameManager_OnCorrectAnswer;
        }

        private void MathGameManager_OnCorrectAnswer(object sender, System.EventArgs e) => IncreaseStage();  
       
        private void IncreaseStage()
        {
            stage++;
            if (stage >= difficultyMathGameSettings.maxArithmeticExpression)
                InvokeFinishGameEvent(difficultyMathGameSettings);
            else
                StartCoroutine(LoadNewStageCoroutine());
        }

        private IEnumerator LoadNewStageCoroutine()
        {
            yield return new WaitForSeconds(NEW_STAGE_LOAD_TIME);
            InitializeField();
            InvokeNewStageLoadEvent();
        }

        private void MemoryGameManager_OnGameStarted(object sender, OnGameStartedEventArgs e)
        {
            difficultyMathGameSettings = mathGameSettingsSO.GetDifficultyLevelSettings(e.difficultyLevel);
            InitializeField();
        }

        private void InitializeField()
        {
            string arithmeticOperator = GetRandomArithmeticOperator();
            int firstNumber = GetRandomNumber(arithmeticOperator);
            int secondNumber = GetRandomNumber(arithmeticOperator);

            if (arithmeticOperator == "/")
            {
                while (firstNumber % secondNumber != 0)
                    secondNumber = GetRandomNumber(arithmeticOperator);
            }

           
            firstNumberTile.UppdateText(firstNumber > secondNumber ? firstNumber.ToString() : secondNumber.ToString());
            secondNumberTile.UppdateText(firstNumber < secondNumber ? firstNumber.ToString() : secondNumber.ToString());

            arithmeticOperatorTile.UppdateText(arithmeticOperator);

            expressionResult = CalculateResultExpression(firstNumber, secondNumber, arithmeticOperator);
            List<int> usedCloseExpressionResult = GetCloseExpressionResult(arithmeticOperator, secondNumber);

            SetupAnswerTile(usedCloseExpressionResult);
        }

        private int CalculateResultExpression(int firstNumber, int secondNumber, string arithmeticOperator)
        {
            return arithmeticOperator switch
            {
                "+" => firstNumber + secondNumber,
                "-" => Mathf.Abs(firstNumber - secondNumber),
                "*" => firstNumber * secondNumber,
                "/" => firstNumber / secondNumber,
                _ => 0,
            };
        }

        private void SetupAnswerTile(List<int> usedCloseExpressionResult)
        {
            List<int> usedIndex = new List<int>();
       
            int randomAnswerIndex = Random.Range(0, answersTile.Count);
            usedIndex.Add(randomAnswerIndex);
            answersTile[randomAnswerIndex].SetAnswer(expressionResult);
   
            for (int i = 0; i < answersTile.Count - 1; i++)
            {
                randomAnswerIndex = Random.Range(0, answersTile.Count);

                while (usedIndex.Contains(randomAnswerIndex))
                    randomAnswerIndex = Random.Range(0, answersTile.Count);

                usedIndex.Add(randomAnswerIndex);

                answersTile[randomAnswerIndex].SetAnswer(usedCloseExpressionResult[i+1]);
            }
        }

        private List<int> GetCloseExpressionResult(string arithmeticOperator, int secondNumber)
        {
           
            List<int> usedCloseExpressionResults = new List<int> { expressionResult };
        
            for (int i = 0; i < answersTile.Count - 1; i++)
            {
                int closeExpressionResult;
                if (arithmeticOperator == "*")
                {
                    int multiplicationRange = 4;
                    closeExpressionResult = Random.Range(-multiplicationRange, multiplicationRange) * secondNumber + expressionResult;

                    while (closeExpressionResult < 0 || usedCloseExpressionResults.Contains(closeExpressionResult))
                        closeExpressionResult = Random.Range(-multiplicationRange, multiplicationRange) * secondNumber + expressionResult;               
                }
                else
                {
                    int closeRange = 5;
                    closeExpressionResult = Random.Range(-closeRange, closeRange) + expressionResult;

                    while (closeExpressionResult < 0 || usedCloseExpressionResults.Contains(closeExpressionResult))
                        closeExpressionResult = Random.Range(-closeRange, closeRange) + expressionResult;
                }
                usedCloseExpressionResults.Add(closeExpressionResult);
            }

            return usedCloseExpressionResults;
        }

        public bool CheckCorrectAnswer(int answer)
        { 
            IncreaseMoveNumber();
            if (answer == expressionResult) 
            {
                InvokeCorrectAnswerEvent();
                return true;
            }     
            InvokeWrongAnswerEvent();
            return false;
        }

        private int GetRandomNumber(string arithmeticOperator) => 
            Random.Range(1,difficultyMathGameSettings.GetNumberRangeFromArithmeticOperator(arithmeticOperator));

        private string GetRandomArithmeticOperator() => 
            difficultyMathGameSettings.arithmeticOperator[Random.Range(0, difficultyMathGameSettings.arithmeticOperator.Length)].arithmeticOperator;
    }
}
