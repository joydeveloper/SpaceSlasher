using System.Collections.Generic;
using System.Linq;
using Assets.Utils;
using UnityEngine;

namespace Assets.Managers
{
    /// <summary>
    /// Class for creating and instancing gameobjects
    /// </summary>
    public class ObjectManager : MonoBehaviour
    {
        public GameObject[] Staticobjects;
        public static ThisGameObjectsFactory Staticfactory = new ThisGameObjectsFactory();
        private readonly ObjectPool _pool = new ObjectPool();
        private List<GameObject> _bombs;
        private List<GameObject> _fuels;
        public void Awake()
        {
            _bombs = new List<GameObject>();
           _fuels = new List<GameObject>();
            _bombs = CreateObjGroup(50, ThisGameObjectsFactory.ObjectType.Cube, Staticobjects[0]);
            _fuels = CreateObjGroup(2, ThisGameObjectsFactory.ObjectType.Cube, Staticobjects[1]);
            _pool.CreatePool(_bombs);
            _pool.CreatePool(_fuels);
            _pool.CreateClonePool(_bombs);
            _pool.CreateClonePool(_fuels);
        }
        public ObjectPool GetPool()
        {
            return _pool;
        }
        public static Component SetComponent(GameObject go, string type)
        {
            switch (type)
            {
                case "AliveCocoon":

                    if (!go.GetComponent<AliveCoocon>())
                    {
                        return go.AddComponent(typeof(AliveCoocon)) as AliveCoocon;
                    }
                    return go.GetComponent<AliveCoocon>();
            }
            return null;
        }
        public static AliveObject GetAliveObject(GameObject go)
        {
            return go.GetComponent<AliveObject>();
        }
        public static GameObject CreateObj(string name, GameObject objtype)
        {
            return Staticfactory.GetObject(name).Create(objtype);//);
        }
        public static GameObject CreateObj(ThisGameObjectsFactory.ObjectType type, GameObject objtype)
        {
            return Staticfactory.GetObject(type).Create(objtype);
        }
        public static List<GameObject> CreateObjGroup(int count, string name, GameObject objtype)
        {
            List<GameObject> gogroup = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                gogroup.Add(CreateObj(name, objtype));//);
            }
            return gogroup;
        }
        public static List<GameObject> CreateObjGroup(int count, ThisGameObjectsFactory.ObjectType objtype, GameObject go)
        {
            List<GameObject> gogroup = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                gogroup.Add(CreateObj(objtype, go));//);
            }
            return gogroup;
        }
        public static void TranslateObjectPosition(GameObject goobject, Vector3 pos)
        {
            goobject.transform.Translate(pos);
        }
        /// <summary>
        /// Class ObjectPool:DesignPattern
        /// </summary>
        public class ObjectPool
        {
            public static int Count;
            private static List<PoolRecord> _poolRecordList;
            public int GetCount()
            {
                return Count;
            }
            /// <summary>
            /// Class ObjectPool unit
            /// </summary>
            public class PoolRecord
            {
                public GameObject Instance;
                public bool InUse;
                public PoolRecord(GameObject go)
                {
                    Instance = go;
                    InUse = true;
                }
                public void SetTrue()
                {
                    InUse = true;
                }
                public void SetFalse()
                {
                    InUse = false;
                }
            }
            public ObjectPool()
            {
                _poolRecordList = new List<PoolRecord>();
            }
            public void SetDeleted(GameObject go)
            {
                _poolRecordList.Find(x => x.Instance = go).SetFalse();
                go = _poolRecordList.Find(x => x.Instance = go).Instance;
                go.SetActive(false);
            }
            public static List<GameObject> GetPoolGameObj(ObjectPool pool)
            {
                List<GameObject> golist = new List<GameObject>();
                for (int i = 0; i < pool.GetCount(); i++)
                {
                    golist.Add(pool.GetRecord(i).Instance);
                }
                return golist;
            }
            public void CreatePool(List<GameObject> golist)
            {
                foreach (GameObject t in golist)
                {
                    Count++;
                    var record = new PoolRecord(t);
                    _poolRecordList.Add(record);
                }
            }
            public void CreateClonePool(List<GameObject> golist)
            {
                foreach (GameObject t in golist)
                {
                    Count++;
                    var record =
                        new PoolRecord(Instantiate(t)) {Instance = {name = t.name + "Clone"}};
                    _poolRecordList.Add(record);
                }
            }
            //TODO add update objects
            public static List<PoolRecord> SelectObjects(string name)
            {
                return _poolRecordList.Where(pr => pr.Instance.name == name).ToList();
            }
            public void RefreshPool()
            {
                foreach (var pr in _poolRecordList)
                {
                    pr.Instance.SetActive(true);
                    pr.InUse = true;
                }
            }
            public static void RefreshPool(string pool)
            {
                foreach (var pr in SelectObjects(pool))
                {
                    pr.Instance.SetActive(true);
                    pr.InUse = true;
                }
            }
            public void ClearPool()
            {
                _poolRecordList.Clear();
            }
            public List<PoolRecord> Getpool()
            {
                return _poolRecordList;
            }
            public PoolRecord GetRecord(int id)
            {
                return _poolRecordList[id];
            }
            public void DeleteObjects()
            {
                foreach (var pr in _poolRecordList)
                    if (pr.InUse == false)
                    {
                        Destroy(pr.Instance);
                    }
            }
        }
    }
}
