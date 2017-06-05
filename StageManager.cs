using System.Collections;
using System.Collections.Generic;
using Assets.Utils;
using UnityEngine.Events;

namespace Assets.Managers
{
    /// <summary>
    /// Class for manage missions/sceneEvents 
    /// </summary>
    public class StageManager
    {
        public IEnumerator[] Actions;
        public static int Currentmission;
        protected List<Mission> Missions;
        public StageManager()
        {
            Missions = new List<Mission>();
        }
        public UnityEvent OnMissionComplete = new UnityEvent();
        public Mission GetMission(int id)
        {
            return Missions[id];
        }
        public Mission GetCurrentMission()
        {
            return Missions[Currentmission];
        }
        public int GetCurrentMissionID()
        {
            return Currentmission;
        }
        public void SetCurrentMissionID(int id)
        {
            Currentmission = id;
        }
        public void AddMission(Mission mission)
        {
            Missions.Add(mission);
        }
        public string StartMission(int id)
        {
            return Missions[id].MissionObjective;
        }
        public string EndMission()
        {
            if (Currentmission < Missions.Count - 1)
            {
                Currentmission++;
                StartMission(Currentmission);
                OnMissionComplete.Invoke();
                return Missions[Currentmission - 1].MissionResult;
            }
            return "Stage complete";
        }
        public int GetCount()
        {
            return Missions.Count;
        }
    }
    /// <summary>
    /// Class helper to gamemanager for fast change sublevel composition:DesignPatterns:Facade
    /// </summary>
    public static class Facade
    {
        private static SubsystemA a = new SubsystemA();
        private static SubsystemB b = new SubsystemB();
        private static SubsystemC c = new SubsystemC();
        public static void Operation1()
        {
        }
        public static void Operation2()
        {
        }
    }
    internal class SubsystemA
    {
        internal string A1()
        {
            return "";
        }
        internal string A2()
        {
            return "";
        }
    }
    internal class SubsystemB
    {
        internal string B1()
        {
            return "Subsystem B, Method B1\n";
        }
    }
    internal class SubsystemC
    {
        internal string C1()
        {
            return "Subsystem C, Method C1\n";
        }
    }
}