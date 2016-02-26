using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectPoolNamespace
{
    class ObjectList
    {
        private List<ObjectPoolGameObject> objects;
        private ObjectPoolGameObject original;
        private ObjectPoolTypes tag;

        public ObjectList(ObjectPoolTypes tag, ObjectPoolGameObject original)
        {
            this.tag = tag;
            this.original = original;
            this.objects = new List<ObjectPoolGameObject>();
        }

        public GameObject GetNext()
        {
            if (objects.Count > 0)
            {
                GameObject go = objects[0].gameObject;
                objects.RemoveAt(0);
                return go;
            }

            objects.Add(GameObject.Instantiate(original));
            return this.GetNext();
        }

        public ObjectPoolTypes GetTag()
        {
            return this.tag;
        }

        public void GiveBack(ObjectPoolGameObject obj)
        {
            objects.Add(obj);
        }
    }
}
