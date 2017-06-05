using UnityEngine;

namespace Assets.Utils
{
    public class MapHelper : MonoBehaviour
    {
        private ObjectMap omap;
        public GameObject road;
        public GameObject cross;
        public GameObject tcross;
        public GameObject light;
        public GameObject fern;
        public GameObject garbage;
        // Use this for initialization
        private void Start()
        {
            //   PlaceObjects();
            //  Texture2D tex=  Terrain.activeTerrain.terrainData.splatPrototypes[0].texture;
            //   Debug.Log(Terrain.activeTerrains[0].name);//.terrainData.name);
            //    Debug.Log(Terrain.activeTerrains[1].name);
            //   Terrain.activeTerrains[0].terrainData.SetHeights(0,0, TerrainGenerator.LoadResourceMap("StartCityTerr", "City/"));
        }

        private void Update()
        {
            if (Input.GetButtonDown("Test"))
                //  StoreData();
                LoadData();
            if (Input.GetButtonDown("Submit"))
                //  StoreData();
                StoreData();

        }

        private void LoadData()
        {
            GameObjectData gdt = new GameObjectData();
            gdt = SaveLoad.ReadJSONFromResources("City3", "City/");
            Terrain.activeTerrain.terrainData.SetHeights(0, 0, TerrainGenerator.LoadResourceMap("CityTerr3", "City/"));
            SaveLoad.InstantiateResourcePrefabs(gdt);
            LightOff();
        }

        private void LightOff()
        {
            int i=0;
            do
            {
                GameObject.Find("StreetLight(Clone)").name = "StreetLight" + i;//.FindGameObjectsWithTag("City"))
                Debug.Log(("StreetLight" + i));
                i++;
            }
            while (GameObject.Find("StreetLight(Clone)"));

            for (int k = 0; k < i; k++)
                if (GameObject.Find("StreetLight" + k).transform.rotation.eulerAngles.z > 0)
                {
                    GameObject.Find("StreetLight" + k.ToString()).transform.GetChild(0).GetComponent<Light>().enabled = false;
                }
        }

        private void StoreData()
        {
            /// GameObjectData god = new GameObjectData();
            /// 

            GameObjectsClean("City");
            SaveLoad.StoreGameObjectsJSON(Application.dataPath, "StartCity.json", "City");
            TerrainGenerator.StoreMap(Application.dataPath + "CityTerr1", Terrain.activeTerrain.terrainData.GetHeights(0, 0, 129, 129));
            Debug.Log("DataStored");
        }

        private string StringCloneClean(string s)
        {
            if (s.EndsWith("(Clone)"))
            {
                s= s.Remove(s.Length - 7, 7);
            }
            return s;
        }

        private void GameObjectsClean(string tag)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
                go.name = StringCloneClean(go.name);
        }

