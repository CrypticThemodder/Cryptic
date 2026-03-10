using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cryptic.Mods
{
    internal class Advantages
    {
        private static readonly Dictionary<int, float> lastTaggedTime = new Dictionary<int, float>();
        private static Photon.Realtime.Player? player;
        public static float Delay;
        public static void TagAura()
        {
            if (GorillaTagger.Instance == null) return;

            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && (Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, vrrig.headMesh.transform.position) < 4f || Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, vrrig.headMesh.transform.position) < 4f))
                {
                    PhotonView photonView = GameObject.Find("Player Objects/RigCache/Network Parent/GameMode(Clone)").GetPhotonView();
                    if (photonView)
                    {
                        photonView.RPC("RPC_ReportTag", RpcTarget.All, new object[]
                        {
                            vrrig.Creator.ActorNumber
                        });
                    }
                }
            }
        }
    }
}
