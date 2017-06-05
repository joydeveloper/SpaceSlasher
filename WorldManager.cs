using System.Collections;
using System.Collections.Generic;
using Assets.Utils;
using UnityEngine;

namespace Assets.Managers
{
    /// <summary>
    /// Parent class for worldmanagers {WorldManager can be instiniated by any inhireted}
    /// </summary>
    public abstract class WorldManager : MonoBehaviour
    {
        public TerrainData[] TerrainDataArray;
        public IEnumerator Routine;
        protected Transform LightTransform;
        protected float Distance = 0;
        protected List<Builder> Builders = new List<Builder>();
        protected Director LeveldDirector = new Director();
        public virtual void StartLevel() { }
        public virtual void RestartWorld() { }
        public virtual void EndLevel() { }
        /// <summary>
        /// Class for build level with objects:Design Patterns:Builder
        /// </summary>
        protected class Director
        {
            // Builder uses a complex series of steps
            public void Construct(Builder builder, Terrains terr, List<ObjectManager.ObjectPool.PoolRecord> pr, List<ObjectManager.ObjectPool.PoolRecord> pr1)
            {
                builder.BuildTerrain(terr);
                builder.BuildPartObjects(pr);
                builder.PlacePowerUps(pr1);
            }
            public void Translate(Builder builder, int id, List<ObjectManager.ObjectPool.PoolRecord> pr, List<ObjectManager.ObjectPool.PoolRecord> pr1)
            {
                builder.TranslateTerrain(id);
                builder.BuildPartObjects(pr);
                builder.PlacePowerUps(pr1);

            }
            public void Translate(Builder builder, int id, string jsonfilename, string terrheightsfilename, string folder, List<ObjectManager.ObjectPool.PoolRecord> pr1)
            {
                builder.TranslateTerrain(id);
                builder.BuildPartObjects(jsonfilename, terrheightsfilename, folder);
                builder.PlacePowerUps(pr1);

            }
            public void ConstructFromFile(Builder builder, Terrains terr,string jsonfilename, string terrheightsfilename, string folder, List<ObjectManager.ObjectPool.PoolRecord> pr1)
            {
                builder.BuildTerrain(terr);
                builder.BuildPartObjects(jsonfilename,terrheightsfilename,folder);
                builder.PlacePowerUps(pr1);
            }
            public TerrainFactory GetTerrainFactory()
            {

                return Builder.BuilderTerrainFactory;
            }
        }
        /// <summary>
        /// Class for implementation of building different terrains and objects setup
        /// </summary>
        protected abstract class Builder
        {
            public Vector3 Startpos;
            public Vector3 Maxpos;
            public string Name;
            public static TerrainFactory BuilderTerrainFactory = new TerrainFactory();
            public virtual void BuildTerrain(Terrains terr) { }
            public virtual void BuildPartObjects(List<ObjectManager.ObjectPool.PoolRecord> pr) { }
            public virtual void BuildPartObjects(string jsonfilename, string terrheightsfilename, string folder = "City/") { }
            public virtual void PlacePowerUps(List<ObjectManager.ObjectPool.PoolRecord> pr) { }
            public virtual void TranslateTerrain(int id) { }
            public static Vector3 RandomPos(Vector3 minpos, Vector3 maxpos)
            {
                return new Vector3(Random.Range(minpos.x, maxpos.x),
                    Random.Range(minpos.y, maxpos.y), 
                    Random.Range(minpos.z, maxpos.z));
            }
            ~Builder()
            {
                Debug.Log("Test builder destructor");
            }
        }
        protected class Desert : Builder
        {
            public Desert(Vector3 pos)
            {
                Startpos = pos;
            }
            public override void TranslateTerrain(int id)
            {
                BuilderTerrainFactory.GetRawType(id).TerrainGo.transform.position = Startpos;
            }
            public override void BuildTerrain(Terrains terr)
            {
                BuilderTerrainFactory.CreateTerrainProduct(terr);
                Name = terr.TerrainGo.name;
                terr.TerrainGo.transform.position = Startpos;
            }
            public override void BuildPartObjects(List<ObjectManager.ObjectPool.PoolRecord> pr)
            {
                foreach (var go in pr)
                {
                    go.Instance.transform.position = RandomPos(Startpos, Maxpos);
                }
            }
            public override void PlacePowerUps(List<ObjectManager.ObjectPool.PoolRecord> pr)
            {
                foreach (var go in pr)
                {
                    var powerpos = Startpos;
                    powerpos.y = GameManager.GetPlayerManager().GetPlayer().transform.position.y;
                    powerpos.x = Startpos.x / 2;
                    var powerposmax = Maxpos;
                    Maxpos.x = Maxpos.x / 2;
                    go.Instance.transform.position = RandomPos(powerpos, powerposmax);
                }
            }
        }
        protected class EarthCity : Builder
        {
            public EarthCity(Vector3 pos)
            {
                Startpos = pos;
            }
            public override void TranslateTerrain(int id)
            {
                BuilderTerrainFactory.GetRawType(id).TerrainGo.transform.position = Startpos;
            }
            public override void BuildTerrain(Terrains terr)
            {
                BuilderTerrainFactory.CreateTerrainProduct(terr);
                Name = terr.TerrainGo.name;
                terr.TerrainGo.transform.position = Startpos;
            }
            public override void BuildPartObjects(string jsonfilename, string terrheightsfilename,string folder = "City/")
            {
                var gdt = SaveLoad.ReadJSONFromResources(jsonfilename, folder);
                TranslateObjects(gdt, Startpos.z);
                Terrain.activeTerrain.terrainData.SetHeights(0, 0, TerrainGenerator.LoadResourceMap(terrheightsfilename, folder));
                SaveLoad.InstantiateResourcePrefabs(gdt);
                //   LightOff();
            }
            public override void PlacePowerUps(List<ObjectManager.ObjectPool.PoolRecord> pr)
            {
                foreach (ObjectManager.ObjectPool.PoolRecord go in pr)
                {
                    Vector3 powerpos = Startpos;
                    powerpos.y = GameManager.GetPlayerManager().GetPlayer().transform.position.y;
                    powerpos.x = Startpos.x / 2;
                    Vector3 powerposmax = Maxpos;
                    Maxpos.x = Maxpos.x / 2;
                    go.Instance.transform.position = RandomPos(powerpos, powerposmax);
                }
            }

            private static void TranslateObjects(GameObjectData god, float z)
            {
                foreach (GameObjectDataItem t in god.Items)
                {
                    t.Position.z += z;
                }
            }

/*
            private void LightOff()
            {
                var i = 0;
                do
                {
                    GameObject.Find("StreetLight(Clone)").name = "StreetLight" + i;//.FindGameObjectsWithTag("City"))
                    i++;
                }
                while (GameObject.Find("StreetLight(Clone)"));
                for (int k = 0; k < i; k++)
                    if (GameObject.Find("StreetLight" + k).transform.rotation.eulerAngles.z > 0)
                    {
                        GameObject.Find("StreetLight" + k.ToString()).transform.GetChild(0).GetComponent<Light>().enabled = false;
                    }
            }
*/
        }
    }
}