using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectPoolNamespace
{
    class ObjectPool : MonoBehaviour
    {
        public List<ObjectPoolGameObject> objects = null;

        private List<ObjectList> lists;

        public void Start()
        {
            instance = this;

            lists = new List<ObjectList>();
            foreach (ObjectPoolGameObject obj in objects)
            {
                lists.Add(new ObjectList(obj.type, obj));
            }
        }

        private static ObjectPool instance;

        public static GameObject GetNextObject(ObjectPoolTypes tag)
        {
            foreach (ObjectList list in instance.lists)
            {
                if (list.GetTag() == tag)
                {
                    return list.GetNext();
                }
            }

            return null;
        }

        public static void GivebackObject(GameObject obj)
        {
            ObjectPoolGameObject gameobject = obj.GetComponent<ObjectPoolGameObject>();
            foreach (ObjectList list in instance.lists)
            {
                if (list.GetTag() == gameobject.type)
                {
                    list.GiveBack(gameobject);
                    gameobject.transform.parent = ObjectPool.instance.transform;
                }
            }
        }
    }
}
