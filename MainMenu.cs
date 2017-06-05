using System.Collections;
using Assets.Localization;
using Assets.Managers;
using Assets.Static;
using Assets.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.UI
{
    public class MainMenu : MonoBehaviour
    {
        public RawImage LogoScreen;
        public RawImage GameScreen;
        public GameObject SplashPanel;
        public GameObject LanguagePanel;
        private bool _islangisteners;
        private Sprite[] _flagssprite;
        private const int Ruflag = 0;
        private const int Enflag = 1;
        private const string Flagpath = "Flags/";
        private string[] _flagsnames;
        private string[] _footerRu;
        private string[] _footerEn;
        private int _footerstate;
        private const string En_langname = "localizedText_en.json";
        private const string Ru_langname = "localizedText_ru.json";
        private const string Playerselectedlang = "selectedlang";
        private string _playername;
        private LocalizedText[] _loctext;

        private Globals _globals;
        //  GUISetup guisetup;
        #region Unity

        private void OnEnable()
        {
            // gameObject.SetActive(true);
            //TODO if build
            //  PlayerPrefs.SetString("playername","");
            _playername = PlayerPrefs.HasKey("playername") ? PlayerPrefs.GetString("playername") : null;
            if (!string.IsNullOrEmpty(_playername))
                DoName(_playername);
        }

        private void Awake()
        {
            _flagsnames = new [] { "rusflag", "enflag" };
            LoadFlagsSprite(Flagpath, _flagsnames);
            _globals = ScriptableObject.CreateInstance<Globals>();
            _footerRu = new [] { "credits_ru.txt", "gratitude_ru.txt", "tips_ru.txt", "support_ru.txt", "tovisit_ru.txt" };
            _footerEn = new [] { "credits_en.txt", "gratitude_en.txt", "tips_en.txt", "support_en.txt", "tovisit_en.txt" };
        }

        private void Start()
        {
            if (GameState.isFirstStartup && GameScreen )
                StartCoroutine(SkipLogo());
            else
                Destroy(GameScreen);
#if !MOBILE_INPUT
            if (  GameState.isFirstStartup && SplashPanel)
                StartCoroutine(SkipSplashScreen());
            else
                Destroy(SplashPanel);
#endif
            _loctext = FindObjectsOfType(typeof(LocalizedText)) as LocalizedText[];
            if (!PlayerPrefs.HasKey(Playerselectedlang) || PlayerPrefs.GetString(Playerselectedlang) == null)
            {
                OnClick_SelectLanguage();
            }
            else
            {
                LocalizationManager.Instance.LoadLocalizedText(PlayerPrefs.GetString(Playerselectedlang));
                GameObject.Find("LanguageSelector").GetComponent<Image>().sprite = _flagssprite[GetCurrentLanguage()];
            }
        }

        private IEnumerator SkipSplashScreen()
        {
            yield return new WaitUntil(() => Time.realtimeSinceStartup > (_globals.Waitlogotime) && Input.GetButtonDown("Jump"));
            DisableSplashScreen();
        }

        private IEnumerator SkipLogo()
        {
            yield return new WaitForSeconds(_globals.Waitlogotime);
            SoundManager.PlayPlaylist(0, false, 1);
            LogoScreen.GetComponent<RawImage>().enabled = false;
        }
        #endregion
        #region Public
        public void OnClick_SelectLanguage()
        {
            SoundManager.PlaySFX("selectclick");
            DoLanguagePanel();
        }
        public void OnClick_Single()
        {
            SoundManager.PlayLoopSFX("airstart", 1.5f);
            DoSingleSelect();
        }
        public void OnClick_Multy()
        {
            SoundManager.PlayLoopSFX("airrightleft", 1.5f);
            DoMultySelect();
        }

        public void OnClick_MainMenu()
        {
            SoundManager.RemoveSFXSourceLooped();
            DoMainMenuGUI();
        }
        public void OnClick_Credits()
        {
            ShowFooterPanel(0);
            _footerstate = 0;
        }
        public void OnClick_Grattide()
        {
            ShowFooterPanel(1);
            _footerstate = 1;
        }
        public void OnClick_Tips()
        {
            ShowFooterPanel(2);
            _footerstate = 2;
        }
        public void OnClick_Support()
        {
            ShowFooterPanel(3);
            _footerstate = 3;
        }
        public void OnClick_ToVisit()
        {
            ShowFooterPanel(4);
            _footerstate = 4;
        }
        public void OnClick_VK()
        {
            Application.OpenURL("http://vk.com/kotzizi");
        }

        public void OnClick_Campaign()
        {
            SoundManager.PlaySFX("spacebarclick");
            GameState.SetFirstStartupFalse() ;
            SceneManager.LoadScene("Campaign");
        }
        public void OnClick_AcceptName()
        {
            DoName(_playername);
        }
        public void OnNameEdit()
        {
            SoundManager.PlaySFX("footerclick");
        }
        public void NameValidator()
        {
            _playername = GameObject.Find("Name").GetComponent<Text>().text.Length == 0 ?
                "playerid" : GameObject.Find("Name").GetComponent<Text>().text;
        }
        public void OnClick_Exit()
        {
            Application.Quit();
        }
        //TODO on first start show panel and set default lang
        public void SetLang(string lang)
        {
            SoundManager.PlaySFX("acceptclick");
            LocalizationManager.Instance.LoadLocalizedText(lang);
            PlayerPrefs.SetString(Playerselectedlang, lang);
            SetFlagSprite();
            DoLanguagePanel();
            foreach (LocalizedText lc in _loctext)
            {
                lc.RefreshText();
            }
            RefreshFooter();
        }
        public void HideNamePanel()
        {
            GUISetup.TogglePanel(GameObject.Find("NamePanel"));
        }
        public void HideFooterPanel()
        {
            GameObject.Find("FooterPanel").GetComponent<Animator>().SetBool("StartMove", false);
            SoundManager.PlaySFX("footerdown");
        }
        #endregion
        #region Private  
        private void DisableSplashScreen()
        {
            SoundManager.PlaySFX("spacebarclick");
            SplashPanel.SetActive(false);
            //  SoundManager.PlayBGM(1, true, 3);

        }

        private void SetLangButListeners()
        {
            GameObject.Find("EnBut").GetComponent<Button>().onClick.AddListener(() => SetLang(En_langname));
            GameObject.Find("RusBut").GetComponent<Button>().onClick.AddListener(() => SetLang(Ru_langname));
            _islangisteners = true;
        }
        private void SetFlagSprite()
        {
            GameObject.Find("LanguageSelector").GetComponent<Image>().sprite = _flagssprite[GetCurrentLanguage()];
        }
        private int GetCurrentLanguage()
        {

            switch (LocalizationManager.Instance.GetLocalePath())
            {
                case Ru_langname: return Ruflag;
                case En_langname: return Enflag;

            }
            return 0;
        }
        private void LoadFlagsSprite(string path, string[] names)
        {
            _flagssprite = new Sprite[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                _flagssprite[i] = Resources.Load<Sprite>(path + names[i]);
            }
        }

        private void ShowFooterPanel(int id_text)
        {
            SoundManager.PlaySFX("footerclick");
            GameObject.Find("FooterPanel").GetComponent<Animator>().SetBool("StartMove", true);
            switch (GetCurrentLanguage())
            {
                case 0:
                    GameObject.Find("Footertext").GetComponent<Text>().text = LocalizationManager.LoadFooterInfo(_footerRu[id_text]);
                    break;
                case 1:
                    GameObject.Find("Footertext").GetComponent<Text>().text = LocalizationManager.LoadFooterInfo(_footerEn[id_text]);
                    break;
            }
        }
        private void RefreshFooter()
        {

            if (GameObject.Find("FooterPanel").GetComponent<Animator>().GetBool("StartMove"))
                ShowFooterPanel(_footerstate);

        }

        #endregion
        #region GUIsetup

        private static void DoMainMenuGUI()
        {
            SoundManager.PlaySFX("acceptclick");
            GameObject.Find("Campaign").GetComponent<Animator>().SetBool("StartMove", false);
            GameObject.Find("Survival").GetComponent<Animator>().SetBool("StartMove", false);
            GameObject.Find("Multy").GetComponent<Animator>().SetBool("StartMove", false);
            GameObject.Find("Multy").GetComponent<Animator>().SetBool("StartMoveLeft", false);
        }

        private void DoMainMenuButtons()
        {
        }

        private void DoLanguagePanel()
        {
            GUISetup.TogglePanel(LanguagePanel);
            if (!_islangisteners)
                SetLangButListeners();
        }

        private static void DoSingleSelect()
        {
            SoundManager.PlaySFX("selectgame");
            GameObject.Find("SelectGame1").GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedValue("Campaign");
            GameObject.Find("SelectGame2").GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedValue("Survival");
            GameObject.Find("Campaign").GetComponent<Animator>().SetBool("StartMove", true);
            GameObject.Find("Survival").GetComponent<Animator>().SetBool("StartMove", true);
            GameObject.Find("Multy").GetComponent<Animator>().SetBool("StartMove", true);
        }

        private static void DoMultySelect()
        {
            SoundManager.PlaySFX("selectgame");
            GameObject.Find("SelectGame1").GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedValue("Faststart");
            GameObject.Find("SelectGame2").GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedValue("Duoqueue");
            GameObject.Find("Campaign").GetComponent<Animator>().SetBool("StartMove", true);
            GameObject.Find("Survival").GetComponent<Animator>().SetBool("StartMove", true);
            GameObject.Find("Multy").GetComponent<Animator>().SetBool("StartMoveLeft", true);
        }

        private void DoName(string pname)
        {
            PlayerPrefs.SetString("playername", pname);
            GameObject.Find("PlayerName").GetComponent<Text>().text = pname;
            HideNamePanel();
        }

        private void DoPracticeGUI()
        {
        }

        private void DoApplyGUI()
        {
        }

        private void DoPlayGUI()
        {
        }
        #endregion
    }
}
