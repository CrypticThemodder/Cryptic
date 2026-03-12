using BepInEx;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using Cryptic.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static Cryptic.Menu.Main;
using Cryptic.Menu;
using static Cryptic.Settings;
using Cryptic.Mods;
using Cryptic_Free.Mods.Settings;

namespace Cryptic.Mods
{
    public class Movement
    {
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * MovementSettings.flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public static bool lPlat;
        public static bool rPlat;
        public static GameObject LeftPlatform;
        public static GameObject RightPlatform;

        public static void Platforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (!Movement.lPlat)
                {
                    Movement.LeftPlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Movement.LeftPlatform.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    Movement.LeftPlatform.GetComponent<Renderer>().material.color = Color.grey;
                    Movement.LeftPlatform.transform.localScale = new Vector3(0.3f, 1f / 1000f, 0.5f);
                    Movement.LeftPlatform.transform.position = GorillaTagger.Instance.leftHandTransform.position + new Vector3(0.0f, -0.07f, 0.0f);
                    Movement.LeftPlatform.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation * Quaternion.Euler(0.0f, 0.0f, -90f);
                    Movement.lPlat = true;
                }
            }
            else if (Movement.lPlat)
            {
                Rigidbody rigidbody = Movement.LeftPlatform.AddComponent(typeof(Rigidbody)) as Rigidbody;
                rigidbody.useGravity = true;
                GameObject.Destroy((GameObject)Movement.LeftPlatform, 1f);
                GameObject.Destroy((UnityEngine.Object)rigidbody, 1f);
                Movement.lPlat = false;
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (Movement.rPlat)
                    return;
                Movement.RightPlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Movement.RightPlatform.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                Movement.RightPlatform.GetComponent<Renderer>().material.color = Color.grey;
                Movement.RightPlatform.transform.localScale = new Vector3(0.3f, 1f / 1000f, 0.5f);
                Movement.RightPlatform.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0.0f, -0.07f, 0.0f);
                Movement.RightPlatform.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation * Quaternion.Euler(0.0f, 0.0f, -90f);
                Movement.rPlat = true;
            }
            else if (Movement.rPlat)
            {
                Rigidbody rigidbody = Movement.RightPlatform.AddComponent(typeof(Rigidbody)) as Rigidbody;
                rigidbody.useGravity = true;
                GameObject.Destroy((GameObject)Movement.RightPlatform, 1f);
                GameObject.Destroy((UnityEngine.Object)rigidbody, 1f);
                Movement.rPlat = false;
            }
        }

        public static bool previousTeleportTrigger;
        public static void TeleportGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var GunData = GunLib.RenderGun();
                GameObject NewPointer = GunData.NewPointer;
                NewPointer.name = "GunLine";

                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && !previousTeleportTrigger)
                {
                        GTPlayer.Instance.TeleportTo(NewPointer.transform.position + Vector3.up, GTPlayer.Instance.transform.rotation);
                        GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                }

                previousTeleportTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f;
            }
        }

        public static void SlingShot()
        {
            if ((double)ControllerInputPoller.instance.leftControllerIndexFloat <= 0.10000000149011612)
                return;
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(GTPlayer.Instance.bodyCollider.transform.forward * (Time.deltaTime * (20f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void CarMonke()
        {
            if (ControllerInputPoller.instance.rightGrab)
                GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(GTPlayer.Instance.bodyCollider.transform.forward * (Time.deltaTime * (20f / Time.deltaTime)), ForceMode.Acceleration);
            if (!ControllerInputPoller.instance.leftGrab)
                return;
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(GTPlayer.Instance.bodyCollider.transform.forward * (Time.deltaTime * (-20f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void NoClip()
        {
            if ((double)ControllerInputPoller.instance.rightControllerIndexFloat > 0.10000000149011612)
            {
                foreach (Collider collider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    collider.enabled = false;
            }
            else
            {
                foreach (Collider collider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    collider.enabled = true;
            }
        }

        public static void SpeedBoost()
        {
            GTPlayer.Instance.maxJumpSpeed = 9f;
            GTPlayer.Instance.jumpMultiplier = 9f;
        }

        public static void PassBoostCheck()
        {
            if (!ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.maxJumpSpeed = 4.5f;
                GTPlayer.Instance.jumpMultiplier = 4.5f;
            }
            else if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.maxJumpSpeed = 6.5f;
                GTPlayer.Instance.jumpMultiplier = 1.0f;
            }
        }

        public static void LongArms()
        {
            GTPlayer.Instance.GetControllerTransform(true).transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position) * 1.0f;
            GTPlayer.Instance.GetControllerTransform(false).transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position) * 1.0f;
        }

        #region
        public static float startX = -1f;
        public static float startY = -1f;

        public static float subThingy;
        public static float subThingyZ;

        public static Vector3 lastPosition = Vector3.zero;
        public static void WASDFly()
        {

            bool W = UnityInput.Current.GetKey(KeyCode.W);
            bool A = UnityInput.Current.GetKey(KeyCode.A);
            bool S = UnityInput.Current.GetKey(KeyCode.S);
            bool D = UnityInput.Current.GetKey(KeyCode.D);
            bool Space = UnityInput.Current.GetKey(KeyCode.Space);
            bool Ctrl = UnityInput.Current.GetKey(KeyCode.LeftControl);
            bool Shift = UnityInput.Current.GetKey(KeyCode.LeftShift);
            bool Alt = UnityInput.Current.GetKey(KeyCode.LeftAlt);

            bool LeftArrow = UnityInput.Current.GetKey(KeyCode.LeftArrow);
            bool RightArrow = UnityInput.Current.GetKey(KeyCode.RightArrow);
            bool UpArrow = UnityInput.Current.GetKey(KeyCode.UpArrow);
            bool DownArrow = UnityInput.Current.GetKey(KeyCode.DownArrow);

            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;

            Transform parentTransform = GTPlayer.Instance.GetControllerTransform(false).parent;

            float turnSpeed = 250f;

            if (LeftArrow)
                parentTransform.eulerAngles += new Vector3(0, -turnSpeed, 0) * Time.deltaTime;
            if (RightArrow)
                parentTransform.eulerAngles += new Vector3(0, turnSpeed, 0) * Time.deltaTime;
            if (UpArrow)
                parentTransform.eulerAngles += new Vector3(-turnSpeed, 0, 0) * Time.deltaTime;
            if (DownArrow)
                parentTransform.eulerAngles += new Vector3(turnSpeed, 0, 0) * Time.deltaTime;

            if (Mouse.current.rightButton.isPressed)
            {
                Quaternion currentRotation = parentTransform.rotation;
                Vector3 euler = currentRotation.eulerAngles;

                if (startX < 0)
                {
                    startX = euler.y;
                    subThingy = Mouse.current.position.value.x / Screen.width;
                }
                if (startY < 0)
                {
                    startY = euler.x;
                    subThingyZ = Mouse.current.position.value.y / Screen.height;
                }

                float newX = startY - (Mouse.current.position.value.y / Screen.height - subThingyZ) * 360 * 1.33f;
                float newY = startX + (Mouse.current.position.value.x / Screen.width - subThingy) * 360 * 1.33f;

                newX = newX > 180f ? newX - 360f : newX;
                newX = Mathf.Clamp(newX, -90f, 90f);

                parentTransform.rotation = Quaternion.Euler(newX, newY, euler.z);
            }
            else
            {
                startX = -1;
                startY = -1;
            }

            if (W)
                GorillaTagger.Instance.rigidbody.transform.position += GTPlayer.Instance.GetControllerTransform(false).parent.forward * (Time.deltaTime * 9f);

            if (S)
                GorillaTagger.Instance.rigidbody.transform.position += GTPlayer.Instance.GetControllerTransform(false).parent.forward * (Time.deltaTime * -9f);

            if (A)
                GorillaTagger.Instance.rigidbody.transform.position += GTPlayer.Instance.GetControllerTransform(false).parent.right * (Time.deltaTime * -9f);

            if (D)
                GorillaTagger.Instance.rigidbody.transform.position += GTPlayer.Instance.GetControllerTransform(false).parent.right * (Time.deltaTime * 9f);

            if (Space)
                GorillaTagger.Instance.rigidbody.transform.position += new Vector3(0f, Time.deltaTime * 9f, 0f);

            if (Ctrl)
                GorillaTagger.Instance.rigidbody.transform.position += new Vector3(0f, Time.deltaTime * -9f, 0f);

            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
        }



        private static float driveSpeed;
        public static int driveInt;
        public static void ChangeDriveSpeed(bool positive = true)
        {
            float[] speedamounts = { 10f, 30f, 50f, 100f, 3f };
            string[] speedNames = { "Normal", "Fast", "Ultra Fast", "The Flash", "Slow", };

            if (positive)
                driveInt++;
            else
                driveInt--;

            driveInt %= speedamounts.Length;
            if (driveInt < 0)
                driveInt = speedamounts.Length - 1;

            driveSpeed = speedamounts[driveInt];
        }
        #endregion

        public static void LowGravity()
        {
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(GTPlayer.Instance.bodyCollider.transform.up * (Time.deltaTime * (4.4f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void highGravity()
        {
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(-GTPlayer.Instance.bodyCollider.transform.up * (Time.deltaTime * (8f / Time.deltaTime)), ForceMode.Acceleration);
        }
    }
}
