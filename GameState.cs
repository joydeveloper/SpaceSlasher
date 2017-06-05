using UnityEngine;

namespace Assets.Static
{
    /// <summary>
    /// Class contains properties between MainMenu and Game
    /// </summary>
    public class GameState : MonoBehaviour {
        public static bool isFirstStartup = true;

        private void Start () {
            DontDestroyOnLoad(gameObject);
        }
        public static void SetFirstStartupFalse()
        {
            isFirstStartup = false;
        }
    }
}