        private void PlaceObjects()
        {
            omap = new ObjectMap(128, 200);
            road.tag = "City";
            cross.tag = "City";
            tcross.tag = "City";
            light.tag = "City";
            // omap.DrawLineH(new Vector2<int>(0, 5), 0, 0, 19, 'R');
            //  omap.DrawLineV(new Vector2<int>(0, 20), 0, 0, 14, 'L');
            omap.DrawLineV(new Vector2<int>(0, 40), 0, 0, 14, 'L');
            omap.DrawLineV(new Vector2<int>(0, 85), 0, 0, 14, 'L');
            omap.DrawLineV(new Vector2<int>(0, 120), 0, 0, 14, 'L');
            omap.DrawLineV(new Vector2<int>(0, 180), 0, 0, 14, 'L');
            //   omap.DrawLineV(new Vector2<int>(0, 100), 0, 0, 14, 'L');
            // omap.DrawLineV(new Vector2<int>(8, 5), 0, 0, 14, 'L');
            //  omap.DrawLineH(new Vector2<int>(0, 16), 0, 0, 19, 'R');
            omap.DrawLineH(new Vector2<int>(0, 55), 0, 0, 19, 'R');
            omap.DrawLineH(new Vector2<int>(0, 80), 0, 0, 19, 'R');
            //for (int i = 12; i < 200; i += 20)
            //    omap.DrawPoint(new Vector2<int>(9, i), 'S');

            //for (int i = 12; i < 200; i += 20)
            //    omap.DrawPoint(new Vector2<int>(11, i), 'U');
            for (int i = 12; i < 200; i += 20)
                omap.DrawPoint(new Vector2<int>(60, i), 'S');
            for (int i = 12; i < 200; i += 20)
                omap.DrawPoint(new Vector2<int>(75, i), 'U');
            for (int i = 17; i < 200; i += 10)
                omap.DrawPoint(new Vector2<int>(Random.Range(5, 15), i), 'F');
            for (int i = 10; i < 200; i += 5)
                omap.DrawPoint(new Vector2<int>(Random.Range(0, 40), i), 'G');
            //  omap.DrawLineV(new Vector2<int>(5, 2), 0, 0, 20, 'R');
            // Debug.Log(testobject.GetComponent<MeshRenderer>().bounds.size);
            //  int xmax = 12;// Mathf.FloorToInt(128 / road.GetComponent<MeshRenderer>().bounds.size.x);
            //   int zmax = 20;//  Mathf.FloorToInt(200 / road.GetComponent<MeshRenderer>().bounds.size.z);
            Quaternion Lroad = Quaternion.Euler(-90, 90, 0);
            for (int i = 0; i < omap.MapArray.GetLength(0); i++)
            for (int j = 0; j < omap.MapArray.GetLength(1); j++)
            {
                if (omap.MapArray[i, j] == 'R')
                    Instantiate(road, new Vector3(i - 60, 3, j * road.GetComponent<MeshRenderer>().bounds.size.z), road.transform.rotation);
                if (omap.MapArray[i, j] == 'L')
                    Instantiate(road, new Vector3(i * road.GetComponent<MeshRenderer>().bounds.size.x - 60, 3, j), Lroad);
                if (omap.MapArray[i, j] == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'R')
                {
                    Instantiate(tcross, new Vector3(i * road.GetComponent<MeshRenderer>().bounds.size.x, 3, j * road.GetComponent<MeshRenderer>().bounds.size.z), tcross.transform.rotation);
                }
                if (omap.MapArray[i, j] == 'R' && omap.GetNeighbourhood(i, j, ObjectMap.direction.W) == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'L')
                {
                    Instantiate(cross, new Vector3(i, 3, j * cross.GetComponent<MeshRenderer>().bounds.size.z), cross.transform.rotation);
                }
                if (omap.MapArray[i, j] == 'S')
                    Instantiate(light, new Vector3(i - 60, 3, j), Quaternion.Euler(0, -90, 0));
                if (omap.MapArray[i, j] == 'U')
                    Instantiate(light, new Vector3(i - 60, 3, j), Quaternion.Euler(0, 90, 0));
                if (omap.MapArray[i, j] == 'F')
                    Instantiate(fern, new Vector3(i, Terrain.activeTerrain.terrainData.GetHeight(i, j), j), Quaternion.identity);
                if (omap.MapArray[i, j] == 'G')
                    Instantiate(garbage, new Vector3(i-20, Terrain.activeTerrain.terrainData.GetHeight(i, j), j), Quaternion.identity);
            }

        }

    }
}

//StartCity-------------------
//omap = new ObjectMap(128, 200);
//road.tag = "City";
//        cross.tag = "City";
//        tcross.tag = "City";
//        LightTransform.tag = "City";
//        omap.DrawLineH(new Vector2<int>(0, 5), 0, 0, 19, 'R');
//        omap.DrawLineV(new Vector2<int>(0, 0), 0, 0, 14, 'L');
//        omap.DrawLineH(new Vector2<int>(0, 16), 0, 0, 19, 'R');
//        omap.DrawLineH(new Vector2<int>(0, 55), 0, 0, 19, 'R');
//        omap.DrawLineH(new Vector2<int>(0, 80), 0, 0, 19, 'R');
//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(9, i), 'S');

