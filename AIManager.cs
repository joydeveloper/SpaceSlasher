using System.Collections;
using System.Collections.Generic;
using Assets.Utils;
using UnityEngine;

namespace Assets.Managers
{
    public class AiManager : MonoBehaviour
    {
        public GameObject[] EnemyObjects;
        public GameObject[] NeitralObjects;
        public GameObject[] AliedObjects;
        public float StartWait=5;
        public float WaveWait = 10;
        private readonly List<GameObject> _cocoons = new List<GameObject>();
        ///private ObjectManager.ObjectPool aipool;
        private void Start()
        {
            //cocoons = ObjectManager.CreateObjGroup(20, "Capsule", enemyobjects[0]);
            //BornEnemies(cocoons, "AliveCocoon");
            //GameManager.GetObjectmanager().GetPool().CreatePool(cocoons);
            //StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            while (true)
            {
                yield return new WaitForSeconds(StartWait);
                while (true)
                {
                    foreach (ObjectManager.ObjectPool.PoolRecord go in ObjectManager.ObjectPool.SelectObjects("AlienCocoon"))
                    {
                        go.Instance.transform.position = new Vector3(Random.Range(-20, 20), GameManager.GetPlayerManager().GetPlayer().transform.position.y-1, GameManager.GetPlayerManager().GetPlayer().transform.position.z + Random.Range(50, 70));
                    }
                    yield return new WaitForSeconds(WaveWait);
                    UpdateEnemiesHP(_cocoons, 20);
                    ObjectManager.ObjectPool.RefreshPool("AlienCocoon");
                }
            }
        }
       private void BornEnemies(List<GameObject> enemies,string type,int hp=10)
        {
            foreach (GameObject go in enemies)
            {
                AliveCoocon ao = ObjectManager.SetComponent(go, type) as AliveCoocon;
                if (ao != null) ao.Born(hp);
            }
        }
        private void UpdateEnemiesHP(List<GameObject> enemies, int hp = 10)
        {
            foreach (GameObject go in enemies)
            {
                AliveCoocon ao = go.GetComponent<AliveCoocon>();
                ao.Born(hp);
            }
        }
    }
}
