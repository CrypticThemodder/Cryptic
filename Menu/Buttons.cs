using Cryptic.Mods;
using Oculus.Interaction;
using Cryptic.Mods;
using UnityEngine;
using static Cryptic.Settings;
using static Cryptic.Classes.ButtonInfo;
using Cryptic.Classes;
using Cryptic_Free.Mods.Settings;

namespace Cryptic.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "Settings", method =() => Main.Cats(1), isTogglable = false, toolTip = "Opens the main settings page for the menu."},
                new ButtonInfo { buttonText = "Room Mods", method =() => Main.Cats(2), isTogglable = false, toolTip = "Opens the room mods page for the menu."},
                new ButtonInfo { buttonText = "Movement Mods", method =() => Main.Cats(3), isTogglable = false, toolTip = "Opens the room mods page for the menu."},
                new ButtonInfo { buttonText = "Safety Mods", method =() => Main.Cats(4), isTogglable = false, toolTip = "Opens the safety mods page for the menu."},
                new ButtonInfo { buttonText = "Visual Mods", method =() => Main.Cats(5), isTogglable = false, toolTip = "Opens the visual mods page for the menu."},
                new ButtonInfo { buttonText = "Advantage Mods", method =() => Main.Cats(6), isTogglable = false, toolTip = "Opens the advantage mods page for the menu."},
                new ButtonInfo { buttonText = "OP Mods", method =() => Main.Cats(7), isTogglable = false, toolTip = "Opens the op mods page for the menu."},
                new ButtonInfo { buttonText = "Exploit Mods", method =() => Main.Cats(8), isTogglable = false, toolTip = "Opens the fun mods page for the menu."},
                //new ButtonInfo { buttonText = "", method =() => Main.Cats(9), isTogglable = false, toolTip = "Opens the exploit mods page for the menu."},
            },

            new ButtonInfo[] { // [1] Settings
                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => SettingsMods.RightHand(), disableMethod =() => SettingsMods.LeftHand(), toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "Notifications", enableMethod =() => SettingsMods.EnableNotifications(), disableMethod =() => SettingsMods.DisableNotifications(), enabled = !disableNotifications, toolTip = "Toggles the notifications."},
                new ButtonInfo { buttonText = "Gunlib Fix", enableMethod =() => SettingsMods.Nonomoregunfrfrfrfrfrfrfrfrfrfrfr(), disableMethod =() => SettingsMods.Nonomoregunfrfrfrfrfrfrfrfrfrfrfr(), enabled = !disableNotifications, toolTip = "Gunlib Fix."},
                new ButtonInfo { buttonText = "Gunlib Fix", enableMethod =() => SettingsMods.Yesyesgunfrfrfrfrfrfrfrfrfrfrfrfr(), disableMethod =() => SettingsMods.Yesyesgunfrfrfrfrfrfrfrfrfrfrfrfr(), toolTip = "Gunlib Fix."},
            },

            new ButtonInfo[] { // Room Mods [2]

                new ButtonInfo { buttonText = "Disconnect", method =() => NetworkSystem.Instance.ReturnToSinglePlayer(), isTogglable = false, toolTip = "Disconnects you from the room."},
            },

            new ButtonInfo[] { // Movement Mods [3]

                new ButtonInfo { buttonText = "Movement Settings", method =() => Main.Cats(9), isTogglable = false, toolTip = "Opens the Movement Settings page for the menu."},
                new ButtonInfo { buttonText = "Platforms", method =() => Movement.Platforms(), toolTip = "Spawns platforms on your hands when pressing grip."},
                new ButtonInfo { buttonText = "Fly", method =() => Movement.Fly(), toolTip = "Sends you forward when holding A."},
                new ButtonInfo { buttonText = "Teleport Gun", method =() => Movement.TeleportGun(), toolTip = "Teleports you to wherever your pointer is when pressing trigger."},
                new ButtonInfo { buttonText = "SpeedBoost", method =() => Movement.SpeedBoost(), toolTip = "Gives the player a speedboost."},
                new ButtonInfo { buttonText = "CarMonke", method =() => Movement.CarMonke(), toolTip = "Monke go vroom vroom."},
                new ButtonInfo { buttonText = "Slingshot", method =() => Movement.SlingShot(), toolTip = "Pushes you forward when activated."},
                new ButtonInfo { buttonText = "Noclip", method =() => Movement.NoClip(), toolTip = "Lets you noclip through everything."},
                new ButtonInfo { buttonText = "SpeedBoost [A to disable]", method =() => Movement.PassBoostCheck(), toolTip = "Gives the player a speedboost and when [A] is pressed is disabled."},
                new ButtonInfo { buttonText = "WASDFly", method =() => Movement.WASDFly(), toolTip = "Lets the player move on PC using [WASD + ctrl + shift]."},
                new ButtonInfo { buttonText = "LongArms", method =() => Movement.LongArms(), toolTip = "gives the player longer arms. [unoticable]"},
                new ButtonInfo { buttonText = "Low Grav", method =() => Movement.LowGravity(), toolTip = "Player on da moon."},
                new ButtonInfo { buttonText = "High Grav", method =() => Movement.highGravity(), toolTip = "Player not on da moon."},
            },

            new ButtonInfo[] { // Safety Mods [4]

                new ButtonInfo { buttonText = "Anti Report", method =() => Safety.AntiReportDisconnect(), toolTip = "Disconnects you when someone tries to report you."},
                new ButtonInfo { buttonText = "Quit Game | Closes game", method =() => Application.Quit()},
            },

            new ButtonInfo[] { // Visual Mods [5]

                new ButtonInfo { buttonText = "Invis Monke", method =() => Visuals.invis(), toolTip = "Makes you go invisible."},
                new ButtonInfo { buttonText = "Ghost Monke [Kinda Broke]", method =() => Visuals.Ghost(), toolTip = "Makes you rig freeze in place."},
                new ButtonInfo { buttonText = "Tracers", method =() => Visuals.Tracers(), toolTip = "Lets you see where other players are at."},
                new ButtonInfo { buttonText = "Max Quest Score", method =() => Visuals.QuestBadge(), toolTip = "Sets your badge score to the max."},
                new ButtonInfo { buttonText = "Enable Smooth Rig", method =() => Visuals.SmoothRig(), toolTip = "Makes your rig smoother."},
                new ButtonInfo { buttonText = "Disable Smooth Rig", method =() => Visuals.DisableSmoothRig(), toolTip = "Disables smooth rig."},
                new ButtonInfo { buttonText = "Spaz Rig", method =() => Visuals.SpazRig(), toolTip = "Makes Your Rig Go Crazy."},
                new ButtonInfo { buttonText = "Spaz Hands", method =() => Visuals.SpazHands(), toolTip = "Makes your hands go crazy."},
                new ButtonInfo { buttonText = "Fix Head", method =() => Visuals.FixHead(), toolTip = "Fixes your head if other mods break it completely."},
                new ButtonInfo { buttonText = "Backwards Head", method =() => Visuals.BackwardsHead(), toolTip = "Makes you head go backwards."},
                new ButtonInfo { buttonText = "Snap Neck", method =() => Visuals.SnapNeck(), toolTip = "Who's the imposter? | Snaps your neck."},
                new ButtonInfo { buttonText = "Upsidedown Head", method =() => Visuals.UpsidedownHead(), toolTip = "Makes your head go upside down."},
                new ButtonInfo { buttonText = "Name Tags", method =() => Visuals.NameTag(), toolTip = "Displays people names above thier head."},
            },

            new ButtonInfo[] { // Advantage Mods [6]

                new ButtonInfo { buttonText = "Tag Aura", method =() => Advantages.TagAura(), toolTip = "Lets you tag from farther."},
            },

            new ButtonInfo[] { // OP Mods [7]

                new ButtonInfo { buttonText = "Tag Gun", method =() => Overpowered.TagGun(), toolTip = "Tags whoever you desire."},
                new ButtonInfo { buttonText = "Tag All", method =() => Overpowered.TagAll(), toolTip = "Tags everyone."},
            },

            new ButtonInfo[] { // Exploit Mods [8]

                new ButtonInfo { buttonText = "Freeze All", method =() => Exploits.FreezeAll(), toolTip = "TBone Steak but... It's A Gorilla?"},
                new ButtonInfo { buttonText = "Kick Server", method =() => Exploits.KickServer(), toolTip = "Kicks All The Other Players."},
                new ButtonInfo { buttonText = "Lag On Touch", method =() => Exploits.LagOnTouch(), toolTip = "Lags Players That Touch You."},
                new ButtonInfo { buttonText = "Kick On Touch", method =() => Exploits.KickOnTouch(), toolTip = "Kicks Players Who Touch You."},
                new ButtonInfo { buttonText = "Lag All", method =() => Exploits.LagAll(), toolTip = "Lags everyone."},
                new ButtonInfo { buttonText = "Kick Gun", method =() => Exploits.KickGun(), toolTip = "Kick Whoever You Desire."},
                new ButtonInfo { buttonText = "Kick All", method =() => Exploits.KickAll(), toolTip = "Kick Everyone."},
                new ButtonInfo { buttonText = "Enable GreyScreen All", method =() => Exploits.GreyScreenAll(), toolTip = "ENABLE: GreyScreen Everyone."},
                new ButtonInfo { buttonText = "Disable GreyScreen All", method =() => Exploits.GreyScreenAllDisable(), toolTip = "DISABLE: GreyScreen Everyone."},
            },

            new ButtonInfo[] { // Movement Settings[9]

                new ButtonInfo { buttonText = "Change Fly Speed", overlapText = "Change Fly Speed [Normal]", method =() => MovementSettings.ChangeFlySpeed(), toolTip = "Change how fast you go while flying"},
            },
        };
    }
}