//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(11, i), 'U');
//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(60, i), 'S');
//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(75, i), 'U');
//        for (int i = 17; i< 200; i += 10)
//            omap.DrawPoint(new Vector2<int>(Random.Range(5, 15), i), 'F');
//        //  omap.DrawLineV(new Vector2<int>(5, 2), 0, 0, 20, 'R');
//        // Debug.Log(testobject.GetComponent<MeshRenderer>().bounds.size);
//        //  int xmax = 12;// Mathf.FloorToInt(128 / road.GetComponent<MeshRenderer>().bounds.size.x);
//        //   int zmax = 20;//  Mathf.FloorToInt(200 / road.GetComponent<MeshRenderer>().bounds.size.z);
//        Quaternion Lroad = Quaternion.Euler(-90, 90, 0);
//        for (int i = 0; i<omap.MapArray.GetLength(0); i++)
//            for (int j = 0; j<omap.MapArray.GetLength(1); j++)
//            {
//                if (omap.MapArray[i, j] == 'R')
//                    GameObject.Instantiate(road, new Vector3(i - 60, 3, j* road.GetComponent<MeshRenderer>().bounds.size.z), road.transform.rotation);
//                if (omap.MapArray[i, j] == 'L')
//                    GameObject.Instantiate(road, new Vector3(i* road.GetComponent<MeshRenderer>().bounds.size.x - 60, 3, j), Lroad);
//                if (omap.MapArray[i, j] == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'R')
//                {
//                    GameObject.Instantiate(tcross, new Vector3(i* road.GetComponent<MeshRenderer>().bounds.size.x, 3, j* road.GetComponent<MeshRenderer>().bounds.size.z), tcross.transform.rotation);
//                }
//                if (omap.MapArray[i, j] == 'R' && omap.GetNeighbourhood(i, j, ObjectMap.direction.W) == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'L')
//                {
//                    GameObject.Instantiate(cross, new Vector3(i, 3, j* cross.GetComponent<MeshRenderer>().bounds.size.z), cross.transform.rotation);
//                }
//                if (omap.MapArray[i, j] == 'S')
//                    GameObject.Instantiate(LightTransform, new Vector3(i - 60, 3, j), Quaternion.Euler(0, -90, 0));
//                if (omap.MapArray[i, j] == 'U')
//                    GameObject.Instantiate(LightTransform, new Vector3(i - 60, 3, j), Quaternion.Euler(0, 90, 0));
//                if (omap.MapArray[i, j] == 'F')
//                    GameObject.Instantiate(fern, new Vector3(i, Terrain.activeTerrain.terrainData.GetHeight(i, j), j), Quaternion.identity);
//            }
//---------------------------------------------------------

///City1
///   omap = new ObjectMap(128, 200);
//road.tag = "City";
//    cross.tag = "City";
//    tcross.tag = "City";
//    LightTransform.tag = "City";

//    omap.DrawLineH(new Vector2<int>(0, 5), 0, 0, 19, 'R');

//    omap.DrawLineH(new Vector2<int>(0, 16), 0, 0, 19, 'R');
//    omap.DrawLineH(new Vector2<int>(0, 55), 0, 0, 19, 'R');
//    omap.DrawLineH(new Vector2<int>(0, 80), 0, 0, 19, 'R');
//    for (int i = 12; i< 200; i += 20)
//        omap.DrawPoint(new Vector2<int>(9, i), 'S');

//    for (int i = 12; i< 200; i += 20)
//        omap.DrawPoint(new Vector2<int>(11, i), 'U');
//    for (int i = 12; i< 200; i += 20)
//        omap.DrawPoint(new Vector2<int>(60, i), 'S');
//    for (int i = 12; i< 200; i += 20)
//        omap.DrawPoint(new Vector2<int>(75, i), 'U');
//    for (int i = 17; i< 200; i += 10)
//        omap.DrawPoint(new Vector2<int>(Random.Range(5, 15), i), 'F');

//    Quaternion Lroad = Quaternion.Euler(-90, 90, 0);
//    for (int i = 0; i<omap.MapArray.GetLength(0); i++)
//        for (int j = 0; j<omap.MapArray.GetLength(1); j++)
//        {
//            if (omap.MapArray[i, j] == 'R')
//                GameObject.Instantiate(road, new Vector3(i - 60, 3, j* road.GetComponent<MeshRenderer>().bounds.size.z), road.transform.rotation);
//            if (omap.MapArray[i, j] == 'L')
//                GameObject.Instantiate(road, new Vector3(i* road.GetComponent<MeshRenderer>().bounds.size.x - 60, 3, j), Lroad);
//            if (omap.MapArray[i, j] == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'R')
//            {
//                GameObject.Instantiate(tcross, new Vector3(i* road.GetComponent<MeshRenderer>().bounds.size.x, 3, j* road.GetComponent<MeshRenderer>().bounds.size.z), tcross.transform.rotation);
//            }
//            if (omap.MapArray[i, j] == 'R' && omap.GetNeighbourhood(i, j, ObjectMap.direction.W) == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'L')
//            {
//                GameObject.Instantiate(cross, new Vector3(i, 3, j* cross.GetComponent<MeshRenderer>().bounds.size.z), cross.transform.rotation);
//            }
//            if (omap.MapArray[i, j] == 'S')
//                GameObject.Instantiate(LightTransform, new Vector3(i - 60, 3, j), Quaternion.Euler(0, -90, 0));
//            if (omap.MapArray[i, j] == 'U')
//                GameObject.Instantiate(LightTransform, new Vector3(i - 60, 3, j), Quaternion.Euler(0, 90, 0));
//            if (omap.MapArray[i, j] == 'F')
//                GameObject.Instantiate(fern, new Vector3(i, Terrain.activeTerrain.terrainData.GetHeight(i, j), j), Quaternion.identity);
//        }

