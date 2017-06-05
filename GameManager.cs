using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Managers
{
    /// <summary>
    /// Top Manager Class:Singleton:store and manage active global data
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private const float PrePauseWaitTime = 1;
        private static GameManager _instance;
        private static LevelState _levelstate;
        private WorldManager _worldman;
        private ObjectManager _objectman;
        private PlayerManager _playerman;
        private AiManager _aiman;
        public IEnumerator Pausecouroutine;

        public bool IsPaused { get; set; }

        // Globals globals;
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Awake()
        {
            // globals = ScriptableObject.CreateInstance<Globals>();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Start()
        {
            _levelstate = new LevelState();
            Pausecouroutine = StopGame(PrePauseWaitTime); 
        }

        public void GamePause()
        {
            StartCoroutine(Pausecouroutine);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Update()
        {
            if (Input.GetButton("Cancel"))
            {
                _levelstate.SetState(LevelState.GameState.Mainmenu);
            }
            if (Input.GetButtonDown("Submit"))
            {
                IsPaused = true;
                _levelstate.SetState(LevelState.GameState.Paused);
            }
            if (Input.GetButtonDown("Restart"))
                _levelstate.SetState(LevelState.GameState.Restart);
        }
        public static LevelState GetLevelState()
        {
            return _levelstate;
        }
        public static GameManager GetInstance(GameObject worldman, GameObject objman, GameObject playerman, GameObject aiman)
        {
            if (!_instance)
            {
                GameObject gamemanager = new GameObject("GameManager");
                _instance = gamemanager.AddComponent<GameManager>();
                InstaniateManagers(worldman, objman, playerman,aiman);         
            }
            return _instance;
        }
        public static void InstaniateManagers(GameObject worldman, GameObject objman, GameObject playerman, GameObject aiman)
        {
            _instance._worldman = Instantiate(worldman).GetComponent<WorldManager>();
            _instance._objectman = Instantiate(objman).GetComponent<ObjectManager>();
            _instance._playerman = Instantiate(playerman).GetComponent<PlayerManager>();
            _instance._aiman = Instantiate(aiman).GetComponent<AiManager>();
        }
        public static PlayerManager GetPlayerManager()
        {
            return _instance._playerman;
        }
        public static WorldManager GetWorldManager()
        {
            return _instance._worldman;
        }
        public static ObjectManager GetObjectmanager()
        {
            return _instance._objectman;
        }
        public static AiManager GetAImanager()
        {
            return _instance._aiman;
        }

        private IEnumerator StopGame(float time)
        {
            yield return new WaitForSeconds(time);
            _levelstate.SetState(LevelState.GameState.Paused);
        }

/*
        private void OnApplicationFocus(bool hasFocus)
        {
            IsPaused = !hasFocus;
        }
*/

/*
        private void OnApplicationPause(bool pauseStatus)
        {
            IsPaused = pauseStatus;
        }
*/
    }
    /// <summary>
    /// Automat to change gamestates 
    /// </summary>
    public class LevelState
    {
        public enum GameState
        {
            Mainmenu,
            Begin,
            Restart,
            Game,
            Paused,
            End
        }
        protected GameState State;
        public LevelState()
        {
            SetState(GameState.Begin);
        }
        public void GetApplication()
        {
            switch (State)
            {
                case GameState.Begin:
                    Debug.Log("Level Start");
                    break;
                case GameState.Game:
                    Debug.Log("Game in process");
                    break;
                case GameState.End:
                    Debug.Log("Level end");
                    break;
            }
        }
        public void SetState(GameState gameState)
        {
            State = gameState;
            CheckLevelState();
        }
        public void CheckLevelState()
        {
            switch (State)
            {
                case GameState.Begin: GameManager.GetWorldManager().StartLevel(); break;
                case GameState.Game:
                {
                    break;
                }
                case GameState.Mainmenu: SceneManager.LoadScene("Main"); break;
                case GameState.Paused:
                {
                    Time.timeScale = Math.Abs(Math.Abs(Time.timeScale)) <=0 ? 1 : 0;
                   
                }
                    break;
                case GameState.End:
                    GameObject.Find("MissionStatusPanel").GetComponent<Animator>().SetBool("StartMove", true);
                    GameObject.Find("MissionResult").GetComponent<Text>().text = "Game Over!";
                    break;
                case GameState.Restart: 
                    GameObject.Find("MissionStatusPanel").GetComponent<Animator>().SetBool("StartMove", false);
                    GameManager.GetPlayerManager().CreatePlayer();
                    GameManager.GetWorldManager().RestartWorld();//.StartLevel();
                    break;
            }
        }
    }
}