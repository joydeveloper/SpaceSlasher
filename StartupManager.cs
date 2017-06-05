using System.Collections;
using Assets.Utils;
using UnityEngine;

namespace Assets.Managers
{
    /// <summary>
    /// Class for preload resources and set start options
    /// </summary>
    public class StartupManager : MonoBehaviour
    {
        private const string Soundpath = "Sound/Tracks/";
        private const string Sfxpath = "Sound/SFX/";
        private string[] _tracklist;
        private string[] _sfxkeys;
        private string[] _sfxsounds;
        private JukeBox _mainjuke;
        private SoundBox _mainsounds;

        private void Awake()
        {
            _tracklist = new [] { "mainsplash","maintheme" };//choose tracks for playing in this scene;
            _mainjuke = new JukeBox(Soundpath, _tracklist);//loaded tracks - no lags
            _sfxkeys = new[] { "airstart" , "airrightleft", "spacebarclick", "selectclick", "acceptclick", "declineclick", "changesceneclick", "exitclick","selectgame","footerdown","footerclick" };//define sfx sounds
            _sfxsounds = new [] { "airstart", "airrightleft", "spacebarclick", "selectclick", "acceptclick", "declineclick", "changesceneclick", "exitclick", "selectgame", "footerdown", "footerclick" };//appropriate sfx with keys
            _mainsounds = new SoundBox(Sfxpath, _sfxkeys, _sfxsounds);
        }
        private IEnumerator Start()
        {

            while (!IsLoadComplete())
            {

                yield return null;
            }
            SoundManager.GetInstance(_mainjuke, _mainsounds);//create soundmanager with loaded music

        }

        private bool IsLoadComplete()
        {

            return (_mainjuke.isJukeready && _mainsounds.isSoundready);//put boolstates underloads instances

        }
    }
}