using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Utils
{
    public class ObjectsTypes : MonoBehaviour
    {
        public abstract class ThisGameObject
        {
            protected int Width;
            protected int Height;
            protected int Lenght;
            protected Vector3 Pos;
            public virtual GameObject Create()
            {
                return new GameObject();
            }
            //TODO add instantiate function
            public virtual GameObject Create(GameObject ego)
            {
                return new GameObject();
            }
        }
        public class Cube : ThisGameObject
        {
            public Cube(Vector3 pos)
            {
               Height = 1;
               Width = 1;
               Lenght = 1;
               Pos = pos;
            }
            public override GameObject Create()
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = Pos;
                go.transform.localScale = new Vector3(Height, Width, Lenght);
                //TODO getrealsizeobject
                //go.GetComponent<Renderer>().bounds.size = new Vector3(this.height, this.lenght, this.width);
                return go;
            }
            public override GameObject Create(GameObject ego)
            {
                var go = Instantiate(ego);
                go.name = ego.name;
                go.transform.position = Pos;
                go.transform.localScale = new Vector3(Height, Width, Lenght);
                return go;
            }
        }
        public class AliveEntity : ThisGameObject
        {
            private readonly Vector3 _aliverot;
            public AliveEntity(Vector3 pos)
            {
                Height = 2;
                Width = 2;
                Lenght = 2;
                Pos = pos;
                _aliverot = new Vector3(0, 180, 0);
            }
            public override GameObject Create()
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                go.transform.position = Pos;
                go.transform.localScale = new Vector3(Height, Width, Lenght);
                //TODO getrealsizeobject
                //go.GetComponent<Renderer>().bounds.size = new Vector3(this.height, this.lenght, this.width);
                return go;
            }
            public override GameObject Create(GameObject ego)
            {
                var go = Instantiate(ego);
                go.name = ego.name;
                go.transform.position = Pos;
                go.transform.localScale = new Vector3(Height, Width, Lenght);
                go.transform.eulerAngles =_aliverot;
                return go;
            }
        }
    }
    public class ThisGameObjectsFactory
    {
        private readonly Dictionary<string, ObjectsTypes.ThisGameObject> _staticobjects = new Dictionary<string, ObjectsTypes.ThisGameObject>();
        public enum ObjectType {Cube,Capsule }
        public ThisGameObjectsFactory()
        {
            _staticobjects.Add("Cube", new ObjectsTypes.Cube(Vector3.zero));
            _staticobjects.Add("Capsule", new ObjectsTypes.AliveEntity(Vector3.zero));
        }
        public ObjectsTypes.ThisGameObject GetObject(string key)
        {
            return _staticobjects[key];
        }
        public ObjectsTypes.ThisGameObject GetObject(ObjectType key)
        {
            switch (key)
            {
                case ObjectType.Cube: return _staticobjects["Cube"];
                case ObjectType.Capsule: return _staticobjects["Capsule"];
                default:
                    throw new ArgumentOutOfRangeException("key", key, null);
            }
        }
    }
//HashTable realization
//private Hashtable staticobjects = new Hashtable();

//public ObjectsTypes.ThisGameObject GetObject(string key)
//{
//        // Uses "lazy initialization"
//        ObjectsTypes.ThisGameObject staticobject = staticobjects[key] as ObjectsTypes.ThisGameObject;
//    if (staticobject == null)
//    {
//        switch (key)
//        {
//            case "Cube": staticobject = new ObjectsTypes.Cube(Vector3.zero); break;
//            case "Capsule": staticobject = new ObjectsTypes.AliveEntity(Vector3.zero); break;
//        }
//        staticobjects.Add(key, staticobject);
//    }
//    return staticobject;
//}
}