//}
//-------------------------------------------

//city2
//omap = new ObjectMap(128, 200);
//road.tag = "City";
//        cross.tag = "City";
//        tcross.tag = "City";
//        LightTransform.tag = "City";
//        omap.DrawLineH(new Vector2<int>(0, 5), 0, 0, 19, 'R');
//        omap.DrawLineV(new Vector2<int>(0, 20), 0, 0, 14, 'L');
//        omap.DrawLineV(new Vector2<int>(0, 60), 0, 0, 14, 'L');
//        omap.DrawLineV(new Vector2<int>(0, 100), 0, 0, 14, 'L');
//        // omap.DrawLineV(new Vector2<int>(8, 5), 0, 0, 14, 'L');
//        omap.DrawLineH(new Vector2<int>(0, 16), 0, 0, 19, 'R');
//        omap.DrawLineH(new Vector2<int>(0, 55), 0, 0, 19, 'R');
//        omap.DrawLineH(new Vector2<int>(0, 80), 0, 0, 19, 'R');
//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(9, i), 'S');

//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(11, i), 'U');
//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(60, i), 'S');
//        for (int i = 12; i< 200; i += 20)
//            omap.DrawPoint(new Vector2<int>(75, i), 'U');
//        for (int i = 17; i< 200; i += 10)
//            omap.DrawPoint(new Vector2<int>(Random.Range(5, 15), i), 'F');
//        for (int i = 10; i< 200; i += 5)
//            omap.DrawPoint(new Vector2<int>(Random.Range(0, 40), i), 'G');
//        //  omap.DrawLineV(new Vector2<int>(5, 2), 0, 0, 20, 'R');
//        // Debug.Log(testobject.GetComponent<MeshRenderer>().bounds.size);
//        //  int xmax = 12;// Mathf.FloorToInt(128 / road.GetComponent<MeshRenderer>().bounds.size.x);
//        //   int zmax = 20;//  Mathf.FloorToInt(200 / road.GetComponent<MeshRenderer>().bounds.size.z);
//        Quaternion Lroad = Quaternion.Euler(-90, 90, 0);
//        for (int i = 0; i<omap.MapArray.GetLength(0); i++)
//            for (int j = 0; j<omap.MapArray.GetLength(1); j++)
//            {
//                if (omap.MapArray[i, j] == 'R')
//                    GameObject.Instantiate(road, new Vector3(i - 60, 3, j* road.GetComponent<MeshRenderer>().bounds.size.z), road.transform.rotation);
//                if (omap.MapArray[i, j] == 'L')
//                    GameObject.Instantiate(road, new Vector3(i* road.GetComponent<MeshRenderer>().bounds.size.x - 60, 3, j), Lroad);
//                if (omap.MapArray[i, j] == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'R')
//                {
//                    GameObject.Instantiate(tcross, new Vector3(i* road.GetComponent<MeshRenderer>().bounds.size.x, 3, j* road.GetComponent<MeshRenderer>().bounds.size.z), tcross.transform.rotation);
//                }
//                if (omap.MapArray[i, j] == 'R' && omap.GetNeighbourhood(i, j, ObjectMap.direction.W) == 'L' && omap.GetNeighbourhood(i, j, ObjectMap.direction.E) == 'L')
//                {
//                    GameObject.Instantiate(cross, new Vector3(i, 3, j* cross.GetComponent<MeshRenderer>().bounds.size.z), cross.transform.rotation);
//                }
//                if (omap.MapArray[i, j] == 'S')
//                    GameObject.Instantiate(LightTransform, new Vector3(i - 60, 3, j), Quaternion.Euler(0, -90, 0));
//                if (omap.MapArray[i, j] == 'U')
//                    GameObject.Instantiate(LightTransform, new Vector3(i - 60, 3, j), Quaternion.Euler(0, 90, 0));
//                if (omap.MapArray[i, j] == 'F')
//                    GameObject.Instantiate(fern, new Vector3(i, Terrain.activeTerrain.terrainData.GetHeight(i, j), j), Quaternion.identity);
//                if (omap.MapArray[i, j] == 'G')
//                    GameObject.Instantiate(garbage, new Vector3(i-20, Terrain.activeTerrain.terrainData.GetHeight(i, j), j), Quaternion.identity);
//            }
//------------------------------------------------------------
