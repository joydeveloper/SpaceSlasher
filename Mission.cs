namespace Assets.Utils
{
    public abstract class Mission
    {
        protected Mission(string missionobj, string missondesc, float fcond, float tcond)
        {
            Missiondescription = missondesc;
            MissionObjective = missionobj;
            _mcond = new MissionCondition(fcond, tcond);
        }
        public float Currentfloat;
        public float Currenttime;
        public const string Resultstring= "Mission Complete";
        public string MissionObjective;
        public string MissionResult;
        public string Missiondescription;
        public struct MissionCondition
        {
            public float Floatcondition, Timecondition;

            public MissionCondition(float fc, float tc)
            {
                Floatcondition = fc;
                Timecondition = tc;
            }
        }
        private readonly MissionCondition _mcond;
        public MissionCondition GetCondition()
        {
            return _mcond;
        }
    }
    public class ChampaignMission : Mission
    {
        public void SetMissionResult(string result)
        {
            MissionResult = result;
        }
        public ChampaignMission(string missionobj, string missondesc, float fcond, float tcond) : base(missionobj, missondesc, fcond, tcond)
        {
            MissionResult = Resultstring;
        }
    }
}