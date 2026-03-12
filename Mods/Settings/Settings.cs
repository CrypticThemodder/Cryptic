using UnityEngine;
using static Cryptic.Menu.Main;
using static Cryptic.Settings;
using Cryptic.Mods;

namespace Cryptic.Mods
{
    internal class SettingsMods
    {
        public static void EnterSettings()
        {
            buttonsType = 1;
        }

        public static void RoomMods()
        {
            buttonsType = 2;
        }

        public static void MovementMods()
        {
            buttonsType = 3;
        }

        public static void SafetyMods()
        {
            buttonsType = 4;
        }

        public static void VisualMods()
        {
            buttonsType = 5;
        }

        public static void AdvantageMods()
        {
            buttonsType = 6;
        }

        public static void OPMods()
        {
            buttonsType = 7;
        }

        public static void ExploitMods()
        {
            buttonsType = 8;
        }

        public static void Movementsettings()
        {
            buttonsType = 9;
        }

        public static void RightHand()
        {
            rightHanded = true;
        }

        public static void LeftHand()
        {
            rightHanded = false;
        }

        public static void EnableFPSCounter()
        {
            fpsCounter = true;
        }

        public static void DisableFPSCounter()
        {
            fpsCounter = false;
        }

        public static void EnableNotifications()
        {
            disableNotifications = false;
        }

        public static void DisableNotifications()
        {
            disableNotifications = true;
        }

        public static void EnableDisconnectButton()
        {
            disconnectButton = true;
        }

        public static void DisableDisconnectButton()
        {
            disconnectButton = false;
        }

        public static void Nonomoregunfrfrfrfrfrfrfrfrfrfrfr()
        {
            GameObject.Find("iiMenu_GunLine").SetActive(false);
            GameObject.Find("GunLine").SetActive(false);
        }
        public static void Yesyesgunfrfrfrfrfrfrfrfrfrfrfrfr()
        {
            GameObject.Find("iiMenu_GunLine").SetActive(true);
            GameObject.Find("GunLine").SetActive(true);

        }
    }
}
