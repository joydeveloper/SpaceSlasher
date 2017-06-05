#if UNITY_EDITOR
using System.IO;
using Assets.Localization;
using UnityEditor;
using UnityEngine;

namespace Assets.Managers
{
    /// <summary>
    /// Class for editing localized files and data
    /// </summary>
    public class LocalizedTextEditor : EditorWindow
    {
        public LocalizationData LocalizationData;
        [MenuItem("Window/Localized Text Editor")]
        private static void Init()
        {
            GetWindow(typeof(LocalizedTextEditor)).Show();
        }
        private void OnGUI()
        {
            if (LocalizationData != null)
            {
                var serializedObject = new SerializedObject(this);
                var serializedProperty = serializedObject.FindProperty("localizationData");
                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }
            }

            if (GUILayout.Button("Load data"))
            {
                LoadGameData();
            }

            if (GUILayout.Button("Create new data"))
            {
                CreateNewData();   
            }
        }
        private void LoadGameData()
        {
            var filePath = EditorUtility.OpenFilePanel("Select localization data file", Application.streamingAssetsPath, "json");

            if (string.IsNullOrEmpty(filePath)) return;
            var dataAsJson = File.ReadAllText(filePath);

            LocalizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        }
        private void SaveGameData()
        {
            var filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "", "json");

            if (string.IsNullOrEmpty(filePath)) return;
            var dataAsJson = JsonUtility.ToJson(LocalizationData);
            File.WriteAllText(filePath, dataAsJson);
        }
        private void CreateNewData()
        {
            LocalizationData = new LocalizationData();
        }
    }
}
#endif