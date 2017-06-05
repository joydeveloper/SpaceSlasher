using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Utils
{
    public abstract class TerrainGenerator : MonoBehaviour
    {
        public static void LoadTerrain(TerrainData td, string filename)
        {
            float[,] dat = td.GetHeights(0, 0, 129, 129);
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                for (int i = 0; i < td.heightmapWidth; i++)
                {
                    for (int j = 0; j < td.heightmapHeight; j++)
                    {
                        dat[i, j] = br.ReadSingle();
                    }
                }
                br.Close();
                td.SetHeights(0, 0, dat);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        public static void StoreMap(string path, float[,] heights)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (float f in heights)
                    sw.WriteLine(f);
            }
        }
        public static float[,] LoadResourceMap(string filename, string folder = null)
        {
            var heights = new float[Terrain.activeTerrain.terrainData.heightmapHeight, Terrain.activeTerrain.terrainData.heightmapWidth];
            try
            {
                TextAsset bindata = Resources.Load(folder + filename) as TextAsset;
                string[] allstrings = bindata.text.Split('\r', '\n');
                string[] separatestrings=new string[allstrings.Length/2+1];
                int k = 0;
                foreach (string s in allstrings)
                {

                    if (s.Trim() != "")
                    {
                        separatestrings[k] = s;
                        k++;
                    }
                }
                k = 0;
                for (int i = 0; i < Terrain.activeTerrain.terrainData.heightmapWidth; i++)
                for (int j = 0; j < Terrain.activeTerrain.terrainData.heightmapHeight; j++)

                {
                    heights[i, j] = float.Parse(separatestrings[k]);
                    k++;
                }
                return heights;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }
        public static float[,] LoadMap(string path, int width, int height)
        {
            float[,] heights = new float[height, width];
            using (StreamReader sw = new StreamReader(path))
            {
                for (int i = 0; i < Terrain.activeTerrain.terrainData.heightmapWidth; i++)
                for (int j = 0; j < Terrain.activeTerrain.terrainData.heightmapHeight; j++)

                {
                    heights[i, j] = float.Parse(sw.ReadLine());
                }
            }
            return heights;
        }
        public static GameObject CreateTerrain(TerrainData td)
        {
            return Terrain.CreateTerrainGameObject(td);
        }
    }
    public abstract class Terrains
    {
        public GameObject TerrainGo;
        public TerrainData Terraindata;
        protected float TileSize;
        protected float Maxheight;
        public string Name;
        public enum TerrainType { Earth }
        public TerrainType Type;
        public abstract void SetModificator(Delegate modificator);
        public void SetPadding(float x, float y)
        {
            TerrainModificator.SetPadding(Terraindata, x, y);
        }
    }
    public sealed class EarthTerrain : Terrains
    {
        public EarthTerrain(float tileSize = 10, float maxheight = 2, TerrainData td = null, string name = "Earth")
        {
            Terraindata = td;
            Name = name;
            TileSize = tileSize;
            Maxheight = maxheight;
            Type = TerrainType.Earth;
            SetModificator(TerrainModificator.ModificatorDelegate(TerrainModificator.FLAT));
        }
        public override void SetModificator(Delegate modificator)
        {
            modificator.DynamicInvoke(Terraindata, TileSize, Maxheight);
        }
    }
    public class TerrainFactory
    {
        private static List<Terrains> _rawterrain;
        public static List<GameObject> Terrains;
        public TerrainFactory()
        {
            Terrains = new List<GameObject>();
            _rawterrain = new List<Terrains>();
        }
        public void CreateTerrainProduct(Terrains terainsettings)
        {
            terainsettings.TerrainGo = TerrainGenerator.CreateTerrain(terainsettings.Terraindata);
            terainsettings.TerrainGo.name = terainsettings.Name;
            _rawterrain.Add(terainsettings);
            Terrains.Add(terainsettings.TerrainGo);
        }
        public GameObject GetTerrain(int id)
        {
            return Terrains[id];
        }
        public Terrains GetRawType(int id)
        {
            return _rawterrain[id];
        }
    }
    public class TerrainModificator
    {
        public const string PERLIN_NOISE = "perlinnoise";
        public const string PERLIN_COSINUS = "perlincos";
        public const string PERLIN_SINUS = "perlinsin";
        public const string FLAT = "flat";
        //public const string SET_PADDING = "setpadding";
        public delegate void MyDelegate(TerrainData tr, float tileSize, float maxheight);
        public static MyDelegate ModificatorDelegate(string mode)
        {
            switch (mode)
            {
                case PERLIN_NOISE: return PerlinNoise;
                case PERLIN_COSINUS: return PerlinCosinus;
                case PERLIN_SINUS: return PerlinSinus;
                case FLAT: return Flat;

            }
            return null;
        }
        public static MyDelegate RandomModificator()
        {
            int f = Random.Range(0, 2);
            switch (f)
            {
                case 0: return PerlinSinus;
                case 1: return PerlinNoise;
            }
            return null;
        }
        //TODO make setpadding y(graphpatterns)
        public static void SetPadding(TerrainData tr, float padx, float pady)
        {
            var heights = tr.GetHeights(0, 0, tr.heightmapWidth, tr.heightmapHeight);

            for (int i = 0; i < tr.heightmapWidth; i++)
            {
                for (int k = 0; k < pady; k++)
                    heights[k, i] = 0f;
            }
            for (int i = 0; i < tr.heightmapHeight; i++)
            {
                for (int k = tr.heightmapHeight - (int)padx; k < tr.heightmapHeight; k++)
                    heights[k, i] = 0f;
            }
            tr.SetHeights(0, 0, heights);
        }
        public static void Flat(TerrainData tr, float tileSize, float maxheight)
        {
            var heights = new float[tr.heightmapWidth, tr.heightmapHeight];

            for (int i = 0; i < tr.heightmapWidth; i++)
            {
                for (int k = 0; k < tr.heightmapHeight; k++)
                {
                    heights[i, k] = 0.01f;
                }
            }
            tr.SetHeights(0, 0, heights);
        }
        public static void PerlinSinus(TerrainData tr, float tileSize, float maxheight)
        {
            var heights = new float[tr.heightmapWidth, tr.heightmapHeight];
            float smooth = 1;
            for (int i = 0; i < tr.heightmapWidth; i++)
            {
                for (int k = 0; k < tr.heightmapHeight; k++)
                {
                    smooth += 0.02f;
                    heights[k, i] = Mathf.Lerp(0f, Mathf.Sin((smooth) / 40), maxheight);
                }
            }
            tr.SetHeights(0, 0, heights);
        }
        public static void PerlinCosinus(TerrainData tr, float tileSize, float maxheight)
        {
            float[,] heights = new float[tr.heightmapWidth, tr.heightmapHeight];
            for (int i = 0; i < tr.heightmapWidth; i++)
            {
                for (int k = 0; k < tr.heightmapHeight; k++)
                {
                    heights[i, k] = Mathf.Lerp(0f, Mathf.Cos(i / 10f), maxheight);
                }
            }
            tr.SetHeights(0, 0, heights);
        }
        public static void PerlinNoise(TerrainData tr, float tileSize, float maxheight)
        {
            var heights = new float[tr.heightmapWidth, tr.heightmapHeight];

            for (int i = 0; i < tr.heightmapWidth; i++)
            {
                for (int k = 0; k < tr.heightmapHeight; k++)
                {
                    heights[i, k] = Mathf.PerlinNoise((i / (float)tr.heightmapWidth) * tileSize, (k / (float)tr.heightmapHeight) * tileSize) * maxheight;
                }
            }
            tr.SetHeights(0, 0, heights);
        }
        public static void PerlinNoise(TerrainData tr, float tileSize, float maxheight, Vector2 padding)
        {
            var heights = new float[tr.heightmapWidth, tr.heightmapHeight];

            for (int i = (int)padding.x; i < tr.heightmapWidth - (int)padding.x; i++)
            {
                for (int k = (int)padding.y; k < tr.heightmapHeight - (int)padding.y; k++)
                {
                    heights[i, k] = Mathf.PerlinNoise((i / (float)tr.heightmapWidth) * tileSize, (k / (float)tr.heightmapHeight) * tileSize) * maxheight;
                }
            }
            tr.SetHeights(0, 0, heights);
        }
    }
}