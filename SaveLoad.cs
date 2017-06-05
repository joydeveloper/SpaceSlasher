using System;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Utils
{
//TODO test  GameObjectDataItem LoadGameObjectJSON(string directory, string filename, int id)
    /// <summary>
    /// Static Class for SLoperations,with:files,json,gameobjects from resources
    /// </summary>
    public static class SaveLoad
    {
        // public enum ReadType{binary,txt,json }
        /// <summary>
        /// Just create directory:return operation result 
        /// </summary>
        public static bool CreateDirectory(string directory)
        {
            try
            {
                if (Directory.Exists(directory))
                    return true;
                Directory.CreateDirectory(directory);
                return true;
            }
            catch
            {
                return false;

            }
        }
        /// <summary>
        /// Create file:return string fullpath  
        /// </summary>
        public static string CreateFile(string directory, string filename)
        {
            if (!File.Exists(directory + filename))
                return File.Create(directory + filename).Name;
            return directory + filename;
        }
        /// <summary>
        /// CD Drivers:return drivers string array
        /// </summary>
        public static string[] DrivesInfo()
        {
            return Directory.GetLogicalDrives();
        }
        /// <summary>
        /// Create file in Assets/StreamingAssets:return string fullpath *{with "/"+ filename,file will be in Assets/StreamingAssets else Assets} 
        /// </summary>
        public static string CreateStreamingAssetFile(string filename)
        {
            if (!File.Exists(Application.streamingAssetsPath + filename))
                return CreateFile(Application.streamingAssetsPath, filename);
            return Application.streamingAssetsPath + filename;
        }
        /// <summary>
        /// ReadToEnd file:return txt string *{if bool=true:path=Assets/StreamingAssets+path }
        /// </summary>
        public static string ReadTXTFromFile(string path, bool isstreamasset)
        {
            string filePath;
            if (isstreamasset)
            {
                filePath = Application.streamingAssetsPath + path;
            }
            else
                filePath = path;
            if (File.Exists(filePath))
            {
                try
                {
                    return new StreamReader(filePath, new UTF8Encoding()).ReadToEnd();
                }
                catch (Exception e)
                {

                    return e.Message;
                }
            }
            return "";

        }
        /// <summary>
        /// ReadToEnd file:return byte array *{if bool=true:path=Assets/StreamingAssets+path }
        /// </summary>
        public static byte[] ReadBINFromFile(string path, bool isstreamasset)
        {
            string filePath;
            if (isstreamasset)
            {
                filePath = Application.streamingAssetsPath + path;
            }
            else
                filePath = path;
            if (File.Exists(filePath))
            {
                try
                {
                    return File.ReadAllBytes(filePath);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    return null;
                }
            }
            return null;
        }
        /// <summary>
        /// ReadToEnd file and return from json:type-GameObjectData *{if bool=true:path=Assets/StreamingAssets+path }
        /// </summary>
        public static GameObjectData ReadJSONFromFile(string path, bool isstreamasset)
        {
            string filePath;
            if (isstreamasset)
            {
                filePath = Application.streamingAssetsPath + path;
            }
            else
                filePath = path;
            if (File.Exists(filePath))
            {
                try
                {
                    string dataAsJson = File.ReadAllText(filePath);
                    return JsonUtility.FromJson<GameObjectData>(dataAsJson);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    return null;
                }
            }
            return null;
        }
        /// <summary>
        /// ReadToEnd and return from json:type-GameObjectData *{path=Resources }
        /// </summary>
        public static GameObjectData ReadJSONFromResources(string filename,string folder=null)
        {   
            try
            {
                TextAsset bindata = Resources.Load(folder+filename) as TextAsset;
                if (bindata != null)
                {
                    var dataAsJson = bindata.text;
                    var gdt = JsonUtility.FromJson<GameObjectData>(dataAsJson);
                    return gdt;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
             return null;
        }
        /// <summary>
        /// Store GameObjectDataItem:return data from gameobject {*Add lines if file exist}
        /// </summary>
        public static GameObjectDataItem StoreGameObjectJSON(string directory, string filename, GameObject go)
        {
            GameObjectDataItem gd = new GameObjectDataItem();
            if (go)
            {
                gd.Position = go.transform.position;
                gd.Rotation = go.transform.rotation;
                gd.ObjectName = go.transform.name;
            }
            else
                return null;
            var dataJson = JsonUtility.ToJson(gd);
            if (File.Exists(directory + filename))
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(directory + filename))
                    {
                        sw.WriteLine(dataJson);
                    }
                    return gd;
                }
                catch
                {
                    return null;
                }
            }
            try
            {
                using (StreamWriter sw = File.CreateText(directory + filename))
                {
                    sw.WriteLine(dataJson);
                }
                return gd;
            }
            catch (Exception e)
            {
                Debug.Log("Error" + e.Message);
                return null;
            }
        }
        /// <summary>
        /// Load GameObjectDataItem from index:where number of string=id:return data from loaded data 
        /// </summary>
        public static GameObjectDataItem LoadGameObjectJSON(string directory, string filename, int id)
        {
            int i = 0;
            if (!File.Exists(directory + filename)) return null;
            try
            {
                GameObjectDataItem gd;
                using (StreamReader sr = new StreamReader(directory + filename))
                {
                    while (i != id)
                    {
                        i++;
                        sr.ReadLine();
                    }
                    string dataJson = sr.ReadLine();
                    gd = JsonUtility.FromJson<GameObjectDataItem>(dataJson);
                }
                return gd;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Store tagselected GameObjectData:return data from gameobjects 
        /// </summary>
        public static GameObjectData StoreGameObjectsJSON(string directory, string filename, string tag)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            GameObjectData gdt = new GameObjectData {Items = new GameObjectDataItem[objects.Length]};
            int i = 0;
            foreach (GameObject go in objects)
            {
                GameObjectDataItem gd = new GameObjectDataItem
                {
                    Position = go.transform.position,
                    Rotation = go.transform.rotation,
                    ObjectName = go.transform.name
                };
                gdt.Items[i] = gd;
                i++;
            }
            string json = JsonUtility.ToJson(gdt);
            File.WriteAllText(directory + filename, json);
            return gdt;
        }
        /// <summary>
        /// Preload gameobject:return GameObject from resource {prefabs must be in Resource/Prefabs/"objectname"} 
        /// </summary>
        public static GameObject GetResourcePrefab(string objectname)
        {
            return Resources.Load("Prefabs/" + objectname) as GameObject;     
        }
        public static GameObject InstantiateResourcePrefab(string objectname,Vector3 pos,Quaternion rot)
        {
            var go = Resources.Load("Prefabs/" + objectname) as GameObject;
            Object.Instantiate(go, pos, rot);
            return go;
        }
        public static GameObject[] InstantiateResourcePrefabs(GameObjectData god)
        {
            GameObject[] gos = new GameObject[god.Items.Length];
            int i = 0;
            foreach (GameObjectDataItem godi in god.Items)
            {
                gos[i] = Resources.Load("Prefabs/" + godi.ObjectName) as GameObject;
                if (gos[i])
                    Object.Instantiate(gos[i], godi.Position, godi.Rotation);
                i++;
            }
            return gos;
        }
    }
    [Serializable]
    public class GameObjectData
    {
        public GameObjectDataItem[] Items;
        ~GameObjectData()

        {
        
            Debug.Log(Items.Length+" gameobjectdata destructed");
        }
    }
    [Serializable]
    public class GameObjectDataItem
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public string ObjectName;
    }
}