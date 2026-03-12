using System;
using System.Collections.Generic;
using System.Text;
using static Cryptic.Menu.Main;

namespace Cryptic_Free.Mods.Settings
{
    internal class MovementSettings
    {
        public static int flySpeedIndex = 2;
        public static float flySpeed = 15f;

        public static void ChangeFlySpeed()
        {
            string[] speedNames = new string[] { "Very Slow", "Slow", "Normal", "Fast", "Very Fast", "Extreme" };
            float[] speedValues = new float[] { 5f, 10f, 15f, 20f, 30f, 50f };

            flySpeedIndex++;
            flySpeedIndex %= speedNames.Length;

            GetIndex("Change Fly Speed").overlapText = $"Change Fly Speed [{speedNames[flySpeedIndex]}]";
        }
    }
}
