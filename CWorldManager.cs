using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Assets.Managers;
using Assets.Utils;
using UnityEngine;

// ReSharper disable IteratorNeverReturns

namespace Assets.GameModes.Campaign.Scripts
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class CWorldManager : WorldManager
    {
        
        private float  _step = 200;
        private const float Maxtilt = 10;
        private float _daytime = 24;
        private const float Tiltspeed = 0.2f;
        private const float Starttime = 0;
        private const float Hour = 0.5f;
        private const float Dayscale = 0.1f;
        private string[] _cityobjects;
        private string[] _earthcityterrainsheights;
        //float time;

        private IEnumerator _coroutine;


        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Awake()
        {
         
            _cityobjects = new[] { "StartCity", "City1", "City2", "City3" };
            _earthcityterrainsheights = new [] { "StartCityTerr", "CityTerr1", "CityTerr2", "CityTerr3" };
            LightTransform = GameObject.Find("Directional LightTransform").transform;
        }
        public override void StartLevel()
        {

            _daytime = 24;
            EarthTerrain ef = new EarthTerrain(10, 0.05f, TerrainDataArray[2], "City1");
            EarthTerrain ef1 = new EarthTerrain(10, 0.05f, TerrainDataArray[3], "City2");
            EarthCity earthcity = new EarthCity(new Vector3(-ef.Terraindata.size.x / 2, 0, 0));
            earthcity.Maxpos = new Vector3(ef.Terraindata.size.x / 2, 10, ef.Terraindata.size.z);
            Builders.Add(earthcity);
            EarthCity earthcity1 = new EarthCity(new Vector3(-ef1.Terraindata.size.x / 2, 0, ef1.Terraindata.size.z));
            earthcity1.Maxpos = new Vector3(ef1.Terraindata.size.x / 2, 10, ef1.Terraindata.size.z * 2);
            Builders.Add(earthcity1);
            LeveldDirector.ConstructFromFile(earthcity, ef, _cityobjects[0], _earthcityterrainsheights[0], "City/", ObjectManager.ObjectPool.SelectObjects("FuelBar"));
            LeveldDirector.ConstructFromFile(earthcity1, ef1, _cityobjects[1], _earthcityterrainsheights[1], "City/", ObjectManager.ObjectPool.SelectObjects("FuelBar"));
            _coroutine = SwapTerrainsbyDistance(ef, ef1);
            StartCoroutine(_coroutine);
            StartCoroutine(LightWorks());

            //  LightOff();
        }
        public override void RestartWorld()
        {
            _daytime = 24;
            Builders[0].Startpos = new Vector3(-GetSizeActiveTerrain(0).x / 2, 0, 0);
            Builders[0].Maxpos = new Vector3(GetSizeActiveTerrain(0).x / 2, 10, GetSizeActiveTerrain(0).z);
            Builders[1].Startpos = new Vector3(-GetSizeActiveTerrain(1).x / 2, 0, GetSizeActiveTerrain(1).z);
            Builders[1].Maxpos = new Vector3(GetSizeActiveTerrain(1).x / 2, 10, GetSizeActiveTerrain(1).z * 2);
            TerrainModificator.SetPadding(TerrainDataArray[0], 5, 5);
            TerrainModificator.SetPadding(TerrainDataArray[1], 5, 5);
            LeveldDirector.Translate(Builders[1], 1, ObjectManager.ObjectPool.SelectObjects("TriangleBombClone"), ObjectManager.ObjectPool.SelectObjects("FuelBarClone"));
            LeveldDirector.Translate(Builders[0], 0, ObjectManager.ObjectPool.SelectObjects("TriangleBomb"), ObjectManager.ObjectPool.SelectObjects("FuelBar"));
            _step = 200;
        }
        public override void EndLevel()
        {

        }
        private Vector3 GetSizeActiveTerrain(int id)
        {
            return new Vector3(TerrainDataArray[id].size.x, TerrainDataArray[id].size.y, TerrainDataArray[id].size.z);

        }

        private IEnumerator SwapTerrainsbyTime(Terrains first, Terrains second)
        {
            while (true)
            {
                Distance += first.Terraindata.size.y;
                yield return new WaitForSeconds(2);
                if (first.TerrainGo.transform.position.z > second.TerrainGo.transform.position.z)
                {
                    second.TerrainGo.transform.Translate(0, 0, Distance * 2 - 2);

                }
                else
                {
                    first.TerrainGo.transform.Translate(0, 0, Distance * 2 - 2);

                }

            }

            // ReSharper disable once IteratorNeverReturns
        }
        /// <summary>
        /// Swap minded terrains and objects from objectpool
        /// </summary>
        /// 
        /// TODO make one enumerator 
        private IEnumerator SwapTerrainsbyDistance(Terrains first, Terrains second)
        {
            Distance = 200;
            while (true)
            {
                yield return new WaitUntil(() => GameManager.GetPlayerManager().Distancecomplete > _step);
                if (first.TerrainGo.transform.position.z > second.TerrainGo.transform.position.z)
                {
                    Builders[1].Startpos = new Vector3(-second.Terraindata.size.x / 2, 0, _step + Distance);
                    Builders[1].Maxpos = new Vector3(second.Terraindata.size.x / 2, 10, _step + Distance * 2);
                    second.SetModificator(TerrainModificator.RandomModificator());
                    second.SetPadding(5, 5);
                    LeveldDirector.Translate(Builders[1], 1, ObjectManager.ObjectPool.SelectObjects("TriangleBombClone"), ObjectManager.ObjectPool.SelectObjects("FuelBarClone"));
                }
                else
                {
                    Builders[0].Startpos = new Vector3(-first.Terraindata.size.x / 2, 0, _step + Distance);
                    Builders[0].Maxpos = new Vector3(first.Terraindata.size.x / 2, 10, _step + Distance * 2);
                    first.SetModificator(TerrainModificator.RandomModificator());
                    first.SetPadding(5, 5);
                    LeveldDirector.Translate(Builders[0], 0, ObjectManager.ObjectPool.SelectObjects("TriangleBomb"), ObjectManager.ObjectPool.SelectObjects("FuelBar"));
                }
                _step += Distance;
            }
        }
        /// <summary>
        /// Swap loaded terrains and objects from file
        /// </summary>
        private IEnumerator SwapTerrainsbyDistanceFromFile(Terrains first, Terrains second)
        {
            Distance = 200;
            while (true)
            {
                yield return new WaitUntil(() => GameManager.GetPlayerManager().Distancecomplete > _step);
                if (first.TerrainGo.transform.position.z > second.TerrainGo.transform.position.z)
                {
                    Builders[1].Startpos = new Vector3(-second.Terraindata.size.x / 2, 0, _step + Distance);
                    Builders[1].Maxpos = new Vector3(second.Terraindata.size.x / 2, 10, _step + Distance * 2);
                    LeveldDirector.Translate(Builders[1], 1, _cityobjects[3],_earthcityterrainsheights[3], "City/", ObjectManager.ObjectPool.SelectObjects("FuelBarClone"));
                }
                else
                {
                    Builders[0].Startpos = new Vector3(-first.Terraindata.size.x / 2, 0, _step + Distance);
                    Builders[0].Maxpos = new Vector3(first.Terraindata.size.x / 2, 10, _step + Distance * 2);
                    LeveldDirector.Translate(Builders[0], 0, _cityobjects[2], _earthcityterrainsheights[2], "City/", ObjectManager.ObjectPool.SelectObjects("FuelBar"));
                }
                _step += Distance;
            }
        }

        private IEnumerator LightWorks()
        {
            float tilt = LightTransform.transform.rotation.eulerAngles.y;
            // float yaw = LightTransform.transform.rotation.eulerAngles.x;
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                tilt += Tiltspeed;
                _daytime += Hour;
                LightTransform.transform.rotation = Quaternion.Euler(2 * Mathf.PI * (_daytime / 2) * Dayscale + Starttime + 50, Mathf.Sin(tilt) * Maxtilt, 0);
            }
        }

        private void LateUpdate()
        {
            LightTransform.GetComponent<Light>().color = Color.Lerp(new Color(0.99f, 1f, 0.99f, 1f), new Color(1f, 0.56f, 0.28f, 0f), Mathf.PingPong(Time.time / 100, 1f));
            //TODO targetunderatacklight
            //LightTransform.GetComponent<Light>().color = Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0.8f, 0.1f, 0f, 0.3f), Mathf.PingPong(Time.time, 0.9f));
        }
    }
}

