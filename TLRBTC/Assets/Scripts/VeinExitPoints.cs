using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class VeinExitPoint
    {
        public Vector3 top { get; set; }
        public Vector3 bottom { get; set; }
        public Vector3 left { get; set; }
        public Vector3 right { get; set; }
        public Vector3 middle { get; set; }

        public VeinExitPoint()
        {
        }

        public static VeinExitPoint ParseExit(Transform exit)
        {
            VeinExitPoint vep = new VeinExitPoint();
            for (int i = 0; i < exit.childCount; i++)
            {
                Transform currentChild = exit.GetChild(i);
                if (currentChild.name == "T")
                {
                    vep.top = currentChild.TransformPoint(currentChild.position);
                }
                else if (currentChild.name == "B")
                {
                    vep.bottom = currentChild.position;
                }
                else if (currentChild.name == "R")
                {
                    vep.right = currentChild.position;
                }
                else if (currentChild.name == "L")
                {
                    vep.left = currentChild.position;
                }
                else if (currentChild.name == "M")
                {
                    vep.middle = currentChild.position;
                }
            }

            return vep;
        }

        public static VeinExitPoint operator -(VeinExitPoint v1, VeinExitPoint v2)
        {
            VeinExitPoint ret_val = new VeinExitPoint();

            ret_val.middle = v1.middle - v2.middle;
            ret_val.left = v1.left - v2.left;
            ret_val.right = v1.right - v2.right;
            ret_val.top = v1.top - v2.top;
            ret_val.bottom = v1.bottom - v2.bottom;

            return ret_val;
        }

        public bool IsZero()
        {
            if (this.middle == Vector3.zero)
                return false;
            if (this.left == Vector3.zero)
                return false;
            if (this.right == Vector3.zero)
                return false;
            if (this.top == Vector3.zero)
                return false;
            if (this.bottom == Vector3.zero)
                return false;
            return true;
        }

    }
}
