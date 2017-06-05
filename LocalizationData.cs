using System;

namespace Assets.Localization
{
    [Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] Items;
    }

    [Serializable]
    public class LocalizationItem
    {
        public string Key;


        public string Value;

    }
}