//daytime = 24;
// EarthTerrain ef = new EarthTerrain(10, 0.05f, TerrainDataArray[0], "Earth1");
//  EarthTerrain ef1 = new EarthTerrain(10, 0.05f, TerrainDataArray[1], "Earth2");
//Desert desert = new Desert(new Vector3(-ef.terraindata.size.x / 2, 0, 0));
//desert.maxpos = new Vector3(ef.terraindata.size.x / 2, 10, ef.terraindata.size.z);
//builders.Add(desert);
//Desert desert1 = new Desert(new Vector3(-ef1.terraindata.size.x / 2, 0, ef1.terraindata.size.z));
//desert1.maxpos = new Vector3(ef1.terraindata.size.x / 2, 10, ef1.terraindata.size.z * 2);
//builders.Add(desert1);
//LeveldDirector.Construct(desert, ef, ObjectManager.ObjectPool.SelectObjects("TriangleBomb"), ObjectManager.ObjectPool.SelectObjects("FuelBar"));
//LeveldDirector.Construct(desert1, ef1, ObjectManager.ObjectPool.SelectObjects("TriangleBombClone"), ObjectManager.ObjectPool.SelectObjects("FuelBarClone"));
//// LeveldDirector.GetTerrainFactory().GetRawType(1).SetModificator(TerrainModificator.ModificatorDelegate(TerrainModificator.PERLIN_SINUS));
//ef.SetPadding(5, 5);
//ef1.SetPadding(5, 5);
//coroutine = SwapTerrainsbyDistance(ef, ef1);
//StartCoroutine(coroutine);
//StartCoroutine(LightWorks());