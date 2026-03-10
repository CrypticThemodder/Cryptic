using GorillaGameModes;
using GorillaLocomotion;
using GorillaTagScripts;
using Photon.Pun;
using Photon.Realtime;
using Cryptic.Classes;
using Cryptic.Menu;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.XR;
using static Cryptic.Menu.Main;

namespace Cryptic.Mods
{
    internal class Overpowered
    {
        private static float tagGunDelay;
        public static void TagPlayer(VRRig p)
        {
            tagGunDelay = Time.time + 0.2f;
            if (p != GorillaTagger.Instance.offlineVRRig)
            {
                if (!p.mainSkin.material.name.Contains("fected") && !p.mainSkin.material.name.Contains("ice"))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = p.transform.position;

                    GameMode.ReportTag(p.OwningNetPlayer);
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }
        private static float TagAllDelay;
        public static void TagAll()
        {
            TagAllDelay = Time.time + 0.2f;
            foreach (VRRig p in VRRigCache.ActiveRigs)
            {
                if (p != GorillaTagger.Instance.offlineVRRig)
                {
                    TagPlayer(p);
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

       public static bool previousTagTrigger;
        public static VRRig p;
        public static VRRig player;
        public static VRRig rig;

        public static bool previousGunTrigger;
        public static void TagGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var GunData = GunLib.RenderGun();
                GameObject NewPointer = GunData.NewPointer;
                NewPointer.name = "GunLine";

                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && !previousGunTrigger)
                {
                    Overpowered.TagPlayer(GunLib.lockTarget);
                }

                previousGunTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f;
            }
        }

        public static void BanAll()
        {
            PhotonNetwork.CloseConnection(PhotonNetwork.LocalPlayer);
            PhotonNetwork.SendDestroyOfAll();
            PhotonNetwork.DestroyAll(false);
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.CloseConnection(player);
                PhotonNetwork.DestroyPlayerObjects(player);
                PhotonNetwork.SendDestroyOfPlayer(player.ActorNumber);
                PhotonNetwork.SetMasterClient(player);
                PhotonNetwork.OpRemoveCompleteCache();
            }
        }
    }
}