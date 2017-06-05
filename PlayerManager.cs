using System.Collections;
using System.Globalization;
using Assets.Static;
using Assets.UI.ProgressBar.Script;
using Assets.Utils;
using ProgressBar;
using UnityEngine;
using UnityEngine.UI;

//TODO make playermanager to controls multyplayers
//TODO make Instaniating player after level isLoaded
namespace Assets.Managers
{
    /// <summary>
    /// Class for store and manage playerstat data and make UI controls
    /// </summary>
//TODO string const arrays;
    public class PlayerManager : MonoBehaviour
    {
        [HideInInspector]
        public float Distancecomplete;
        [HideInInspector]
        public float Kills;
        public readonly string[] Statuspanel = { "MissionStatus", "HitPointSensor", "FuelGauge", "KillStat" };
        public float Gametime;
        private bool _trigger;
        private float _deltakills;
        private float _deltadistance;
        private AlivePlayer _playerstatus;
        private GameObject _player;
        private Vector3 _respawnposition;
        private GameMissions _gmissions;
        private static readonly StageManager Stageman = new StageManager();
        private ProgressBarBehaviour _missionstatus;
        private ProgressBarBehaviour _hitpointsensor;
        private ProgressRadialBehaviour _fuelgauge;
        public float Test = 0;
        private Text _killstat;

        private void Awake()
        {
            _gmissions = ScriptableObject.CreateInstance<GameMissions>();
            _respawnposition = GameObject.Find("FlyCopter").transform.position;
            _player = GameObject.Find("FlyCopter");
            CreatePlayer();
            _missionstatus = GameObject.Find("MissionStatus").GetComponent<ProgressBarBehaviour>();
            _hitpointsensor = GameObject.Find("HitPointSensor").GetComponent<ProgressBarBehaviour>();
            _fuelgauge = GameObject.Find("FuelGauge").GetComponent<ProgressRadialBehaviour>();
            _killstat = GameObject.Find("KillStat").GetComponent<Text>();
            Stageman.AddMission(_gmissions.Chm1);
            Stageman.AddMission(_gmissions.Chm2);
            Stageman.AddMission(_gmissions.Chm3);
        }
        public void CreatePlayer()
        {
            _player.SetActive(true);
            _player.transform.position = _respawnposition;
            if (!_player.GetComponent<AlivePlayer>())
                _playerstatus = _player.AddComponent<AlivePlayer>();
            _playerstatus.Born(500);
            Distancecomplete = 0;
            Stageman.SetCurrentMissionID(0);
            SetTrigerFalse();
        }

        private void Start()
        {
            _missionstatus.OnCompleteMethods.AddListener(ShowMissionPanel);
            Stageman.OnMissionComplete.AddListener(SetTrigerFalse);
            Stageman.GetCurrentMission().Currentfloat = 0;
            _missionstatus.SetFillerSize(10000);
        }

        private void Update()
        {
            Gametime += Time.deltaTime;
            SelectMissionValues(Stageman.GetCurrentMissionID());
            _missionstatus.StartText(Stageman.GetCurrentMission().MissionObjective);
            Distancecomplete += _player.GetComponent<Rigidbody>().velocity.z * Time.deltaTime;
            _killstat.text = Kills.ToString(CultureInfo.InvariantCulture);
            _missionstatus.Value = Mathf.Min(GetProcent(Stageman.GetCurrentMission().GetCondition().Floatcondition, Stageman.GetCurrentMission().Currentfloat), 100);
            if (_playerstatus == null) return;
            _fuelgauge.Value = _playerstatus.GetFuel() / 20f;
            _hitpointsensor.Value = _playerstatus.GetHP() * 2;
            //TODO delete test button
            // if (Input.GetButtonDown("Test"))
        }
        public GameObject GetPlayer()
        {
            return _player;
        }
        public AlivePlayer GetPlayerStatus()
        {
            return _playerstatus;
        }
        public float GetProcent(float target, float val)
        {
            float percent = (val / target * 100);
            return percent;
        }
        public void ShowMissionPanel()
        {
            GameObject.Find("MissionStatusPanel").GetComponent<Animator>().SetBool("StartMove", true);
            GameObject.Find("MissionResult").GetComponent<Text>().text = Stageman.EndMission();
            StartCoroutine(HideMissionStatus());
        }
        public void SetTrigerFalse()
        {
            _trigger = false;
            _deltakills = 0;
            _deltadistance = 0;
        }
        public void SelectMissionValues(int id)
        {
            switch (id)
            {
                case 0:
                    Stageman.GetCurrentMission().Currentfloat = Distancecomplete;
                    //  GameManager.GetAImanager().StopAllCoroutines();
                    break;
                case 1:
                    if (!_trigger)
                    {
                        _deltakills = Kills;
                        _trigger = true;
                    }
                    Stageman.GetCurrentMission().Currentfloat = Kills - _deltakills;
                    break;
                case 2:
                    if (!_trigger)
                    {
                        _deltadistance = Distancecomplete;
                        _trigger = true;
                    }
                    Stageman.GetCurrentMission().Currentfloat = Distancecomplete - _deltadistance;
                    break;
            }
        }

        private IEnumerator HideMissionStatus()
        {
            yield return new WaitForSeconds(2.5f);
            GameObject.Find("MissionStatusPanel").GetComponent<Animator>().SetBool("StartMove", false);
        }
    }
}