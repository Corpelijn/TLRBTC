using Assets.Scripts.ObjectPoolNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Veins
{
    public abstract class Vein
    {
        private int id;
        private GameObject gameObject;
        private bool isDrawn;
        private int drawcalls;

        protected Vein[] exits;
        protected Vein entry;
        protected ObjectPoolTypes type;

        public Vein(int id, ObjectPoolTypes type)
        {
            this.type = type;
            this.id = id;
        }

        public void SetExit(int index, Vein vein)
        {
            this.exits[index] = vein;
        }

        public void SetEntry(Vein vein)
        {
            this.entry = vein;
        }

        public void DrawMe(VeinExitPoint connectInfo)
        {
            if (!isDrawn)
            {
                if (connectInfo != null)
                {
                    GetGameObject().transform.position = connectInfo.middle;
                    VeinExitPoint vp = GetEntryVeinExitInformation();

                    VeinExitPoint delta = vp - connectInfo;

                    // Check if there is any delta between the veins
                    if (!delta.IsZero())
                    {
                        float angle = CalculateAngle(connectInfo.middle, connectInfo.left, vp.left);

                        GetGameObject().transform.localEulerAngles = new Vector3(0, angle, 0);
                    }

                }

                drawcalls = 0;
                isDrawn = true;
            }

            drawcalls++;
        }

        private float CalculateAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
        {
            /**
             *       c
             *      /|
             *   ac/ |bc 
             *    /  |
             *  a --- b
             *     ab
             */

            float ac = Vector3.Distance(pointA, pointC);
            float ab = Vector3.Distance(pointA, pointB);
            float bc = Vector3.Distance(pointC, pointB);

            float a = Mathf.Acos((Mathf.Pow(ac, 2) + Mathf.Pow(ab, 2) - Mathf.Pow(bc, 2)) / (2 * ac * ab)) * 180 / Mathf.PI;

            return a;
        }

        public int GetId()
        {
            return id;
        }

        public void DestoryGameObject()
        {
            ObjectPool.GivebackObject(gameObject);
            gameObject = null;
            isDrawn = false;
        }

        public GameObject GetGameObject()
        {
            if (gameObject == null)
            {
                gameObject = ObjectPool.GetNextObject(type);
                gameObject.GetComponent<VeinObject>().id = this.id;
                gameObject.transform.parent = VeinBuilder.INSTANCE.transform;
            }

            return gameObject;
        }

        public void RemoveDrawCall()
        {
            drawcalls--;
        }

        public int GetDrawCalls()
        {
            return drawcalls;
        }

        public VeinExitPoint GetStraightVeinExitInformation()
        {
            return GetVeinExitInformation(VeinExitTypes.StraightExit);
        }

        public VeinExitPoint GetEntryVeinExitInformation()
        {
            return GetVeinExitInformation(VeinExitTypes.Entry);
        }

        private VeinExitPoint GetVeinExitInformation(VeinExitTypes type)
        {
            Transform transform = this.GetGameObject().transform;
            string name = type == VeinExitTypes.Entry ? "exit0" : type == VeinExitTypes.StraightExit ? "exit1" : "exit2";

            Transform exit = null;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == name)
                {
                    exit = transform.GetChild(i);
                    break;
                }
            }

            return VeinExitPoint.ParseExit(exit);
        }

        public Vein GetStraight()
        {
            return exits[0];
        }

        public Vein GetEntry()
        {
            return this.entry;
        }
    }
}
