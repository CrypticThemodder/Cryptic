using BepInEx;
using MonoMod.Utils;
using Oculus.Platform;
using Photon.Pun;
using Cryptic.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Device;

namespace Cryptic.Mods
{
    internal class Visuals
    {
        public static bool RunOnce;
        public static bool Toggled;
        public static GameObject Rball, Lball;

        public static void Ghost()
        {
            bool pressed = ControllerInputPoller.instance.rightControllerSecondaryButton
                           || UnityInput.Current.GetKey(KeyCode.E);


            if (pressed && !RunOnce)
            {
                Toggled = !Toggled;
                RunOnce = true;
            }

            if (!pressed)
                RunOnce = false;

            var rig = GorillaTagger.Instance.offlineVRRig;
            rig.enabled = !Toggled;


            if (Toggled)
            {
                if (Rball == null && Lball == null)
                {
                    Rball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Lball = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                    Rball.transform.parent = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform;
                    Lball.transform.parent = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform;

                    Rball.transform.localPosition = Vector3.zero;
                    Lball.transform.localPosition = Vector3.zero;

                    Rball.transform.localRotation = Quaternion.identity;
                    Lball.transform.localRotation = Quaternion.identity;

                    Rball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Lball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    GameObject.Destroy(Rball.GetComponent<Collider>());
                    GameObject.Destroy(Lball.GetComponent<Collider>());
                }
            }
            else
            {
                GameObject.Destroy(Rball);
                GameObject.Destroy(Lball);
                Rball = null;
                Lball = null;
            }

        }
        public static void invis()
        {
            if (ControllerInputPoller.instance.rightControllerSecondaryButton)
                GorillaTagger.Instance.offlineVRRig.headBodyOffset.x = 180f;
            else
                GorillaTagger.Instance.offlineVRRig.headBodyOffset.x = 0.0f;
        }

        public static void Tracers()
        {
            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject line = new GameObject("Line");
                    LineRenderer liner = line.AddComponent<LineRenderer>();
                    UnityEngine.Color thecolor = vrrig.playerColor;
                    liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.010f; liner.endWidth = 0.010f; liner.positionCount = 2; liner.useWorldSpace = true;
                    liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                    liner.SetPosition(1, vrrig.transform.position);
                    liner.material.shader = Shader.Find("GUI/Text Shader");
                    UnityEngine.Object.Destroy(line, Time.deltaTime);
                }
            }
        }

        public static void QuestBadge()
        {
            VRRig.LocalRig.SetQuestScore(99999);
        }

        public static void SmoothRig()
        {
            PhotonNetwork.SerializationRate = 30;
        }

        public static void DisableSmoothRig()
        {
            PhotonNetwork.SerializationRate = 10;
        }

        public static void SpazRig()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }

        public static void SpazHands()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }

        public static void FixHead()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 0f;
        }

        public static void BackwardsHead()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 180f;
        }

        public static void SnapNeck()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 90f;
        }

        public static void UpsidedownHead()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 180f;
        }

        public static void NameTag()
        {
            VRRig rig = new VRRig();
            GameObject NameTag;
            foreach (VRRig Player in VRRigCache.ActiveRigs)
            {
                    var NameTagText = new GameObject("NAMELABEL");
                    NameTag = NameTagText;
                    NameTagText.transform.SetParent(Player.transform);
                    NameTagText.transform.localPosition = new Vector3(0f, 1f, 0f);
                    NameTagText.transform.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
                    NameTagText.transform.Rotate(0f, 180f, 0f);

                    var text = NameTagText.AddComponent<TextMeshPro>();
                    text.text = "Name: " + rig.Creator.NickName.ToUpper();
                    text.fontSize = 1;
                    text.alignment = TextAlignmentOptions.Center;
                    text.color = Color.yellow;
                    text.enableAutoSizing = true;
                    text.rectTransform.sizeDelta = new Vector2(500f, 400f);
                    text.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    GameObject.Destroy(NameTag, 0.02f);
            }
        }
    }
}
