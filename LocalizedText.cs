using UnityEngine;
using UnityEngine.UI;

namespace Assets.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        public string Key;
        private Text _text;

        private void Start()
        {
            RefreshText();
        }
        public void RefreshText()
        {
            _text = GetComponent<Text>();
            if (LocalizationManager.Instance.GetIsReady())
                _text.text = LocalizationManager.Instance.GetLocalizedValue(Key);
            else
                return;
        }
    }
}