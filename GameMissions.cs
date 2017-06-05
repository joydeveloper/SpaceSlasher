using Assets.Utils;
using UnityEngine;

namespace Assets.Static
{
    public class GameMissions : ScriptableObject
    {
        #region ChampaingMissions
        public ChampaignMission Chm1 = new ChampaignMission("Pass the distance 1000", "Need a cover", 100, 0);
        public ChampaignMission Chm2 = new ChampaignMission("Kill 20 enemies", "Aliens come to fast...", 20, 0);
        public ChampaignMission Chm3 = new ChampaignMission("Pass the distance 2000", "Go to space lift", 2000, 0);  
        #endregion
    }
}
