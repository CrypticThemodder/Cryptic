using GorillaLocomotion;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static Cryptic.Settings;

namespace Cryptic.Menu
{
    public class GunLib
    {
        private static int? noInvisLayerMask;

        public static int NoInvisLayerMask()
        {
            noInvisLayerMask ??= ~(
                1 << LayerMask.NameToLayer("TransparentFX") |
                1 << LayerMask.NameToLayer("Ignore Raycast") |
                1 << LayerMask.NameToLayer("Zone") |
                1 << LayerMask.NameToLayer("Gorilla Trigger") |
                1 << LayerMask.NameToLayer("Gorilla Boundary") |
                1 << LayerMask.NameToLayer("GorillaCosmetics") |
                1 << LayerMask.NameToLayer("GorillaParticle"));

            return noInvisLayerMask ?? GTPlayer.Instance.locomotionEnabledLayers;
        }

        public static bool gunLocked;
        public static VRRig lockTarget;

        public static (RaycastHit Ray, GameObject NewPointer) RenderGun(int? overrideLayerMask = null)
        {
            Transform GunTransform = GorillaTagger.Instance.rightHandTransform;

            Vector3 StartPosition = GunTransform.position;
            Vector3 Direction = GunTransform.forward;

            Physics.Raycast(StartPosition + Direction / 4f, Direction, out var Ray, 512f, overrideLayerMask ?? NoInvisLayerMask());

            Vector3 EndPosition = Ray.point;

            if (gunLocked && lockTarget != null)
                EndPosition = lockTarget.transform.position;

            if (EndPosition == Vector3.zero)
                EndPosition = StartPosition + Direction * 512f;

            if (GunPointer == null)
                GunPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            GunPointer.SetActive(true);
            GunPointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            GunPointer.transform.position = EndPosition;

            Renderer PointerRenderer = GunPointer.GetComponent<Renderer>();
            PointerRenderer.material.shader = Shader.Find("GUI/Text Shader");
            PointerRenderer.material.color =
                gunLocked || ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f
                ? new Color32(105, 105, 105, 255)
                : new Color32(50, 50, 50, 255);

            GameObject.Destroy(GunPointer.GetComponent<Collider>());

            if (GunLine == null)
            {
                GameObject line = new GameObject("Cryptic_GunLine");
                GunLine = line.AddComponent<LineRenderer>();
            }

            GunLine.gameObject.SetActive(true);
            GunLine.material.shader = Shader.Find("GUI/Text Shader");
            GunLine.startColor = new Color32(105, 105, 105, 255);
            GunLine.endColor = new Color32(50, 50, 50, 255);
            GunLine.startWidth = 0.025f;
            GunLine.endWidth = 0.025f;
            GunLine.positionCount = 2;
            GunLine.useWorldSpace = true;

            GunLine.SetPosition(0, StartPosition);
            GunLine.SetPosition(1, EndPosition);

            return (Ray, GunPointer);
        }

        private static GameObject GunPointer;
        private static LineRenderer GunLine;

        if (GunLine && !ControllerInputPoller.Instance.RightHand.TriggerFloat(XRNode.RightHand) > 0.5f)
        {
            GunLine.SetActive(false);
            gunLocked.SetActive(false);
        }

        public static void prefix()
        {
            try
            {
                if (GunPointer != null)
                {
                    if (!GunPointer.activeSelf)
                        GameObject.Destroy(GunPointer);
                    else
                        GunPointer.SetActive(false);
                }

                if (GunLine != null)
                {
                    if (!GunLine.gameObject.activeSelf)
                    {
                        GameObject.Destroy(GunLine.gameObject);
                        GunLine = null;
                    }
                    else
                        GunLine.gameObject.SetActive(false);
                }
            }
            catch { }
        }
    }
}
