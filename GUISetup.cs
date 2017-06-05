using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Assets.Utils
{
    public class GUISetup
    {
        public delegate void GUIMethod();
        public enum EMenuMode
        {
            EMenuContact,
            EMenuCompany
        }
        protected EnumeratedDelegate<EMenuMode, GUIMethod> MMenuEnum;

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Awake()
        {
            MMenuEnum = new EnumeratedDelegate<EMenuMode, GUIMethod>
            (
                new GUIMethod[]
                {
                    DoMainMenuGUI,	//eMenu_MainMenu,
                    DoContact    //eMenu_Review,			
                }
            );
        }
        public static void TogglePanel(GameObject panel)
        {
            panel.SetActive(!panel.activeSelf);
        }
        public void DoMainMenuGUI()
        {
        }
        public void DoContact()
        {
        }
    }
}