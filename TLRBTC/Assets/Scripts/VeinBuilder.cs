using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Veins;
using System;
using Assets.Scripts.Vains;
using Assets.Scripts.ObjectPoolNamespace;

namespace Assets.Scripts
{
    public class VeinBuilder : MonoBehaviour
    {
        public TextAsset[] veindata;

        public int drawDistance = 5;

        private List<Vein> veins;
        private List<Vein> visibleVeins;

        public void Start()
        {
            INSTANCE = this;
            veins = new List<Vein>();
            visibleVeins = new List<Vein>();

            foreach (TextAsset ta in veindata)
            {
                string[] lines = ta.text.Split(new char[] { '\n' });
                foreach (string line in lines)
                {
                    if (line.StartsWith("v"))
                    {
                        int aantal = Convert.ToInt32(line.Substring(4, line.Length - 4));

                        for (int i = 0; i < aantal; i++)
                        {
                            veins.Add(new TVein(i + 1));
                        }
                    }
                }
            }

            for (int i = 0; i < veins.Count; i++)
            {
                Vein next = veins.Count - 1 <= i ? veins[0] : veins[i + 1];
                Vein current = veins[i];

                current.SetExit(0, next);
                next.SetEntry(current);
            }
        }

        public void Update()
        {
            // Get the current vein the player is in
            GameObject go = Player.INSTANCE.currentVein;
            Vein currentVein = null;
            if (go == null)
            {
                currentVein = this.veins[0];
            }
            else
            {
                VeinObject vo = go.GetComponent<VeinObject>();
                if (vo == null) return;
                currentVein = VeinBuilder.GetVain(vo.id);
            }

            // Draw the current vein
            currentVein.DrawMe(null);

            // Keep track of the vain
            if (!visibleVeins.Contains(currentVein)) visibleVeins.Add(currentVein);

            // Draw the rest of the visible distance of veins
            for (int i = 0; i < drawDistance - 1; i++)
            {
                // Get the straight out vain
                VeinExitPoint connectInfo = currentVein.GetStraightVeinExitInformation();
                currentVein = currentVein.GetStraight();

                // Draw the next vain
                currentVein.DrawMe(connectInfo);

                // Keep track of the drawn vains
                if (!visibleVeins.Contains(currentVein)) visibleVeins.Add(currentVein);
            }

            // Loop trought all the visible veins
            int index = 0;
            while (index < visibleVeins.Count)
            {
                // Check if the current vein has been currently drawn
                if (visibleVeins[index].GetDrawCalls() >= 0)
                {
                    // Remove one drawcall
                    visibleVeins[index].RemoveDrawCall();
                }
                else
                {
                    // The vein has not been drawn very often, remove it
                    visibleVeins[index].DestoryGameObject();
                    visibleVeins.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }

        public static VeinBuilder INSTANCE { private set; get; }

        public static Vein GetVain(int id)
        {
            foreach (Vein v in INSTANCE.veins)
            {
                if (v.GetId() == id)
                {
                    return v;
                }
            }

            return null;
        }
    }
}