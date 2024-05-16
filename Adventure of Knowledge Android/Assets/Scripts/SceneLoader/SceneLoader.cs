
using UnityEngine.SceneManagement;

namespace AdventureOfKnowledge
{
    public static class SceneLoader
    {
        public static void LoadScene(GameScene gameScene) => SceneManager.LoadScene(gameScene.ToString());
     
        public static void LoadTheSameScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        public static string GetActiveSceneName() => SceneManager.GetActiveScene().name;
    }

}
