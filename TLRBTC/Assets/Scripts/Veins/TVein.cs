using Assets.Scripts.ObjectPoolNamespace;
using Assets.Scripts.Veins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Vains
{
    public class TVein : Vein
    {
        public TVein(int id)
            : base(id, ObjectPoolTypes.TurnVein)
        {
            this.exits = new Vein[1];
        }
    }
}
