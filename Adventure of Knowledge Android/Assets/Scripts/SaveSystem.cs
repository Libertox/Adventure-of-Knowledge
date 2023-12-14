using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public static class SaveSystem
    {
        private const string SAVES_PATH = "/saves";

        private const string SAVE_BODY_PARTS_KEY = "/BodyParts";
        private const string SAVE_AVAILABLE_SKIN_ELEMENT_KEY = "/AvailableSkinElement";
        private const string PLAYER_PREFS_NAME = "PlayerName";
        private const string PLAYER_PREFS_BEST_SCORE = "BestScore";
        private const string PLAYER_PREFS_DIAMOND_AMOUNT = "DiamondAmount";
        private const string PLAYER_PREFS_SPIN_DATA_YEAR = "SpinDataYear";
        private const string PLAYER_PREFS_SPIN_DATA_MONTH = "SpinDataMonth";
        private const string PLAYER_PREFS_SPIN_DATA_DAY = "SpinDataDay";

        public static void SaveSpinDataDay(int day) => PlayerPrefs.SetInt(PLAYER_PREFS_SPIN_DATA_DAY, day);

        public static int LoadSpinDataDay() => PlayerPrefs.GetInt(PLAYER_PREFS_SPIN_DATA_DAY);

        public static void SaveSpinDataMonth(int month) => PlayerPrefs.SetInt(PLAYER_PREFS_SPIN_DATA_MONTH, month);

        public static int LoadSpinDataMonth() => PlayerPrefs.GetInt(PLAYER_PREFS_SPIN_DATA_MONTH);

        public static void SaveSpinDataYear(int year) => PlayerPrefs.SetInt(PLAYER_PREFS_SPIN_DATA_YEAR, year);

        public static int LoadSpinDataYear() => PlayerPrefs.GetInt(PLAYER_PREFS_SPIN_DATA_YEAR);

        public static string LoadPlayerName() => PlayerPrefs.GetString(PLAYER_PREFS_NAME);

        public static void SavePlayerName(string playerName) => PlayerPrefs.SetString(PLAYER_PREFS_NAME, playerName);

        public static int LoadTheBestLevelScore(DifficultyLevel difficultyLevel, string gameScene) => PlayerPrefs.GetInt(gameScene.ToString() + difficultyLevel.ToString() + PLAYER_PREFS_BEST_SCORE,int.MaxValue);

        public static void SaveTheBestLevelScore(int gameScore, DifficultyLevel difficultyLevel, string gameScene) => PlayerPrefs.SetInt(gameScene.ToString() + difficultyLevel.ToString() + PLAYER_PREFS_BEST_SCORE, gameScore);

        public static int LoadDiamondAmount() => PlayerPrefs.GetInt(PLAYER_PREFS_DIAMOND_AMOUNT,0);

        public static void SaveDiamondAmount(int diamondAmount) => PlayerPrefs.SetInt(PLAYER_PREFS_DIAMOND_AMOUNT, diamondAmount);

        public static List<AvailableMonsterSkinElementSaveData> LoadAvailableSkinElement() => Load<List<AvailableMonsterSkinElementSaveData>>(SAVE_AVAILABLE_SKIN_ELEMENT_KEY);

        public static void SaveAvailableSkinElement(List<AvailableMonsterSkinElementSaveData> monsterSkinElementSaveDatas) => Save(monsterSkinElementSaveDatas, SAVE_AVAILABLE_SKIN_ELEMENT_KEY);

        public static List<BodyPartSaveData> LoadMonsterVisual() => Load<List<BodyPartSaveData>>(SAVE_BODY_PARTS_KEY);

        public static void SaveMonsterVisual(List<BodyPartSaveData> bodyPartSaveDatas) => Save(bodyPartSaveDatas, SAVE_BODY_PARTS_KEY);

        private static void Save<T>(T obj, string key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SAVES_PATH;
            Directory.CreateDirectory(path);

            FileStream file = new FileStream(path + key, FileMode.OpenOrCreate);

            binaryFormatter.Serialize(file, obj);

            file.Close();

        }

        private static T Load<T>(string key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SAVES_PATH;
            T obj = default;

            if (File.Exists(path + key)) 
            {
                FileStream file = new FileStream(path + key, FileMode.Open);

                obj = (T)binaryFormatter.Deserialize(file);

                file.Close();
            }

            return obj;
        }

    }
}
