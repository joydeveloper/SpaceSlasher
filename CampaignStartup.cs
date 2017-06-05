using UnityEngine;

namespace Assets.Managers
{
    /// <summary>
    /// Class for startup campaign and define properties and managers(sealed)
    /// </summary>
    public sealed class CampaignStartup : MonoBehaviour {
        public GameObject CworldManager;
        public GameObject CobjectManager;
        public GameObject CplayerManager;
        public GameObject CaiManager;

        private void Awake () {
            GameManager.GetInstance(CworldManager, CobjectManager, CplayerManager, CaiManager);
        }
    }
}
