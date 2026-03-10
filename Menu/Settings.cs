using Cryptic.Classes;
using UnityEngine;
using static Cryptic.Menu.Main;

namespace Cryptic
{
    internal class Settings
    {
        public static ExtGradient backgroundColor = new ExtGradient{isRainbow = false};
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient{colors = GetSolidGradient(new Color32(52, 72, 28,255)) }, // Disabled
            new ExtGradient{colors = GetSolidGradient(new Color32(52, 72, 28,255)) }, // Enabled

        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };

        public static ExtGradient[]GunColors = new ExtGradient[]
       {
            new ExtGradient{colors = GetSolidGradient(new Color32(105, 105, 105, 255)) }, // Disabled
            new ExtGradient{colors = GetSolidGradient(new Color32(50, 50, 50, 255)) }, // Enabled

       };

        public static Font currentFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);

        public static bool fpsCounter = false;
        public static bool disconnectButton = true;
        public static bool rightHanded = false;
        public static bool disableNotifications = true;

        public static KeyCode keyboardButton = KeyCode.Q;

        public static Vector3 menuSize = new Vector3(0.001f, 1.015f, 0.98f); // Depth, Width, Height
        public static int buttonsPerPage = 6;
    }
}
