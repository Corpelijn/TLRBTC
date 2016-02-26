using Assets.Scripts.ObjectPoolNamespace;
using Assets.Scripts.Veins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Vains
{
    public class SVein : Vein
    {
        public SVein(int id)
            : base(id, ObjectPoolTypes.StraightVein)
        {
            this.exits = new Vein[1];
        }
    }
}
