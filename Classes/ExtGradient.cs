using System;
using UnityEngine;

namespace Cryptic.Classes
{
    public class ExtGradient
    {
        public GradientColorKey[] colors = new GradientColorKey[]
        {
            new GradientColorKey(new Color32(105, 105, 105, 255), 0f),

        };

        public bool isRainbow = false;
        public bool copyRigColors = false;
    }
}
