using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;
        private string _selectedloc;
        private Dictionary<string, string> _localizedText;
        private bool _isReady;
        private const string MissingLocString = "Loc not found";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        public void LoadLocalizedText(string fileName)
        {
            _localizedText = new Dictionary<string, string>();
            var filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(filePath))
            {
                var dataAsJson = File.ReadAllText(filePath);
                var loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

                foreach (var t in loadedData.Items)
                {
                    _localizedText.Add(t.Key, t.Value);
                }
            }
            _selectedloc = fileName;
            _isReady = true;
        }
        public string GetLocalizedValue(string key)
        {
            string result = "Missing_loc_string";
            if (_localizedText.ContainsKey(key))
            {
                result = _localizedText[key];
            }
            return result;
        }
        public bool GetIsReady()
        {
            return _isReady;
        }
        public string GetLocalePath()
        {
            return _selectedloc;
        }
        public static string LoadFooterInfo(string fileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            try
            {
                return new StreamReader(filePath, new UTF8Encoding()).ReadToEnd();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}